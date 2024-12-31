using StudentApp.BAL;
using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentApp.Controllers
{
    public class StudentUIController : Controller
    {
        private IStudentServiceInterface _studentServiceInterface;
        private IClassServiceInterface _classInterface;
        public StudentUIController(IStudentServiceInterface studentServiceInterface, IClassServiceInterface classInterface)
        {
            _studentServiceInterface = studentServiceInterface;
            _classInterface = classInterface;
        }

        public async Task<ActionResult> Index()
        {
            var students = await _studentServiceInterface.GetAllStudents();
            return View(students);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _studentServiceInterface.GetStudentById((int)id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public async Task<ActionResult> Create()
        {
            
            ViewBag.ClassList = await GetClassData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentServiceInterface.CreateStudent(student);
                return RedirectToAction("Index");
            }
            ViewBag.ClassList = await GetClassData();
            ViewBag.ClassId = new SelectList(await _classInterface.GetAllClasses(), "ClassId", "ClassName", student.ClassId);
            return View(student);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _studentServiceInterface.GetStudentById((int)id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassList = await GetClassData();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentServiceInterface.UpdateStudent(student);
                return RedirectToAction("Index");
            }
            ViewBag.ClassList = await GetClassData();
            ViewBag.ClassId = new SelectList(await _classInterface.GetAllClasses(), "ClassId", "ClassName", student.ClassId);
            return View(student);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _studentServiceInterface.GetStudentById((int)id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _studentServiceInterface.DeleteStudent(id);
            return RedirectToAction("Index");
        }
        private  async Task<List<SelectListItem>> GetClassData()
        {
            var ClassDataList = await _classInterface.GetAllClasses();
            var classList = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "", Text = "Select Class" }, // Default invalid option
                        };
            foreach (var item in ClassDataList)
            {
                classList.Add(new SelectListItem { Value = item.ClassId.ToString(), Text = item.ClassName });
            }
            return classList;
        }
    }
}