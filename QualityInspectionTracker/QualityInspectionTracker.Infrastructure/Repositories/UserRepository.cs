using Microsoft.EntityFrameworkCore;
using QualityInspectionTracker.Application.Interfaces;
using QualityInspectionTracker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Username == username &&
                         x.IsActive,
                    cancellationToken);
        }
        public async Task<bool> UsernameExistsAsync(
        string username,
        CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AnyAsync(
                    x => x.Username == username,
                    cancellationToken);
        }

        public async Task<User> CreateAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(
                user,
                cancellationToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            return user;
        }

        public async Task<List<User>> GetSupervisorsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(x => x.Role == "Supervisor")
                .OrderBy(x => x.DisplayName)
                .ToListAsync(cancellationToken);
        }
    }
}
