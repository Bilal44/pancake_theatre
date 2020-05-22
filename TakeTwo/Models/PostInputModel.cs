using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TakeTwo.Models
{
    public class PostInputModel
    {
        [Required(ErrorMessage = "Post title is required")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string Body { get; set; }
        [Display(Name = "Is Approved?")]
        public bool IsApproved { get; set; }

        public string Category { get; set; }
    }
}