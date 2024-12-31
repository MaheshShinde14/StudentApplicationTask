using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StudentApp.DAL
{
    public class ClassDataAccessLayer : IClassDataAccessLayer
    {
        private DbContextClass _db = null;

        public ClassDataAccessLayer(DbContextClass db)
        {
            _db = new DbContextClass(); 
        }

        public async Task CreateClass(Class classData)
        {
             _db.Classes.Add(classData);
            await _db.SaveChangesAsync();
        }


        public async Task<List<string>> GetAllClasseNames()
        {
            return await _db.Classes.Select(c => c.ClassName).ToListAsync(); 
        }

        public async Task<List<Class>> GetAllClasses()
        {
            return await _db.Classes.ToListAsync();
        }
        public async Task<Class> GetClassById(int classId)
        {
            return await _db.Classes.FindAsync(classId);
        }
        public async Task UpdateClass(Class classdata)
        {
            var existingClass = await _db.Classes.FindAsync(classdata);
            if (existingClass!=null)
            {
                existingClass.ClassName = classdata.ClassName;
                _db.SaveChanges();

            }

        }
        public async Task DeleteClass(int classid)
        {
            var classData = await _db.Classes.FindAsync(classid);
            if (classData!=null)
            {
                _db.Classes.Remove(classData);
                await _db.SaveChangesAsync();
            }
        }
    }
}