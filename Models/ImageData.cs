using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class ImageData
    {
        [Required (ErrorMessage ="*")]
        public string image { get; set; }
    }
}