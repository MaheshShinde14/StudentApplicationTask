using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class DbContextClass : DbContext
    {
        public DbContextClass() : base("ConnectToDB")
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
    }
}