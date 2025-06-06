using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riva_Backend_Developer.Models;

namespace Riva_Backend_Developer.Services
{
    public interface ISyncValidator
    {
        ValidationResult ValidateSyncJob(SyncJob job);
    }
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public static ValidationResult Success => new ValidationResult { IsValid = true };
        public static ValidationResult Failure(string errorMessage) => new ValidationResult { IsValid = false, ErrorMessage = errorMessage };
    }
}