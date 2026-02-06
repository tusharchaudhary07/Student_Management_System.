using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyLoginPage.Models
{
    public class UserClass
    {
        [Required]
       [Display(Name="User ID")]
        public string USERID { get; set; }
        [Required]
        public string PASSWORD { get; set; }
      
        public string ROLE { get; set; }
    }
}