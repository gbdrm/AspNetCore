using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Models
{
    public class TestPackage
    {
        public Guid TestPackageId { get; set; }
        [Display(Name = "Название теста")]
        [MaxLength(40)]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        [MaxLength(100)]
        public string Description { get; set; }
        [Display(Name = "Создан")]
        public DateTime TimeCreated { get; set; }
        public int Viewed { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Вопросы")]
        public virtual ICollection<TestItem> TestItems { get; set; }
    }
}
