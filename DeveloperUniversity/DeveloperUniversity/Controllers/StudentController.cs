using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using DeveloperUniversity.Helpers;
using DeveloperUniversity.Models;
using DeveloperUniversity.Models.ViewModels;

namespace DeveloperUniversity.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var students = db.Students.Select(s => new StudentIndexViewModel()
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                EnrollmentDate = s.EnrollmentDate
            });

            return View(students);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var student = new Student()
                {
                    Address = vm.Address,
                    City = vm.City,
                    EnrollmentDate = vm.EnrollmentDate,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    ZipCode = vm.ZipCode,
                    State = vm.State
                };
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentIndexViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var student = db.Students.FirstOrDefault(s => s.Id == vm.Id);

                if (student != null)
                {
                    student.FirstName = vm.FirstName;
                    student.LastName = vm.LastName;
                    student.EnrollmentDate = vm.EnrollmentDate;
                }

                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public void EmailAllStudents(string subject, string message)
        {
            //Get all currently active students + any extra search criteria we might want.
            var students = db.Students;

            foreach (var student in students)
            {
                //Send Email..


                var IsDebug = false;

#if DEBUG
                IsDebug = true;
#endif

                var toEmail = "tylersmtptest@gmail.com";
                var toEmailPassword = "smtpwelcome1";

                //Debug SMTP Settings
                var debugSmtpClient = Constants.DebugSmtpClient;
                //Setup credentials to login to our sender email address("UserName", "Password")
                NetworkCredential credentials = new NetworkCredential(toEmail, toEmailPassword);
                debugSmtpClient.Credentials = credentials;

                //Release SMTP Settings
                var releaseSmtpClient = Constants.ReleaseSmtpClient;


                if (ModelState.IsValid || (ModelState.IsValid == false && IsDebug))
                {
                    string Body = message;

                    MailMessage mail = new MailMessage();
                    mail.To.Add(toEmail);
                    mail.From = new MailAddress(student.Email);
                    mail.Subject = subject;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
#if DEBUG
                    smtp = debugSmtpClient;
#else
                smtp = releaseSmtpClient;
#endif
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (SmtpException)
                    {
                        RedirectToAction("Index");
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
