using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TakeTwo.Models;

namespace TakeTwo.Controllers
{
    [Authorize(Roles = "Administrator")] //Only allow Admins to access the Admin Controller! :)
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult AdminDashboard()
        {
            return View();
        }

        public ActionResult ViewAllUsers()
        {
            return View(db.Users.ToList());
        }


        public ActionResult ViewAllPosts()
        {
            var posts = db.Posts;

            return View(posts.ToList());
        }

        //Finds and approves a post
        public ActionResult ApprovePost(int id)
        {
            Post post = db.Posts.Find(id); //Find post in db
            post.IsApproved = true; //Set approved bool to true
            db.SaveChanges(); //Save db
            return RedirectToAction("ViewAllPosts"); //Rerender page
        }

        //Finds and disables a post
        public ActionResult DisablePost(int id)
        {
            Post post = db.Posts.Find(id); //Find post in db
            post.IsApproved = false; //Set approved bool to false
            db.SaveChanges(); //Save db
            return RedirectToAction("ViewAllPosts"); //Rerender page
        }

        
        public ActionResult SuspendUser(string id)
        {
            User user = db.Users.Find(id);
            user.IsSuspended = true;
            user.LockoutEnabled = true;
            user.LockoutEndDateUtc = DateTime.Now.AddDays(1); //suspend the user for 1 day
            db.SaveChanges();
            return RedirectToAction("ViewAllUsers");
        }

        public ActionResult UnsuspendUser(string id)
        {
            User user = db.Users.Find(id);
            user.IsSuspended = false;
            user.LockoutEnabled = false;
            user.LockoutEndDateUtc = DateTime.Now; //Ends the user lockout
            db.SaveChanges();
            return RedirectToAction("ViewAllUsers");
        }

    }
}