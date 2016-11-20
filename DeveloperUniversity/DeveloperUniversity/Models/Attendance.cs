using System;

namespace DeveloperUniversity.Models
{
    public class Absence
    {
        public int Id { get; set; }
        public DateTime AbsenceDate { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string CourseTitle { get; set; }

        public int CourseId { get; set; }
        public int StudentId { get; set; }

    }
}