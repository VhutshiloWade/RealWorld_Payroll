using PayrollSystem.Application.DTOs;
using System.Threading.Tasks;

namespace PayrollSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password);
        Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto);
    }
}
