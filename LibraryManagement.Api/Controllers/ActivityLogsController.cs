using LibraryManagement.Application.ActivityLogs.DTOs;
using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{

    [ApiController]
    [Route("api/activity-logs")]
    [Authorize(Roles = RoleNames.Administrator)]
    public class ActivityLogsController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogsController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ActivityLogResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var logs = await _activityLogService.GetAllAsync(cancellationToken);

            return Ok(logs);
        }
    }
}
