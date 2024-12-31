using StudentApp.BAL;
using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace StudentApp.Controllers
{
    public class ClassAPIController : ApiController
    {
        private readonly IClassServiceInterface _classServiceInterface;

        public ClassAPIController(IClassServiceInterface classServiceInterface)
        {
            _classServiceInterface = classServiceInterface;
        }
        [HttpPost]
        public async Task<IHttpActionResult> POST([FromBody] Class classData)
        {
            if (classData == null)
            {
                return BadRequest("Invalid data.");
            }

            await _classServiceInterface.CreateClass(classData);
            return Ok(HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetClassList()
        {
            var classList = await _classServiceInterface.GetAllClasseNames();
            if (classList == null || classList.Count == 0)
            {
                return NotFound();
            }

            return Json(classList);
        }
        [HttpGet]
        [Route("api/GetClassByID")]

        public async Task<IHttpActionResult> GetClassById(int classId)
        {
            var classData = await _classServiceInterface.GetClassById(classId);
            if (classData == null)
            {
                return NotFound();
            }

            return Json(classData);
        }
        [HttpPut]
        public async Task<IHttpActionResult> UpdateClass(int classId,Class classData)
        {
            if (classData == null || classId != classData.ClassId)
            {
                return BadRequest("Invalid data.");
            }

            var existingStudent = await _classServiceInterface.GetClassById(classId);
            if (existingStudent == null)
            {
                return NotFound();
            }

            await _classServiceInterface.UpdateClass(classData);
            return StatusCode(HttpStatusCode.OK);
        }
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteClass(int classId)
        {
            var result = await _classServiceInterface.GetClassById(classId);
            if (result == null)
            {
                return NotFound();
            }

            await _classServiceInterface.DeleteClass(classId);
            return StatusCode(HttpStatusCode.OK);
        }
    }
}
