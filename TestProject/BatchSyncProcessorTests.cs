using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Riva_Backend_Developer.Models;
using Riva_Backend_Developer.Services;

namespace TestProject
{
    public class BatchSyncProcessorTests
    {
        private BatchSyncProcessor _processor;
        private TestSyncValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new TestSyncValidator();
            _processor = new BatchSyncProcessor(_validator);
        }

        [Test]
        public async Task ProcessBatchAsync_EmptyJobs_CompletesSuccessfully()
        {
            // Arrange
            var jobs = new List<SyncJob>();

            // Act & Assert
            await _processor.ProcessAll(jobs);
        }

        [Test]
        public async Task ProcessBatchAsync_NullJobs_CompletesSuccessfully()
        {
            // Act & Assert
            await _processor.ProcessAll(null);
        }

        [Test]
        public async Task ProcessBatchAsync_ValidJob_ProcessesSuccessfully()
        {
            // Arrange
            var job = CreateValidSyncJob();
            var jobs = new List<SyncJob> { job };

            // Act
            await _processor.ProcessAll(jobs);

            // Assert
            Assert.That(job.Status, Is.EqualTo(SyncJobStatus.Completed));
            Assert.That(job.ProcessedAt, Is.Not.Null);
            Assert.That(job.ErrorMessage, Is.Null);
        }

        [Test]
        public async Task ProcessBatchAsync_InvalidJob_MarksAsFailed()
        {
            // Arrange
            var job = CreateInvalidSyncJob();
            var jobs = new List<SyncJob> { job };
            _validator.ShouldValidationSucceed = false;

            // Act
            await _processor.ProcessAll(jobs);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(job.Status, Is.EqualTo(SyncJobStatus.Failed));
                Assert.That(job.ErrorMessage, Is.Not.Null);
                Assert.That(job.ErrorMessage, Does.Contain("Validation failed"));
            });
        }

        [Test]
        public async Task ProcessBatchAsync_MultipleBatchJobs_ProcessesAllJobs()
        {
            // Arrange
            var jobs = new List<SyncJob>
            {
                CreateValidSyncJob(),
                CreateValidSyncJob(),
                CreateValidSyncJob()
            };

            // Act
            await _processor.ProcessAll(jobs);

            // Assert
            Assert.That(jobs.TrueForAll(j => j.Status == SyncJobStatus.Completed), Is.True);
            Assert.That(jobs.TrueForAll(j => j.ProcessedAt.HasValue), Is.True);
        }

        [Test]
        public async Task ProcessBatchAsync_MixedValidationResults_HandlesBothSuccessAndFailure()
        {
            // Arrange
            var validJob = CreateValidSyncJob();
            var invalidJob = CreateValidSyncJob();
            var jobs = new List<SyncJob> { validJob, invalidJob };

            _validator.ValidationResults = new Dictionary<Guid, bool>
            {
                { validJob.JobId, true },
                { invalidJob.JobId, false }
            };

            // Act
            await _processor.ProcessAll(jobs);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(validJob.Status, Is.EqualTo(SyncJobStatus.Completed));
                Assert.That(invalidJob.Status, Is.EqualTo(SyncJobStatus.Failed));
                Assert.That(invalidJob.ErrorMessage, Does.Contain("Validation failed"));
            });
        }

        private static SyncJob CreateValidSyncJob()
        {
            return new SyncJob
            {
                JobId = Guid.NewGuid(),
                User = new CrmUser
                {
                    UserId = Guid.NewGuid(),
                    Email = "test@example.com",
                    Platform = "TestPlatform",
                    Token = "valid_token"
                },
                ObjectType = "Contact",
                Payload = "test_payload",
                Status = SyncJobStatus.Pending
            };
        }

        private static SyncJob CreateInvalidSyncJob()
        {
            return new SyncJob
            {
                JobId = Guid.NewGuid(),
                User = new CrmUser
                {
                    UserId = Guid.NewGuid(),
                    Email = "test@example.com",
                    Platform = "TestPlatform",
                    Token = "" // Invalid - empty token
                },
                ObjectType = "Contact",
                Payload = "test_payload",
                Status = SyncJobStatus.Pending
            };
        }

        // Test implementation of ISyncValidator
        private class TestSyncValidator : ISyncValidator
        {
            public bool ShouldValidationSucceed { get; set; } = true;
            public Dictionary<Guid, bool> ValidationResults { get; set; } = new();

            public ValidationResult ValidateSyncJob(SyncJob job)
            {
                if (ValidationResults.ContainsKey(job.JobId))
                {
                    return ValidationResults[job.JobId]
                        ? ValidationResult.Success
                        : ValidationResult.Failure($"Validation failed for job {job.JobId}");
                }

                return ShouldValidationSucceed
                    ? ValidationResult.Success
                    : ValidationResult.Failure($"Validation failed for job {job.JobId}");
            }
        }
    }
}