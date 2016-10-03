using System;

namespace AspNetCore.Models
{
    public class TestOption
    {
        public Guid TestOptionId { get; set; }
        public string Content { get; set; }

        public Guid TestItemId { get; set; }
        public virtual TestItem TestItem { get; set; }
    }
}
