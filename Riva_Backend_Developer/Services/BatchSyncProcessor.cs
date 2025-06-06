using Riva_Backend_Developer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Riva_Backend_Developer.Services
{
    public class BatchSyncProcessor
    {
        private readonly ISyncValidator _syncValidator;
        public BatchSyncProcessor(ISyncValidator syncValidator)
        {
            _syncValidator = syncValidator ?? throw new ArgumentNullException(nameof(syncValidator));
        }
        public async Task ProcessAll(IEnumerable<SyncJob> jobs)
        {
            if (jobs == null || !jobs.Any())
            {
                Console.WriteLine("No jobs to process.");
                return;
            }
            foreach (var job in jobs)
            {
                Console.WriteLine($"Processing job : {job}");
                //ProcessJobAsync(job).GetAwaiter().GetResult();
                await ProcessJobAsync(job);
                if (job.Status == SyncJobStatus.Completed)
                {
                    Console.WriteLine("Job Completed.");
                }
                else
                {
                    Console.WriteLine($"Job failed {job.ErrorMessage}");
                }
                Console.WriteLine();
            }
        }

        private async Task ProcessJobAsync(SyncJob job)
        {
            if (job == null)
            {
                Console.WriteLine("Job cannot be null.");
                return;
            }
            job.MarkAsProcessing();
            var validationResult = _syncValidator.ValidateSyncJob(job);
            if (!validationResult.IsValid)
            {
                job.MarkAsFailed(validationResult.ErrorMessage);
                Console.WriteLine($"Job {job.JobId} failed validation: {validationResult.ErrorMessage}");
                return;
            }
            try
            {
                // Simulate processing the job
                await Task.Delay(1000);
                job.MarkAsCompleted();
                Console.WriteLine($"Job {job.JobId} completed successfully.");
            }
            catch (Exception ex)
            {
                job.MarkAsFailed($"Error processing job: {ex.Message}");
                Console.WriteLine($"Job {job.JobId} failed with error: {ex.Message}");
            }
        }
    }
}
