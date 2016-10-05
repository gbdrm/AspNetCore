using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AspNetCore.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual ICollection<TestPackage> TestPackages { get; set; }

        public virtual ICollection<TestResult> Testresults { get; set; }
    }
}
