using LibraryManagement.Application.ActivityLogs.DTOs;
using LibraryManagement.Application.Common.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.ActivityLogs.Interfaces.Services
{
    public class ActivityLogService : IActivityLogService, IActivityLogger
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly ICurrentUserService _currentUserService;

        public ActivityLogService(
            IActivityLogRepository activityLogRepository,
            ICurrentUserService currentUserService)
        {
            _activityLogRepository = activityLogRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IReadOnlyList<ActivityLogResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var logs = await _activityLogRepository.GetAllAsync(cancellationToken);

            return logs
                .Select(MapToResponse)
                .ToList();
        }

        public async Task LogAsync(
            ActivityAction action,
            string entityName,
            string? entityId,
            string description,
            CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId ?? "System";

            await LogAsync(
                userId,
                action,
                entityName,
                entityId,
                description,
                cancellationToken);
        }

        public async Task LogAsync(
            string userId,
            ActivityAction action,
            string entityName,
            string? entityId,
            string description,
            CancellationToken cancellationToken = default)
        {
            var log = new ActivityLog
            {
                UserId = userId,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            await _activityLogRepository.AddAsync(log, cancellationToken);
            await _activityLogRepository.SaveChangesAsync(cancellationToken);
        }

        private static ActivityLogResponse MapToResponse(ActivityLog log)
        {
            return new ActivityLogResponse
            {
                Id = log.Id,
                UserId = log.UserId,
                Action = log.Action,
                EntityName = log.EntityName,
                EntityId = log.EntityId,
                Description = log.Description,
                CreatedAt = log.CreatedAt
            };
        }
    }
}
