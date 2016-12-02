using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeveloperUniversity.Models.ViewModels
{
    public class AbsenceIndexViewModel
    {
        [Display(Name = "Absence Date")]
        public DateTime AbsenceDate { get; set; }

        [Display(Name = "Student First Name")]
        [Required(ErrorMessage = "Student first name is required.")]
        public string StudentFirstName { get; set; }

        [Display(Name = "Student Last Name")]
        [Required(ErrorMessage = "Student last name is required.")]
        public string StudentLastName { get; set; }

        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }
        
        public List<string> CourseTitles { get; set; } 
    }
}