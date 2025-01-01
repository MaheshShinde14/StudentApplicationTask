using StudentApp.BAL;
using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;

namespace StudentApp.Controllers
{
    public class StudentAPIController : ApiController
    {
        private readonly IStudentServiceInterface _studentServiceInterface;
        private readonly IClassServiceInterface _classInterface;

        public StudentAPIController(IStudentServiceInterface studentServiceInterface, IClassServiceInterface classInterface)
        {
            _studentServiceInterface = studentServiceInterface;
            _classInterface = classInterface;
        }
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var students = await _studentServiceInterface.GetAllStudents();
            if (students == null)
            {
                return NotFound();
            }
            return Json(students);
        }
        [HttpGet]
        [Route("api/GetStudentByID")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var student = await _studentServiceInterface.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest("Invalid data.");
            }

            await _studentServiceInterface.CreateStudent(student);
            return Ok(HttpStatusCode.OK);
        }

        
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Student student)
        {
            if (student == null || id != student.StudentId)
            {
                return BadRequest("Invalid data.");
            }

            var existingStudent = await _studentServiceInterface.GetStudentById(id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            await _studentServiceInterface.UpdateStudent(student);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var student = await _studentServiceInterface.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }

            await _studentServiceInterface.DeleteStudent(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
