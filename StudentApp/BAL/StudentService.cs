using StudentApp.DAL;
using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}