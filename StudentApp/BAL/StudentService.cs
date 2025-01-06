using OfficeOpenXml;
using StudentApp.DAL;
using StudentApp.Models;
using StudentApp.Models.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace StudentApp.BAL
{
    public class StudentService : IStudentServiceInterface
    {
        private IStudentDataAccessLayer _studentDataAccessLayer;

        public StudentService(IStudentDataAccessLayer studentDataAccessLayer)
        {
            _studentDataAccessLayer = studentDataAccessLayer;
        }

        public async Task CreateStudent(Student student)
        {
            await _studentDataAccessLayer.CreateStudent(student);
        }

        public async Task DeleteStudent(int id)
        {
            await _studentDataAccessLayer.DeleteStudent(id);
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _studentDataAccessLayer.GetAllStudents();
        }

        public async Task<Student> GetStudentById(int id)
        {
            return await _studentDataAccessLayer.GetStudentById(id);
        }

        public async Task UpdateStudent(Student student)
        {
            await _studentDataAccessLayer.UpdateStudent(student);
        }
        public async Task<List<Student>> ReadStudentsFromExcel(string filePath)
        {
            var students = new List<Student>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var student = new Student
                    {
                        FirstName = worksheet.Cells[row, 1].Text,
                        LastName = worksheet.Cells[row, 2].Text,
                        Age = Convert.ToInt32(worksheet.Cells[row, 3].Text),
                        Gender = worksheet.Cells[row, 4].Text,
                        StudentId = 0,
                        ClassId = Convert.ToInt32(worksheet.Cells[row, 5].Text),
                    };
                    students.Add(student);
                }
            }
            return await Task.FromResult(students);
        }
        public async Task<ResultMessage> InsertStudentAfterValidation(List<Student> students)
        {
            string successMessage = string.Empty;
            Regex nameRegex = new Regex(@"^[a-zA-Z\s]+$");
           
            var filteredStudents = students
                .Where(s => nameRegex.IsMatch(s.FirstName)) 
                .GroupBy(s => new { s.FirstName }).ToList() 
                .Where(g =>
                    g.Count() == 1 || 
                    g.Count() == 2 && g.Select(s => s.Gender).Distinct().Count() > 1 
                )
                .SelectMany(g => g) 
                .ToList();
            var studentsDbData = await _studentDataAccessLayer.GetAllStudents();
            filteredStudents = filteredStudents
                                     .Where(f => !studentsDbData.ToList().Any(db => db.FirstName == f.FirstName && db.Gender == f.Gender))
                                     .ToList();

            var result = await _studentDataAccessLayer.BulkStudentInsert(filteredStudents);
            if(result == true)
            {
                filteredStudents.ForEach(x => { successMessage += $"{x.FirstName}'s data saved successfully\n"; });
                return new ResultMessage
                {
                    Message = successMessage,
                    Count = filteredStudents.Count
                };
            }
            else
            {
                return new ResultMessage
                {
                    Message = "No records inserted",
                    Count = 0
                };
            }
        }
    }
}