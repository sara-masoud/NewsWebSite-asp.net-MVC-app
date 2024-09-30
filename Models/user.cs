namespace NewsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class user:UserInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            News = new HashSet<News>();
        }

       // public int userId { get; set; }

        //[StringLength(50)]
        //[Required(ErrorMessage ="*")]
        //[DisplayName("First Name")]
        //public string firstName { get; set; }

        //[StringLength(50)]
        //[Required(ErrorMessage ="*")]
        //[DisplayName("Last Name")]
        //public string lastName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage ="*")]
        [RegularExpression(@"(^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$)", ErrorMessage = "Enter correct email format. e.g examle@gmail.com")]
        [Remote("checkEmail","Auth",ErrorMessage ="This email is already exist")]
        [DisplayName("Email")]
        public string email { get; set; }

        [StringLength(10, ErrorMessage = "Password should be 6 digits at least.", MinimumLength = 6)]
        [Required(ErrorMessage = "*") ]
        [DisplayName("Password")]
        public string pass_word { get; set; }
        [NotMapped]
        [System.ComponentModel.DataAnnotations.Compare("pass_word",ErrorMessage = "Password and confirm password do not match. ")]
        [DisplayName("Confirm Password")]
        public string Confirm_password { get; set; }

        //[StringLength(200)]
        //public string photo { get; set; }

        //[StringLength(250)]
        //public string brief { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News> News { get; set; }
    }
}
