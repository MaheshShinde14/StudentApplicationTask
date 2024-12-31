using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StudentApp.DAL
{
    public interface IClassDataAccessLayer
    {
        Task<List<string>> GetAllClasseNames();
        Task<List<Class>> GetAllClasses();
        Task CreateClass(Class classData);
        Task<Class> GetClassById(int classId);
        Task UpdateClass(Class classData);
        Task DeleteClass(int id);
    }
}