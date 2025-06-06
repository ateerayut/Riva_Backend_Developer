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
    public class TokenValidatorTests
    {
        private SimpleTokenValidator _tokenValidator;

        [SetUp]
        public void Setup()
        {
            _tokenValidator = new SimpleTokenValidator();
        }

        [Test]
        public void ValidateSyncJob_ValidJob_ReturnsSuccess()
        {
            // Arrange
            var user = new CrmUser
            {
                UserId = Guid.CreateVersion7(),
                Email = "sample@company.com",
                Platform = "Salesforce",
                Token = "valid_token"
            };

            var job = new SyncJob
            {
                JobId = Guid.CreateVersion7(),
                User = user,
                ObjectType = "Contact",
                Payload = "123",
                Status = SyncJobStatus.Pending
            };

            // Act
            var result = _tokenValidator.ValidateSyncJob(job);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.True);
                Assert.That(result.ErrorMessage, Is.Null);
            });
        }

        [Test]
        public void ValidateSyncJob_NullJob_ReturnsFailure()
        {
            // Act
            var result = _tokenValidator.ValidateSyncJob(null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("Sync job cannot be null."));
            });
        }

        [Test]
        public void ValidateSyncJob_NullUser_ReturnsFailure()
        {
            // Arrange
            var job = new SyncJob
            {
                JobId = Guid.CreateVersion7(),
                User = null,
                ObjectType = "Contact",
                Payload = "123",
                Status = SyncJobStatus.Pending
            };

            // Act
            var result = _tokenValidator.ValidateSyncJob(job);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("User information is required for sync job."));
            });
        }

        [Test]
        public void ValidateSyncJob_EmptyToken_ReturnsFailure()
        {
            // Arrange
            var user = new CrmUser
            {
                UserId = Guid.CreateVersion7(),
                Email = "sample@company.com",
                Platform = "Salesforce",
                Token = ""
            };

            var job = new SyncJob
            {
                JobId = Guid.CreateVersion7(),
                User = user,
                ObjectType = "Contact",
                Payload = "123",
                Status = SyncJobStatus.Pending
            };

            // Act
            var result = _tokenValidator.ValidateSyncJob(job);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo($"User token is required for sync job. {user.Email}"));
            });
        }

        [Test]
        public void ValidateSyncJob_EmptyObjectType_ReturnsFailure()
        {
            // Arrange
            var user = new CrmUser
            {
                UserId = Guid.CreateVersion7(),
                Email = "sample@company.com",
                Platform = "Salesforce",
                Token = "valid_token"
            };

            var job = new SyncJob
            {
                JobId = Guid.CreateVersion7(),
                User = user,
                ObjectType = "",
                Payload = "123",
                Status = SyncJobStatus.Pending
            };

            // Act
            var result = _tokenValidator.ValidateSyncJob(job);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("Object type is required for sync job."));
            });
        }

        [Test]
        public void ValidateSyncJob_EmptyPayload_ReturnsFailure()
        {
            // Arrange
            var user = new CrmUser
            {
                UserId = Guid.CreateVersion7(),
                Email = "sample@company.com",
                Platform = "Salesforce",
                Token = "valid_token"
            };

            var job = new SyncJob
            {
                JobId = Guid.CreateVersion7(),
                User = user,
                ObjectType = "Contact",
                Payload = "",
                Status = SyncJobStatus.Pending
            };

            // Act
            var result = _tokenValidator.ValidateSyncJob(job);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("Payload is required for sync job."));
            });
        }
    }
}