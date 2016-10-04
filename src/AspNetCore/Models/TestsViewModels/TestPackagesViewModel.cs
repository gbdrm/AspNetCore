using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Models.TestsViewModels
{
    public class TestPackagesViewModel
    {
        public TestPackage TestPackage { get; set; }

        public IEnumerable<TestItem> TestItems { get; set; }
    }
}
