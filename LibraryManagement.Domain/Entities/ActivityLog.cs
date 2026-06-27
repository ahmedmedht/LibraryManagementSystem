using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class ActivityLog
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public ActivityAction Action { get; set; }

        public string EntityName { get; set; } = string.Empty;

        public string? EntityId { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
