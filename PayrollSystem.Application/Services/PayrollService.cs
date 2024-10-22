using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;
using FluentValidation;
using PayrollSystem.Application.Validators;
using Microsoft.Extensions.Logging;

namespace PayrollSystem.Application.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITaxBracketRepository _taxBracketRepository;
        private readonly ILeaveService _leaveService;
        private readonly PayrollCalculationRequestDtoValidator _payrollCalculationRequestValidator;
        private readonly IBenefitsService _benefitsService;
        private readonly IShiftOvertimeService _shiftOvertimeService;
        private readonly ILogger<PayrollService> _logger;
        private readonly ITaxCalculator _taxCalculator;

        public PayrollService(
            IPayrollRepository payrollRepository,
            IEmployeeRepository employeeRepository,
            ITaxBracketRepository taxBracketRepository,
            ILeaveService leaveService,
            PayrollCalculationRequestDtoValidator payrollCalculationRequestValidator,
            IBenefitsService benefitsService,
            IShiftOvertimeService shiftOvertimeService,
            ILogger<PayrollService> logger,
            ITaxCalculator taxCalculator)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
            _taxBracketRepository = taxBracketRepository;
            _leaveService = leaveService;
            _payrollCalculationRequestValidator = payrollCalculationRequestValidator;
            _benefitsService = benefitsService;
            _shiftOvertimeService = shiftOvertimeService;
            _logger = logger;
            _taxCalculator = taxCalculator;
        }

        public async Task<PayrollDto> CalculatePayrollAsync(PayrollCalculationRequestDto request)
        {
            var validationResult = await _payrollCalculationRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {request.EmployeeId} not found.");
            }

            var taxBracket = await _taxBracketRepository.GetByIdAsync(employee.TaxBracketId);
            if (taxBracket == null)
            {
                throw new KeyNotFoundException($"Tax bracket with ID {employee.TaxBracketId} not found.");
            }

            decimal baseSalary = employee.BaseSalary;
            decimal overtimeHours = await _shiftOvertimeService.CalculateOvertimeHoursAsync(request.EmployeeId, request.PayPeriodStart, request.PayPeriodEnd);
            decimal shiftDifferential = await _shiftOvertimeService.CalculateShiftDifferentialAsync(request.EmployeeId, request.PayPeriodStart, request.PayPeriodEnd);

            decimal grossSalary = CalculateGrossSalary(baseSalary, overtimeHours, request.BonusPercentage, shiftDifferential);
            decimal taxDeduction = CalculateTaxDeduction(grossSalary, taxBracket);
            decimal leaveAdjustment = await _leaveService.CalculateLeaveAdjustmentAsync(request.EmployeeId, request.PayPeriodStart, request.PayPeriodEnd);
            decimal benefitsDeduction = await _benefitsService.CalculateTotalBenefitsCostAsync(request.EmployeeId);

            var payroll = new Payroll
            {
                Employee = employee,
                EmployeeId = request.EmployeeId,
                PayPeriodStart = request.PayPeriodStart,
                PayPeriodEnd = request.PayPeriodEnd,
                GrossSalary = grossSalary,
                TaxDeduction = taxDeduction,
                NetSalary = grossSalary - taxDeduction - benefitsDeduction
            };

            await _payrollRepository.AddAsync(payroll);

            return MapToDto(payroll);
        }

        private decimal CalculateGrossSalary(decimal baseSalary, decimal overtimeHours, decimal bonusPercentage, decimal shiftDifferential)
        {
            decimal regularPay = baseSalary;
            decimal overtimePay = (baseSalary / 160) * 1.5m * overtimeHours; // Assuming 160 working hours per month
            decimal bonus = baseSalary * (bonusPercentage / 100);
            return regularPay + overtimePay + bonus + shiftDifferential;
        }

        private decimal CalculateTaxDeduction(decimal grossSalary, TaxBracket taxBracket)
        {
            return grossSalary * (taxBracket.Rate / 100);
        }

        
        public async Task ProcessPayrollForAllEmployeesAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Starting payroll processing for all employees from {startDate} to {endDate}", startDate, endDate);

            var employees = await _employeeRepository.GetAllAsync();
            _logger.LogInformation("Retrieved {count} employees for payroll processing", employees.Count());

            foreach (var employee in employees)
            {
                try
                {
                    _logger.LogInformation("Processing payroll for employee {employeeId}", employee.Id);

                    var request = new PayrollCalculationRequestDto
                    {
                        EmployeeId = employee.Id,
                        PayPeriodStart = startDate,
                        PayPeriodEnd = endDate,
                        OvertimeHours = 0,
                        BonusPercentage = 0
                    };

                    var payroll = await CalculatePayrollAsync(request);

                    _logger.LogInformation("Payroll processed successfully for employee {employeeId}. Gross Salary: {grossSalary}, Net Salary: {netSalary}", 
                        employee.Id, payroll.GrossSalary, payroll.NetSalary);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing payroll for employee {employeeId}", employee.Id);
                    // Consider whether to continue processing other employees or throw the exception
                }
            }

            _logger.LogInformation("Completed payroll processing for all employees");
        }

        public async Task<IEnumerable<PayrollDto>> GetPayrollHistoryForEmployeeAsync(int employeeId)
        {
            var payrolls = await _payrollRepository.GetByEmployeeIdAsync(employeeId);
            return payrolls.Select(p => MapToDto(p));
        }

        public async Task<IEnumerable<PayrollDto>> GetPayrollsByPeriodAsync(DateTime payPeriodStart, DateTime payPeriodEnd)
        {
            var payrolls = await _payrollRepository.GetByPayPeriodAsync(payPeriodStart, payPeriodEnd);
            return payrolls.Select(p => MapToDto(p));
        }

        public async Task<YearToDateTotalsDto> GetYearToDateTotalsAsync(int employeeId, int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);

            var payrolls = await _payrollRepository.GetByEmployeeIdAndDateRangeAsync(employeeId, startDate, endDate);

            var ytdTotals = new YearToDateTotalsDto
            {
                GrossSalary = payrolls.Sum(p => p.GrossSalary),
                BaseSalary = payrolls.Sum(p => p.BaseSalary),
                OvertimePay = payrolls.Sum(p => p.OvertimePay),
                Bonus = payrolls.Sum(p => p.Bonus),
                TaxDeduction = payrolls.Sum(p => p.TaxDeduction),
                MedicalAidDeduction = payrolls.Sum(p => p.MedicalAidDeduction),
                RetirementContribution = payrolls.Sum(p => p.RetirementContribution),
                UnionDues = payrolls.Sum(p => p.UnionDues),
                NetSalary = payrolls.Sum(p => p.NetSalary)
            };

            return ytdTotals;
        }

        public async Task<decimal> CalculateAnnualTaxReconciliationAsync(int employeeId, int year)
        {
            var ytdTotals = await GetYearToDateTotalsAsync(employeeId, year);
            var taxBrackets = await _taxBracketRepository.GetAllAsync();

            return _taxCalculator.CalculateAnnualTaxReconciliation(
                ytdTotals.GrossSalary - ytdTotals.RetirementContribution,
                ytdTotals.TaxDeduction,
                taxBrackets,
                12000 // Assuming annual tax credits of 12000
            );
        }

        private PayrollDto MapToDto(Payroll payroll)
        {
            return new PayrollDto
            {
                EmployeeId = payroll.EmployeeId,
                PayPeriodStart = payroll.PayPeriodStart,
                PayPeriodEnd = payroll.PayPeriodEnd,
                GrossSalary = payroll.GrossSalary,
                BaseSalary = payroll.BaseSalary,
                OvertimePay = payroll.OvertimePay,
                Bonus = payroll.Bonus,
                TaxDeduction = payroll.TaxDeduction,
                MedicalAidDeduction = payroll.MedicalAidDeduction,
                RetirementContribution = payroll.RetirementContribution,
                UnionDues = payroll.UnionDues,
                NetSalary = payroll.NetSalary
            };
        }

        public async Task<PayrollDto> CalculatePayrollAsync(int employeeId, DateTime startDate, DateTime endDate, decimal overtimeHours, decimal bonusPercentage)
        {
            var request = new PayrollCalculationRequestDto
            {
                EmployeeId = employeeId,
                PayPeriodStart = startDate,
                PayPeriodEnd = endDate,
                OvertimeHours = overtimeHours,
                BonusPercentage = bonusPercentage
            };
            return await CalculatePayrollAsync(request);
        }
    }
}
