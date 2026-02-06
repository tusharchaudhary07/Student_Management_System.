using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyLoginPage.Models
{
    public class StudentDetails
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name ="User ID")]
        public string USERID { get; set; }

        [Required]
        public string NAME { get; set; }

        [Required]
        public string BRANCH { get; set; }

        [Required]
        [Display(Name ="Mobile No.")]
        public string MOBILENO { get; set; }

        [Required]
        public string ADDRESS { get; set; }
    }
}