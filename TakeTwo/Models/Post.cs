using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TakeTwo.Models
{
    public class Post
    {
        public Post()
        {
            this.IsApproved = true;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public string Category { get; set; }

        public bool IsApproved { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }


    }
}
