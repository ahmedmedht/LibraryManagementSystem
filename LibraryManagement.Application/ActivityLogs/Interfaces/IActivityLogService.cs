using LibraryManagement.Application.ActivityLogs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.ActivityLogs.Interfaces
{
    public interface IActivityLogService
    {
        Task<IReadOnlyList<ActivityLogResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
