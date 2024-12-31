using StudentApp.BAL;
using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentApp.Controllers
{
    public class ClassUIController : Controller
    {
        private IClassServiceInterface _classServiceInterface;

        public ClassUIController(IClassServiceInterface classInterface)
        {
            _classServiceInterface = classInterface;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Class classData)
        {
            if (ModelState.IsValid)
            {
                await _classServiceInterface.CreateClass(classData);
                return RedirectToAction("Create");
            }

            return View(classData);
        }
        public async Task<JsonResult> GetClassList()
        {
            var classList = await _classServiceInterface.GetAllClasseNames();
            return Json(classList, JsonRequestBehavior.AllowGet);
        }
    }
}