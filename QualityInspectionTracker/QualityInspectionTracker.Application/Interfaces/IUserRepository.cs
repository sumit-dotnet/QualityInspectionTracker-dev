using QualityInspectionTracker.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<User> CreateAsync(
        User user,
        CancellationToken cancellationToken = default);

        Task<bool> UsernameExistsAsync(
            string username,
            CancellationToken cancellationToken = default);

        Task<List<User>> GetSupervisorsAsync(
            CancellationToken cancellationToken = default);
    }
}
