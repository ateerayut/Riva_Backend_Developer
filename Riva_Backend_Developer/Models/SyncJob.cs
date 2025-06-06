using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riva_Backend_Developer.Models;

namespace Riva_Backend_Developer.Models
{
    public class SyncJob
    {
        public Guid JobId { get; set; }
        public CrmUser? User { get; set; }
        public string? ObjectType { get; set; }
        public string? Payload { get; set; }
        public SyncJobStatus Status { get; set; } = SyncJobStatus.Pending;
        public string? ErrorMessage { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }

        public void MarkAsCompleted()
        {
            Status = SyncJobStatus.Completed;
            ProcessedAt = DateTime.UtcNow;
            ErrorMessage = null;
        }

        public void MarkAsFailed(string errorMessage)
        {
            Status = SyncJobStatus.Failed;
            ProcessedAt = DateTime.UtcNow;
            ErrorMessage = errorMessage;
        }

        public void MarkAsProcessing()
        {
            Status = SyncJobStatus.Processing;
            ProcessedAt = null;
            ErrorMessage = null;
        }

        public void MarkAsCancelled()
        {
            Status = SyncJobStatus.Cancelled;
            ProcessedAt = DateTime.UtcNow;
            ErrorMessage = null;
        }

        public void MarkAsPending()
        {
            Status = SyncJobStatus.Pending;
            ProcessedAt = null;
            ErrorMessage = null;
        }

        public override string ToString()
        {
            return $"JobId: {JobId} - {User.Email}, Status: {Status}";
        }
    }

    public enum SyncJobStatus
    {
        Pending,
        Processing,
        Completed,
        Failed,
        Cancelled
    }
}
