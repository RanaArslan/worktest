using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Teacher : Person
    {
        [DataType(DataType.Date)]
        [Display(Name = "Lecture Date")]
        public DateTime LectureDate { get; set; }

        
    }
}
