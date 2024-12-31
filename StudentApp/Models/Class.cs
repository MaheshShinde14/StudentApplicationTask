using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class Class
    {
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        [StringLength(20, ErrorMessage = "Name should not be more than 20 characters")]
        public string ClassName { get; set; }
        [JsonIgnore]
        public virtual ICollection<Student> Students { get; set; }
    }
}