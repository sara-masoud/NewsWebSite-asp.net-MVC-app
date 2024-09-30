using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class ChangePassword
    {
       [Required(ErrorMessage ="*")]
       [StringLength(10,ErrorMessage = "Password should be 6 digits at least.", MinimumLength =6)]
        public string Password { get; set; }
       
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and confirm password do not match. ")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}