using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ActivityLog>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.ActivityLogs
                .AsNoTracking()
                .OrderByDescending(log => log.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(
            ActivityLog log,
            CancellationToken cancellationToken = default)
        {
            await _context.ActivityLogs.AddAsync(log, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
