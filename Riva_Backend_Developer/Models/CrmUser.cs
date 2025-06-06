using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riva_Backend_Developer.Models
{
    public class CrmUser
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? Platform { get; set; }
        public string? Token { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastSyncAt { get; set; }
        public override string ToString()
        {
            return $"{Email} ({Platform})";
        }
    }
}
