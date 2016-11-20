using System;
using System.ComponentModel.DataAnnotations;

namespace DeveloperUniversity.Models.ViewModels
{
    public class AbsenceViewModel
    {
        [Display(Name = "Absence Date")]
        public DateTime AbsenceDate { get; set; }
        [Display(Name = "Student First Name")]
        public string StudentFirstName { get; set; }
        [Display(Name = "Student Last Name")]
        public string StudentLastName { get; set; }
        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }

    }
}