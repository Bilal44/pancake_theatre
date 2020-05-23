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
    [Authorize(Roles = "Administrator,Staff")]
    public class PostsController : BaseController
    {
        [HttpGet]
        public ActionResult CreatePost(PostViewModel model)
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePost(PostInputModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var p = new Post()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    Body = model.Body,
                    Category = model.Category,
                    IsApproved = model.IsApproved
                };

                db.Posts.Add(p);
                db.SaveChanges();

                this.AddNotification("Post submitted for Review", NotificationType.INFO);

                return RedirectToAction("UserPosts");
            }

            return View(model);
        }

        public ActionResult UserPosts()
        {
            string currentUserId = User.Identity.GetUserId();
            var posts = db.Posts.Where(p => p.AuthorId == currentUserId)
                .OrderBy(p => p.CreatedAt);

            return View(posts);
        }

        [HttpGet]
        public ActionResult EditPost(int id)
        {
            Post post = db.Posts.Find(id);

            return View(post);
        }

        [HttpPost]
        public ActionResult EditPost(int id, PostInputModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = db.Posts.Find(id);
                post.Title = model.Title;
                post.Body = model.Body;
                post.Category = model.Category;
                post.IsApproved = false;

                db.SaveChanges();

                this.AddNotification("Edit submitted for Review", NotificationType.INFO);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public ActionResult DeletePost(int id)
        {
            Post post = db.Posts.Find(id);

            db.Posts.Remove(post);
            db.SaveChanges();

            this.AddNotification("Post deleted :(", NotificationType.SUCCESS);

            return RedirectToAction("Index", "Home");
        }



    }
}