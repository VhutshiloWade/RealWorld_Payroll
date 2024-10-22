namespace PayrollSystem.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendAdminAlertAsync(string subject, string message);
        Task SendWorkflowNotificationAsync(string to, string subject, string body);
    }
}
