using System;

namespace AspNetCore.Models
{
    public class Answer
    {
        public Guid AnswerId { get; set; }

        public Guid? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Guid TestItemId { get; set; }
        public virtual TestItem TestItem { get; set; }

        public string Value { get; set; }
        public bool IsCorrect { get; set; }
    }
}