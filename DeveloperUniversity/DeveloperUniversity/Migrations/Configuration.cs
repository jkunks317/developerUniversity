using System;
using System.Collections.Generic;
using DeveloperUniversity.Models;

namespace DeveloperUniversity.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //#if DEBUG
                    var student1 = new Student
                    {
                        FirstName = "Tyler",
                        LastName = "Blake",
                        Address = "1 Cardinal Way",
                        City = "Louisville",
                        State = "Kentucky",
                        ZipCode = 40222,
                        Email = "student1@email.com",
                        EnrollmentDate = new DateTime(2012, 1, 1),
                        Enrollments = new List<Enrollment>()
                    };

                    var student2 = new Student()
                    {
                        FirstName = "Jane",
                        LastName = "Doe",
                        Address = "101 Cardinal Way",
                        City = "Louisville",
                        State = "Kentucky",
                        ZipCode = 40222,
                        Email = "student2@email.com",
                        EnrollmentDate = new DateTime(2013, 8, 1),
                        Enrollments = new List<Enrollment>()

                    };

                    var student3 = new Student()
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Address = "201 Cardinal Way",
                        City = "Louisville",
                        State = "Kentucky",
                        ZipCode = 40222,
                        Email = "student3@email.com",
                        EnrollmentDate = new DateTime(2013, 5, 1),
                        Enrollments = new List<Enrollment>()
                    };

                    var enrollment1 = new Enrollment()
                    {
                        Grade = Grade.B
                    };
                    var enrollment2 = new Enrollment()
                    {
                        Grade = Grade.A
                    };
                    var enrollment3 = new Enrollment()
                    {
                        Grade = Grade.C
                    };

                    var course1 = new Course()
                    {
                        Id = 1,
                        Credits = 3,
                        Enrollments = new List<Enrollment>(),
                        Title = "CIS 411"
                    };
                    var course2 = new Course()
                    {
                        Id = 2,
                        Credits = 3,
                        Enrollments = new List<Enrollment>(),
                        Title = "CIS 420"
                    };
                    var course3 = new Course()
                    {
                        Id = 3,
                        Credits = 3,
                        Enrollments = new List<Enrollment>(),
                        Title = "CIS 450"
                    };

                    if (context.Students == null)
                    {
                        context.Students.AddOrUpdate(student1);
                        context.Students.AddOrUpdate(student2);
                        context.Students.AddOrUpdate(student3);
                    }

            


                    if (context.Enrollments == null)
                    {
                        context.Enrollments.AddOrUpdate(enrollment1);
                        context.Enrollments.AddOrUpdate(enrollment2);
                        context.Enrollments.AddOrUpdate(enrollment3);

                        student1.Enrollments.Add(enrollment1);
                        student2.Enrollments.Add(enrollment2);
                        student3.Enrollments.Add(enrollment3);
                    }

                    if (context.Courses == null)
                    {
                        context.Courses.AddOrUpdate(course1);
                        context.Courses.AddOrUpdate(course2);
                        context.Courses.AddOrUpdate(course3);

                        course1.Enrollments.Add(enrollment1);
                        course2.Enrollments.Add(enrollment2);
                        course3.Enrollments.Add(enrollment3);
                    }

                context.SaveChanges();
            //#endif
        }
    }
}
