using System.Linq;
using System.Net;
using System.Web.Mvc;
using DeveloperUniversity.Models;
using DeveloperUniversity.Models.ViewModels;

namespace DeveloperUniversity.Controllers
{
    [Authorize(Roles = "Admin, Volunteer")]
    public class AbsenceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Absence
        public ActionResult Index()
        {
            return View(db.Absences.ToList());
        }

        // GET: Absence/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Absence absence = db.Absences.Find(id);
            if (absence == null)
            {
                return HttpNotFound();
            }
            return View(absence);
        }

        // GET: Absence/Create
        public ActionResult Create()
        {
            //Note: On 2nd pass I updated this view to have a dropdown for course title instead of textbox,
            //      this helps remove the need for so much error checking/handling on course title textbox because now,
            //      the title will match what is in the database when they select it from the dropdown list.

            //      Old Code 
            //      return View();

            //      New Code
            var viewModel = new AbsenceIndexViewModel();
            // viewModel.CourseTitles = db.Courses.Select(c => c.Title).ToList();
            //viewModel.CourseTitles = db.Courses.Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            viewModel.CourseTitles = db.Courses.Select(c => c.Title).ToList();

            return View(viewModel);
        }

        // POST: Absence/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AbsenceIndexViewModel viewModel)
        {
            //TODO: Refreshing the whole list of Courses, only the selected 1 is posting back, maybe refactor later?          
            viewModel.CourseTitles = db.Courses.Select(c => c.Title).ToList();

            if (ModelState.IsValid)
            {
                var absence = new Absence();

                absence.StudentLastName = viewModel.StudentLastName;
                absence.StudentFirstName = viewModel.StudentFirstName;
                absence.AbsenceDate = viewModel.AbsenceDate;

                //Old Code

                //.Replace(" ", "")  and .ToLower() are used to try and get both strings to a similar format
                //this helps when trying to compare 2 strings, and input/comparison variations are likely

                //var student = db.Students.Where(s => s.FirstName.Replace(" ", "").ToLower() == viewModel.StudentFirstName.Replace(" ", "").ToLower() && 
                //                                     s.LastName.Replace(" ", "").ToLower() == viewModel.StudentLastName.Replace(" ", "").ToLower()).FirstOrDefault();

                //var course = db.Courses.Where(c => c.Title.Replace(" ", "").ToLower() == viewModel.CourseTitle.Replace(" ", "").ToLower()).FirstOrDefault();

                //New Code
                var student = db.Students.Where(s => s.FirstName.Replace(" ", "").ToLower() == viewModel.StudentFirstName.Replace(" ", "").ToLower() &&
                                                     s.LastName.Replace(" ", "").ToLower() == viewModel.StudentLastName.Replace(" ", "").ToLower()).FirstOrDefault();

                var course = db.Courses.Where(c => c.Title == viewModel.CourseTitle).FirstOrDefault();


                //Get the student and course record for entered data
                //TODO: Update Later.
                if (student == null)
                {
                    return View("Create", viewModel);
                    //Old Code
                   // return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Student was not found.");
                }

                if (course == null)
                {
                    return View("Create", viewModel);

                    //Old Code
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Course was not found.");
                }

                //Make sure that student is enrolled in that specific course
                var enrollment = db.Enrollments.Where(e => e.StudentId == student.Id && e.CourseId == course.Id).FirstOrDefault();

                if (enrollment == null)
                {
                    return View("Create", viewModel);

                    //Old Code
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Student is not enrolled in that course.");
                }

                absence.CourseId = course.Id;
                absence.StudentId = student.Id;
                absence.CourseTitle = course.Title; //Use the db instance of the title (in case the user entered in a mangled string :))


                db.Absences.Add(absence);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           return View("Create", viewModel);
        }

        // GET: Absence/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Absence absence = db.Absences.Find(id);
            if (absence == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AbsenceViewModel();

            viewModel.AbsenceDate = absence.AbsenceDate.Date;
            viewModel.CourseTitle = absence.CourseTitle;
            viewModel.StudentFirstName = absence.StudentFirstName;
            viewModel.StudentLastName = absence.StudentLastName;
            viewModel.Id = absence.Id;
            viewModel.CourseId = absence.CourseId;
            viewModel.StudentId = absence.StudentId;
           
            return View(viewModel);
        }

        // POST: Absence/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AbsenceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var absence = db.Absences.FirstOrDefault(a => a.Id == viewModel.Id);

                absence.AbsenceDate = viewModel.AbsenceDate;
                absence.CourseTitle = viewModel.CourseTitle;
                absence.StudentFirstName = viewModel.StudentFirstName;
                absence.StudentLastName = viewModel.StudentLastName;
                absence.CourseId = viewModel.CourseId;
                absence.StudentId = viewModel.StudentId;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Absence/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Absence absence = db.Absences.Find(id);
            if (absence == null)
            {
                return HttpNotFound();
            }
            return View(absence);
        }

        // POST: Absence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Absence absence = db.Absences.Find(id);
            db.Absences.Remove(absence);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public static readonly string ResponseMessageKey = "ResponseMessage";
        //public static readonly string ResponseMessageDebugKey = "ResponseMessageDebug";
        //public static readonly string ResponseMessageTypeKey = "ResponseMessageType";
        //public enum NotificationType { Alert, Success, Error }
        //protected void ToastNotification(string message, NotificationType type = NotificationType.Success)
        //{
        //    TempData[ResponseMessageKey] = message;
        //    TempData[ResponseMessageTypeKey] = type.ToString().ToLower();
        //}
    }
}
