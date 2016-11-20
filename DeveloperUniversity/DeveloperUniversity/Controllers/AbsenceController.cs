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
            return View();
        }

        // POST: Absence/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AbsenceIndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var absence = new Absence();

                absence.StudentLastName = viewModel.StudentLastName;
                absence.StudentFirstName = viewModel.StudentFirstName;
                absence.AbsenceDate = viewModel.AbsenceDate;

                //.Replace(" ", "")  and .ToLower() are used to try and get both strings to a similar format
                //this helps when trying to compare 2 strings, and input/comparison variations are likely
                var student = db.Students.Where(s => s.FirstName.Replace(" ", "").ToLower() == viewModel.StudentFirstName.Replace(" ", "").ToLower() && 
                                                     s.LastName.Replace(" ", "").ToLower() == viewModel.StudentLastName.Replace(" ", "").ToLower()).FirstOrDefault();

                var course = db.Courses.Where(c => c.Title.Replace(" ", "").ToLower() == viewModel.CourseTitle.Replace(" ", "").ToLower()).FirstOrDefault();
                
                //Get the student and course record for entered data
                //TODO: Update Later.
                if (student == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Student not found.");
                }
                if (course == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Course not found.");
                }

                //Make sure that student is enrolled in that specific course
                var enrollment = db.Enrollments.Where(e => e.StudentId == student.Id && e.CourseId == course.Id).FirstOrDefault();

                if (enrollment == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Student is not enrolled in that course.");
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
    }
}
