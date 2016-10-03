using System;

namespace AspNetCore.Models
{
    public class TestResult
    {
        public Guid TestResultId { get; set; }
        public int Score { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Guid TestPackageId { get; set; }
        public virtual TestPackage TestPackage { get; set; }
    }
}