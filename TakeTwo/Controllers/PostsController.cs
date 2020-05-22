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
                return this.RedirectToAction("My");
            }
            return this.View(model);
        }

        //GET Posts/My
        public ActionResult My()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var posts = this.db.Posts.Where(p => p.AuthorId == currentUserId)
                .OrderBy(p => p.CreatedAt)
                .Select(PostViewModel.ViewModel);

            var announcements = posts.Where(p => p.Category == "Announcements");
            var reviews = posts.Where(p => p.Category == "Reviews");
            var other = posts.Where(p => p.Category == "Other");

            return View(new PostCategoryViewModel()
            {
                Announcements = announcements,
                Reviews = reviews,
                Other = other
            });
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