using System;
using System.Collections.Generic;

namespace AspNetCore.Models
{
    public class TestItem
    {
        public Guid TestItemId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public TestType TestType { get; set; }

        public Guid TestPackageId { get; set; }
        public virtual TestPackage TestPackage { get; set; }

        public virtual ICollection<TestOption> TestOptions { get; set; }
    }

    public enum TestType
    {
        TextAnswer,
        OneOption, // radio box
        FewOptions, // check boxes
    }
}