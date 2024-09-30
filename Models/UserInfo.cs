using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class UserInfo
    {
        public int userId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        [DisplayName("First Name")]
        public string firstName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        [DisplayName("Last Name")]
        public string lastName { get; set; }


        [StringLength(250)]
        public string brief { get; set; }

        [StringLength(200)]
        public string photo { get; set; }
    }
}