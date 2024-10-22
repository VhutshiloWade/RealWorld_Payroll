using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;
using FluentValidation;
using PayrollSystem.Application.Validators;

namespace PayrollSystem.Application.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly LeaveDtoValidator _leaveDtoValidator;
        private readonly IWorkflowService _workflowService;

        public LeaveService(
            ILeaveRepository leaveRepository,
            ILeaveBalanceRepository leaveBalanceRepository,
            IEmployeeRepository employeeRepository,
            LeaveDtoValidator leaveDtoValidator,
            IWorkflowService workflowService)
        {
            _leaveRepository = leaveRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
            _employeeRepository = employeeRepository;
            _leaveDtoValidator = leaveDtoValidator;
            _workflowService = workflowService;
        }

        public async Task<LeaveDto> RequestLeaveAsync(LeaveDto leaveRequest)
        {
            var validationResult = await _leaveDtoValidator.ValidateAsync(leaveRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var leaveBalance = await _leaveBalanceRepository.GetByEmployeeIdAndTypeAndYearAsync(
                leaveRequest.EmployeeId, leaveRequest.LeaveType, DateTime.Now.Year);

            if (leaveBalance == null || leaveBalance.Balance < (leaveRequest.EndDate - leaveRequest.StartDate).Days)
            {
                throw new InvalidOperationException("Insufficient leave balance");
            }

            var leave = new Leave
            {
                EmployeeId = leaveRequest.EmployeeId,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                LeaveType = leaveRequest.LeaveType,
                Status = LeaveStatus.Pending,
                Comments = leaveRequest.Comments
            };

            await _leaveRepository.AddAsync(leave);

            // Initiate workflow
            await _workflowService.InitiateWorkflowAsync("LeaveRequest", leave.Id);

            return MapToDto(leave);
        }

        public async Task<LeaveDto> UpdateLeaveStatusAsync(int leaveId, LeaveStatus status)
        {
            var leave = await _leaveRepository.GetByIdAsync(leaveId);
            if (leave == null)
                throw new KeyNotFoundException($"Leave with ID {leaveId} not found.");

            leave.Status = status;
            await _leaveRepository.UpdateAsync(leave);

            if (status == LeaveStatus.Approved)
            {
                var leaveDays = (leave.EndDate - leave.StartDate).Days + 1;
                await UpdateLeaveBalanceAsync(leave.EmployeeId, leave.LeaveType, -leaveDays, leave.StartDate.Year);
            }

            return MapToDto(leave);
        }

        public async Task<IEnumerable<LeaveDto>> GetEmployeeLeavesAsync(int employeeId)
        {
            var leaves = await _leaveRepository.GetByEmployeeIdAsync(employeeId);
            return leaves.Select(MapToDto);
        }

        public async Task<decimal> CalculateLeaveAdjustmentAsync(int employeeId, DateTime payPeriodStart, DateTime payPeriodEnd)
        {
            var leaves = await _leaveRepository.GetByEmployeeIdAndDateRangeAsync(employeeId, payPeriodStart, payPeriodEnd);
            
            decimal adjustment = 0;
            foreach (var leave in leaves)
            {
                if (leave.LeaveType == LeaveType.Unpaid && leave.Status == LeaveStatus.Approved)
                {
                    var leaveDays = (leave.EndDate - leave.StartDate).Days + 1;
                    var employee = await _employeeRepository.GetByIdAsync(employeeId);
                    var dailyRate = employee.BaseSalary / 22; // Assuming 22 working days in a month
                    adjustment -= leaveDays * dailyRate;
                }
            }

            return adjustment;
        }

        public async Task<LeaveBalanceDto> GetLeaveBalanceAsync(int employeeId, int year)
        {
            var balances = await _leaveBalanceRepository.GetByEmployeeIdAndYearAsync(employeeId, year);
            
            return new LeaveBalanceDto
            {
                EmployeeId = employeeId,
                Year = year,
                Balances = balances.ToDictionary(b => b.LeaveType, b => b.Balance)
            };
        }

        public async Task UpdateLeaveBalanceAsync(int employeeId, LeaveType leaveType, decimal adjustment, int year)
        {
            var leaveBalance = await _leaveBalanceRepository.GetByEmployeeIdAndTypeAndYearAsync(employeeId, leaveType, year);
            
            if (leaveBalance == null)
            {
                leaveBalance = new LeaveBalance
                {
                    EmployeeId = employeeId,
                    LeaveType = leaveType,
                    Balance = adjustment,
                    Year = year
                };
                await _leaveBalanceRepository.AddAsync(leaveBalance);
            }
            else
            {
                leaveBalance.Balance += adjustment;
                await _leaveBalanceRepository.UpdateAsync(leaveBalance);
            }
        }

        public async Task InitializeLeaveBalancesAsync(int employeeId, int year)
        {
            var leaveTypes = Enum.GetValues(typeof(LeaveType)).Cast<LeaveType>();
            foreach (var leaveType in leaveTypes)
            {
                var initialBalance = GetInitialBalance(leaveType);
                await UpdateLeaveBalanceAsync(employeeId, leaveType, initialBalance, year);
            }
        }

        private decimal GetInitialBalance(LeaveType leaveType)
        {
            // This is a simplified example. In a real system, these values might be configurable or based on employee contracts.
            return leaveType switch
            {
                LeaveType.Annual => 20, // 20 days of annual leave
                LeaveType.Sick => 10,   // 10 days of sick leave
                LeaveType.Maternity => 0, // Maternity leave is typically handled differently
                LeaveType.Paternity => 0, // Paternity leave is typically handled differently
                LeaveType.Unpaid => 0,   // Unpaid leave doesn't typically have a balance
                _ => 0
            };
        }

        private LeaveDto MapToDto(Leave leave)
        {
            return new LeaveDto
            {
                Id = leave.Id,
                EmployeeId = leave.EmployeeId,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                LeaveType = leave.LeaveType,
                Status = leave.Status,
                Comments = leave.Comments
            };
        }
    }
}
