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
    public class CommentController : BaseController
    {

        [HttpGet]
        public ActionResult CreateComment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateComment(int id, CommentInputModel model)
        {

            if (model != null && ModelState.IsValid)
            {
                var c = new Comment()
                {
                    Text = model.Text,
                    PostId = id,
                    AuthorId = User.Identity.GetUserId(),
                };

                db.Comments.Add(c);
                db.SaveChanges();

                this.AddNotification("Comment posted", NotificationType.SUCCESS);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);

            db.Comments.Remove(comment);
            db.SaveChanges();
            this.AddNotification("Comment successfully delete :)", NotificationType.SUCCESS);
            return RedirectToAction("Index", "Home");
        }
    }
}