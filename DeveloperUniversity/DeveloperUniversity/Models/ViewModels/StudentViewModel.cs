using System;
using System.ComponentModel.DataAnnotations;

namespace DeveloperUniversity.Models.ViewModels
{
    public class StudentIndexViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
    }
}