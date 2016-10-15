using System;

namespace AspNetCore.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime Time { get; set; }

        public Guid TestPackageId { get; set; }
        public virtual TestPackage TestPackage { get; set; }

        public string Text { get; set; }
    }
}
