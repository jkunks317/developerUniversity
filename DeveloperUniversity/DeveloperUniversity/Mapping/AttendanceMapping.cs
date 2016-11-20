using System.Data.Entity.ModelConfiguration;
using DeveloperUniversity.Models;

namespace DeveloperUniversity.Mapping
{
    public class AbsenceMapping : EntityTypeConfiguration<Absence>
    {
        public AbsenceMapping()
        {
            HasKey(p => p.Id);

            Property(p => p.AbsenceDate).IsRequired();
            Property(p => p.StudentFirstName).IsRequired();
            Property(p => p.StudentLastName).IsRequired();
            Property(p => p.CourseTitle).IsRequired();

            Property(p => p.StudentId).IsRequired();
            Property(p => p.CourseId).IsRequired();
        }
    }
}