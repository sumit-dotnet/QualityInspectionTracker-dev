using QualityInspectionTracker.Application.DTOs;
using QualityInspectionTracker.Application.Interfaces;
using QualityInspectionTracker.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<SupervisorDto> CreateSupervisorAsync(
            CreateSupervisorRequest request,
            CancellationToken cancellationToken = default)
        {
            var username = request.Username.Trim();

            var exists = await _userRepository.UsernameExistsAsync(
                username,
                cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException(
                    "Username already exists.");
            }

            var passwordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password);

            var user = new User
            {
                Username = username,

                PasswordHash = passwordHash,

                DisplayName = request.DisplayName.Trim(),

                // IMPORTANT:
                // Role is controlled by backend.
                Role = "Supervisor",

                IsActive = true,

                CreatedAt = DateTime.UtcNow
            };

            var createdUser =
                await _userRepository.CreateAsync(
                    user,
                    cancellationToken);

            return new SupervisorDto
            {
                Id = createdUser.Id,
                Username = createdUser.Username,
                DisplayName = createdUser.DisplayName,
                Role = createdUser.Role,
                IsActive = createdUser.IsActive,
                CreatedAt = createdUser.CreatedAt
            };
        }

        public async Task<List<SupervisorDto>> GetSupervisorsAsync(
            CancellationToken cancellationToken = default)
        {
            var users =
                await _userRepository.GetSupervisorsAsync(
                    cancellationToken);

            return users.Select(x => new SupervisorDto
            {
                Id = x.Id,
                Username = x.Username,
                DisplayName = x.DisplayName,
                Role = x.Role,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
    }
}
