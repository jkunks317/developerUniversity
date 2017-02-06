using System.Data.Entity.ModelConfiguration;
using DeveloperUniversity.Models;

namespace DeveloperUniversity.Mapping
{
    public class StudentMapping : EntityTypeConfiguration<Student>
    {
        public StudentMapping()
        {
            HasKey(p => p.Id);

            Property(p => p.FirstName).IsRequired();
            Property(p => p.LastName).IsRequired();
            Property(p => p.EnrollmentDate).IsRequired();
            Property(p => p.Email).IsRequired();

            HasMany(p => p.Enrollments);
        }
    }
}