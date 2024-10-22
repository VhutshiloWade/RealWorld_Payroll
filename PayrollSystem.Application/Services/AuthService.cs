using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PayrollSystem.API.Settings;
using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;
using BCrypt.Net;

namespace PayrollSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtGenerator _jwtGenerator;

        public AuthService(IUserRepository userRepository, IJwtGenerator jwtGenerator)
        {
            _userRepository = userRepository;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            return _jwtGenerator.GenerateToken(user);
        }

        public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByUsernameAsync(registerUserDto.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Create new user
            var newUser = new User
            {
                Username = registerUserDto.Username,
                Email = registerUserDto.Email,
                PasswordHash = HashPassword(registerUserDto.Password),
                Role = registerUserDto.Role
            };

            // Save user to database
            await _userRepository.AddAsync(newUser);

            // Return UserDto (without sensitive information)
            return new UserDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                Role = newUser.Role
            };
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Verify(password, storedHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.HashPassword(password);
        }
    }
}
