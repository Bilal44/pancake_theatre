using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace TakeTwo.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        private ApplicationUserManager userManager;


        [Required]
        [Display(Name = "Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Is Suspended")]
        public bool IsSuspended { get; set; }

        private readonly List<IRole> _roles;



        //the CurrentRole property is not mapped as a field in the Users table
        //i neeed it to get the current role that the user is logged in

        [NotMapped]
        public string CurrentRole
        {
            get
            {
                if (userManager == null)
                {
                    userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    return userManager.GetRoles(Id).Single();
                }
                else
                {
                    return userManager.GetRoles(Id).Single();
                }
            }
            set
            {
                CurrentRole = value;
            }
        }

        public IEnumerable<SelectListItem> ShowRoles
        {
            get { return new SelectList(Roles1, "Id", "Name"); }
        }

        public List<IRole> Roles1 => _roles;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


}