using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.ActivityLogs.Interfaces
{
    public interface IActivityLogger
    {
        Task LogAsync(
            ActivityAction action,
            string entityName,
            string? entityId,
            string description,
            CancellationToken cancellationToken = default);

        Task LogAsync(
            string userId,
            ActivityAction action,
            string entityName,
            string? entityId,
            string description,
            CancellationToken cancellationToken = default);
    }
}
