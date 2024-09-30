using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class LoginData
    {
        [Required(ErrorMessage ="*")]
        [RegularExpression(@"(^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$)",ErrorMessage ="Incorrect email.")]
        public string Email { get; set; }
        [Required(ErrorMessage ="*")]
        public string Password { get; set; }
    }
}