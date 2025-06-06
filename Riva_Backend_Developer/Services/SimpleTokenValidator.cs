using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riva_Backend_Developer.Models;

namespace Riva_Backend_Developer.Services
{
    public class SimpleTokenValidator : ISyncValidator
    {
        public ValidationResult ValidateSyncJob(SyncJob job)
        {
            if (job == null)
            {
                return ValidationResult.Failure("Sync job cannot be null.");
            }
            if (job.User == null)
            {
                return ValidationResult.Failure("User information is required for sync job.");
            }
            if (string.IsNullOrWhiteSpace(job.User.Token))
            {
                return ValidationResult.Failure($"User token is required for sync job. {job.User.Email}");
            }
            if (string.IsNullOrWhiteSpace(job.ObjectType))
            {
                return ValidationResult.Failure("Object type is required for sync job.");
            }
            if (string.IsNullOrWhiteSpace(job.Payload))
            {
                return ValidationResult.Failure("Payload is required for sync job.");
            }
            // Additional validation logic can be added here
            return ValidationResult.Success;
        }
    }



}
