using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TakeTwo.Models;

namespace TakeTwo.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public bool IsAdmin()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = (currentUserId != null && this.User.IsInRole("Administrator"));
            return isAdmin;
        }


        public bool IsCustomer()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isCustomer = (currentUserId != null && this.User.IsInRole("Customer"));
            return isCustomer;
        }
    }


}