using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TakeTwo.Extensions;
using TakeTwo.Models;

namespace TakeTwo.Controllers
{
    //[Authorize] 
    public class PostsController : BaseController
    {
        // GET: Posts
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Administrator,Staff")]
        [HttpGet]
        public ActionResult Create(PostViewModel model)
        {
            return View();
        }

        //GET: Posts/Create
        [Authorize(Roles = "Administrator,Staff")]
        [HttpPost]
        public ActionResult Create(PostInputModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var p = new Post()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    Body = model.Body,
                    Category = model.Category,
                    IsApproved = model.IsApproved
                };
                this.db.Posts.Add(p);
                this.db.SaveChanges();
                this.AddNotification("Post Created", NotificationType.INFO);
                return this.RedirectToAction("UserPosts");
            }
            return this.View(model);
        }

        //GET Posts/My
        public ActionResult UserPosts()
        {
            string currentUserId = User.Identity.GetUserId();
            var posts = db.Posts.Where(p => p.AuthorId == currentUserId)
                .OrderBy(p => p.CreatedAt);

            return View(posts);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            return View(post);
        }

        [HttpPost]
        public ActionResult Edit(int id, PostInputModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = db.Posts.Find(id);
                post.Title = model.Title;
                post.Body = model.Body;
                post.Category = model.Category;
                post.IsApproved = false;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);

            db.Posts.Remove(post);
            db.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }



    }
}