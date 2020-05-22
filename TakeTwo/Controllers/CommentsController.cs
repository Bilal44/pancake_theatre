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
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateComment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateComment(int id, CommentInputModel model)
        {

            if (model != null && this.ModelState.IsValid)
            {
                var c = new Comment()
                {
                    Text = model.Text,
                    PostId = id,
                    AuthorId = this.User.Identity.GetUserId(),
                };

                this.db.Comments.Add(c);
                this.db.SaveChanges();
                this.AddNotification("Comment made", NotificationType.SUCCESS);
                return this.RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(db.Comments.Find(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Comments.Remove(db.Comments.Find(id));
            db.SaveChanges();
            this.AddNotification("Comment successfully delete :)", NotificationType.SUCCESS);
            return this.RedirectToAction("Index", "Home");
        }


        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}