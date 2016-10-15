using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime Time { get; set; }

        public Guid TestPackageId { get; set; }
        public TestPackage TestPackage { get; set; }

        public string Text { get; set; }
    }
}
