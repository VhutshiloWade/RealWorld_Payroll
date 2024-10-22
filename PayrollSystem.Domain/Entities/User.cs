namespace PayrollSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        
    }

    public enum UserRole
    {
        Employee,
        Manager,
        HRAdmin,
        SystemAdmin
    }
}
