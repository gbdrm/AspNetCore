using System;
using System.Collections.Generic;

namespace AspNetCore.Models
{
    public class TestPackage
    {
        public Guid TestPackageId { get; set; }
        public string Description { get; set; }
        public DateTime TimeCreated { get; set; }
        public int Viewed { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<TestItem> TestItems { get; set; }
    }
}
