using StudentApp.DAL;
using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StudentApp.BAL
{
    public class ClassService : IClassServiceInterface
    {
        private IClassDataAccessLayer _classDataAccessLayer;
        public ClassService(IClassDataAccessLayer classDataAccessLayer)
        {
            _classDataAccessLayer = classDataAccessLayer;
        }

        public async Task CreateClass(Class classData)
        {
            await _classDataAccessLayer.CreateClass(classData);
        }

        public async Task<List<string>> GetAllClasseNames()
        {
           return await _classDataAccessLayer.GetAllClasseNames();
        }
        public async Task<List<Class>> GetAllClasses()
        {
            return await _classDataAccessLayer.GetAllClasses();
        }
        public async Task<Class> GetClassById(int ClassId)
        {
           return await _classDataAccessLayer.GetClassById(ClassId);
        }
        public async Task UpdateClass(Class ClassData)
        {
             await _classDataAccessLayer.UpdateClass(ClassData);
        }
        public async Task DeleteClass(int id)
        {
            await _classDataAccessLayer.DeleteClass(id);
        }
    }
}