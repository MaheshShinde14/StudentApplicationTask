﻿using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StudentApp.BAL
{
    public interface IStudentServiceInterface
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> GetStudentById(int id);
        Task CreateStudent(Student student);
        Task UpdateStudent(Student student);
        Task DeleteStudent(int id);
        //Student validateStudent(string FirstName, string Gender);
        Task<List<Student>> ReadStudentsFromExcel(string filePath);
        string validateNameandGender(Student student);
        Task<object> InsertStudentAfterValidation(List<Student> students);
    }
}