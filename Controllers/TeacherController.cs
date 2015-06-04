using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.DAL;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class TeacherController : Controller
    {
        private ITeacherRepository teacherRepository;

        public TeacherController()
        {
            this.teacherRepository = new TeacherRepository(new SchoolContext());
        }

        public TeacherController(ITeacherRepository teacherRepository)
        {
            this.teacherRepository = teacherRepository;
        }

        //
        // GET: /Emp/

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var teachers = from t in teacherRepository.GetTeachers()
                       select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(e => e.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || e.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    teachers = teachers.OrderByDescending(e => e.LastName);
                    break;
                case "Date":
                    teachers = teachers.OrderBy(e => e.LectureDate);
                    break;
                case "date_desc":
                    //emps = emps.OrderByDescending(e => e.EntryDate);
                    break;
                default:  // Name ascending 
                    teachers = teachers.OrderBy(e => e.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
             
           // return View();
            return View(teachers.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Emp/Details/5
        
        public ViewResult Details(int id)
        {
            Teacher teacher = teacherRepository.GetTeacherByID(id);
            return View(teacher);
        }
        
        //
        // GET: /Emp/Create
        
        public ActionResult Create()
        {
            return View();
        }
        
        //
        // POST: /Student/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName, FirstMidName, LectureDate")]Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teacherRepository.InsertTeacher(teacher);
                    teacherRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException )
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(teacher);
        }
        
        //
        // GET: /Student/Edit/5
        
        public ActionResult Edit(int id)
        {
            Teacher teacher = teacherRepository.GetTeacherByID(id);
            return View(teacher);
        }
        
        //
        // POST: /Student/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID, LastName, FirstMidName")]Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teacherRepository.UpdateTeacher(teacher);
                    teacherRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException )
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(teacher);
        }
        
        //
        // GET: /Student/Delete/5
        
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Teacher teacher = teacherRepository.GetTeacherByID(id);
            return View(teacher);
        }
        
        //
        // POST: /Student/Delete/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Teacher teacher = teacherRepository.GetTeacherByID(id);
                teacherRepository.DeleteTeacher(id);
                teacherRepository.Save();
            }
            catch (DataException )
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            teacherRepository.Dispose();
            base.Dispose(disposing);
        }
         
    }
}
