namespace NewsWebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public int Id { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage ="*")]
        [DisplayName("Title")]
        public string title { get; set; }

        //[StringLength(300)]
        //public string brief { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(500)]
        [DisplayName("Decription")]
        public string decription { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Date")]
        public DateTime? Dateofoccurrence { get; set; }

        [StringLength(200)]
        public string image { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage ="*")]
        public int Category_Id { get; set; }

        public int userId { get; set; }

        public virtual Category Category { get; set; }

        public virtual user user { get; set; }
    }
}
