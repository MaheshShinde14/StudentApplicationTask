using OfficeOpenXml;
using StudentApp.DAL;
using StudentApp.Models;
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
        public async Task<object> InsertStudentAfterValidation(List<Student> students)
        {
            string validationMessage = string.Empty;
            int successCount = 0;
            string successMessage = string.Empty;
            foreach (var item in students)
            {
                var validateStudent = await _studentDataAccessLayer.ValidateStudent(item.FirstName, item.Gender);

                if (validateStudent == null)
                {
                    #region validate name and gender
                    string isValid = validateNameandGender(item);
                    #endregion
                    if (string.IsNullOrEmpty(isValid))
                    {
                        await _studentDataAccessLayer.CreateStudent(item);
                        successCount++;
                        successMessage += item.FirstName+"Students saved successfully";
                    }
                    else
                    {
                        validationMessage += isValid + Environment.NewLine;
                    }
                }
                else
                {
                    validationMessage += validateStudent.FirstName + ":already exists in Database" + Environment.NewLine;
                }
            }
            if (successMessage == string.Empty)
            {
                successMessage = "all records in file already exists in database.";
            }
            var responseMessage = new
            {
                SuccessMessage = successMessage,
                ValidationMessage = validationMessage,
                SuccessfullyInsertedCount = successCount
            };
            return responseMessage;
        }
        public string validateNameandGender(Student student)
        {
            string error = string.Empty;
            var uniqueCombinations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var uniqueKey = $"{student.FirstName.ToLower()}_{student.Gender.ToLower()}";
            // Validation: No special characters in the Name
            if (!Regex.IsMatch(student.FirstName, @"^[a-zA-Z\s]+$"))
            {
                error = ($"Invalid Name: {student.FirstName}. Only alphabets and spaces are allowed.");
            }

            // Validation: Check for duplicate Name and Gender combinations

            if (uniqueCombinations.Contains(uniqueKey))
            {
                error = ($"Duplicate entry: Name={student.FirstName}, Gender={student.Gender}");
            }
            uniqueCombinations.Add(uniqueKey);

            return error;
        }
    }
}