using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.DAL;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class EmpController : Controller
    {
        private IEmpRepository empRepository;

        public EmpController()
        {
            this.empRepository = new EmpRepository(new SchoolContext());
        }

        public EmpController(IEmpRepository empRepository)
        {
            this.empRepository = empRepository;
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

            var emps = from e in empRepository.GetEmps()
                           select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                emps = emps.Where(e => e.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || e.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    emps = emps.OrderByDescending(e => e.LastName);
                    break;
                case "Date":
                    emps = emps.OrderBy(e => e.EntryDate);
                    break;
                case "date_desc":
                    //emps = emps.OrderByDescending(e => e.EntryDate);
                    break;
                default:  // Name ascending 
                    emps = emps.OrderBy(e => e.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(emps.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Emp/Details/5

        public ViewResult Details(int id)
        {
            Emp emp = empRepository.GetEmpByID(id);
            return View(emp);
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
        public ActionResult Create([Bind(Include = "LastName, FirstMidName, EntryDate")]Emp emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    empRepository.InsertEmp(emp);
                    empRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(emp);
        }

        //
        // GET: /Student/Edit/5

        public ActionResult Edit(int id)
        {
            Emp emp = empRepository.GetEmpByID(id);
            return View(emp);
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID, LastName, FirstMidName")]Emp emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    empRepository.UpdateEmp(emp);
                    empRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(emp);
        }

        //
        // GET: /Student/Delete/5

        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Emp emp = empRepository.GetEmpByID(id);
            return View(emp);
        }

        //
        // POST: /Student/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Emp emp = empRepository.GetEmpByID(id);
                empRepository.DeleteEmp(id);
                empRepository.Save();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            empRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
