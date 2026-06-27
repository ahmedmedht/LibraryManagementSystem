using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.ActivityLogs.Interfaces
{
    public interface IActivityLogRepository
    {
        Task<IReadOnlyList<ActivityLog>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(ActivityLog log, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
