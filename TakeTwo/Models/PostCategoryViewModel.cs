using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TakeTwo.Models
{
    public class PostCategoryViewModel
    {
        public IEnumerable<PostViewModel> Announcements { get; set; }
        public IEnumerable<PostViewModel> Reviews { get; set; }
        public IEnumerable<PostViewModel> Other { get; set; }
    }
}