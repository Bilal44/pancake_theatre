using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TakeTwo.Models;

namespace TakeTwo.Controllers
{
      public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var posts = this.db.Posts
                .OrderBy(p => p.CreatedAt)
                .Where(p => p.IsApproved)
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

        public ActionResult PostDetailById(int id)
            {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var isCustomer = this.IsCustomer();

            var postDetails = this.db.Posts
                .Where(p => p.Id == id)
                .Where(p => p.IsApproved || isAdmin || (p.AuthorId != null && p.AuthorId == currentUserId))
                .Select(PostDetailsViewModel.ViewModel).FirstOrDefault();

            var isOwner = (postDetails != null && postDetails.AuthorId != null &&
                postDetails.AuthorId == currentUserId);

            this.ViewBag.CanEdit = isOwner || isAdmin;

            this.ViewBag.CanComment = isCustomer;

            return this.PartialView("_PostDetails", postDetails);
        }
    }
}