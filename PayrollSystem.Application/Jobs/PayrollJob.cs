using Quartz;
using PayrollSystem.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace PayrollSystem.Application.Jobs
{
    public class PayrollJob : IJob
    {
        private readonly IPayrollService _payrollService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<PayrollJob> _logger;

        public PayrollJob(IPayrollService payrollService, INotificationService notificationService, ILogger<PayrollJob> logger)
        {
            _payrollService = payrollService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting automated payroll run at {time}", DateTimeOffset.Now);

            try
            {
                // Assuming we're running payroll for the previous month
                var now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                _logger.LogInformation("Processing payroll for period: {startDate} to {endDate}", startDate, endDate);

                await _payrollService.ProcessPayrollForAllEmployeesAsync(startDate, endDate);

                _logger.LogInformation("Completed automated payroll run at {time}", DateTimeOffset.Now);

                await _notificationService.SendAdminAlertAsync(
                    "Payroll Processing Completed",
                    $"Payroll processing for the period {startDate:d} to {endDate:d} has been completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during automated payroll run");

                await _notificationService.SendAdminAlertAsync(
                    "Payroll Processing Error",
                    $"An error occurred during the automated payroll run: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");

                // Rethrow the exception to mark the job as failed in Quartz
                throw;
            }
        }
    }
}
