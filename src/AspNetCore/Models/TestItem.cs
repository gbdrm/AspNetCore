using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Models
{
    public class TestItem
    {
        public Guid TestItemId { get; set; }
        [Display(Name = "Вопрос")]
        public string Question { get; set; }
        [Display(Name = "Ответ")]
        public string Answer { get; set; }
        [Display(Name = "Тип ответа")]
        public TestType TestType { get; set; }

        [Display(Name = "Имя теста")]
        public Guid TestPackageId { get; set; }
        public virtual TestPackage TestPackage { get; set; }

        public virtual ICollection<TestOption> TestOptions { get; set; }
    }

    public enum TestType
    {
        Text,
        OneOption, // radio box
        FewOptions, // check boxes
    }
}