using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Name should not be empty")]
        [StringLength(20, ErrorMessage = "Name should not be more than 20 characters")]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [Range(3, 18, ErrorMessage = "Age should be in between 3 to 18")]
        public int Age { get; set; }

        [RegularExpression("male|female|Male|Female", ErrorMessage = "Gender should be Male or Female")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please select a class.")]
        public int ClassId { get; set; }
        [JsonIgnore]
        public virtual Class Class { get; set; }

    }
}