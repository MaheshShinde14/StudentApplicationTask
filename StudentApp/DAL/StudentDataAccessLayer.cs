using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;

namespace StudentApp.DAL
{
    public class StudentDataAccessLayer : IStudentDataAccessLayer
    {
        private DbContextClass _db = null;

        public StudentDataAccessLayer(DbContextClass db)
        {
            _db = db;
        }

        public async Task CreateStudent(Student student)
        {
            try
            {
                _db.Students.Add(student);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Student");
            }
        }

        public async Task DeleteStudent(int id)
        {
            Student student = _db.Students.Find(id);
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            students = await _db.Students.Include(s => s.Class).ToListAsync();
            return students;
        }

        public async Task<Student> GetStudentById(int id)
        {
            Student student = await _db.Students.FindAsync(id);
            return student;
        }

        public async Task UpdateStudent(Student student)
        {
            var existingEntity = _db.Students.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (existingEntity != null)
            {
                _db.Entry(existingEntity).State = EntityState.Detached;
            }
            _db.Entry(student).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}