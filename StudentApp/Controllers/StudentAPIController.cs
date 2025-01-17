﻿using OfficeOpenXml;
using StudentApp.BAL;
using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
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
        [HttpPost]
        [Route("api/StudentAPI/Import")]
        public async Task<IHttpActionResult> Import()
        {
            string successMessage = String.Empty;
            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count == 0)
                    return BadRequest("No file uploaded.");

                var postedFile = httpRequest.Files[0];
                if (postedFile == null || postedFile.ContentLength == 0)
                    return BadRequest("File is empty.");

                var filePath = HttpContext.Current.Server.MapPath("~/Uploads/" + postedFile.FileName);
                postedFile.SaveAs(filePath);
                var students = await _studentServiceInterface.ReadStudentsFromExcel(filePath);
                var outputMessage = await _studentServiceInterface.InsertStudentAfterValidation(students);
                
                return Ok(outputMessage);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
