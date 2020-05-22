using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TakeTwo.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public IDbSet<Post> Posts { get; set; }
        public IDbSet<Comment> Comments { get; set; }

        public ApplicationDbContext()
            : base("TheatreDBV1", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DatabaseInitializer());
        }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}