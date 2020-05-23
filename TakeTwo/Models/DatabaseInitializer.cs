using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web;

namespace TakeTwo.Models
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            //Seed initial data only if the database is empty
            if (!context.Users.Any())
            {
                var adminEmail = "admin@pancake.com";
                var adminUserName = adminEmail;
                var adminFullName = "System Admin";
                var adminPassword = adminEmail;
                string adminRole = "Administrator";


                var staffEmail = "staff@pancake.com";
                var staffUserName = staffEmail;
                var staffFullName = "Pancake Staff";
                var staffPassword = staffEmail;
                string staffRole = "Staff";

                var customerEmail = "manager@pancake.com";
                var customerUserName = customerEmail;
                var customerFullName = "Pancake Manager";
                var customerPassword = customerEmail;
                string customerRole = "Staff";

                var suspendedEmail = "suspended@pancake.com";
                var suspendedUserName = suspendedEmail;
                var suspendedFullName = "Naughty Boy";
                var suspendedPassword = suspendedEmail;
                string suspendedRole = "Staff";

                CreateAdminUser(context, adminEmail, adminUserName, adminFullName, adminPassword, adminRole);
                CreateStaffUser(context, staffEmail, staffUserName, staffFullName, staffPassword, staffRole);
                CreateStaffUser(context, customerEmail, customerUserName, customerFullName, customerPassword, customerRole);
                CreateSuspendedUser(context, suspendedEmail, suspendedUserName, suspendedFullName, suspendedPassword, suspendedRole);
                CreateSeveralPosts(context);
                context.SaveChanges();
            }
        }
        private void CreateSeveralPosts(ApplicationDbContext context)
        {

            context.Posts.Add(new Post()
            {
                Title = "Wolf subway tile selfies cardigan",
                Body = "I'm baby kickstarter messenger bag coloring book green juice. Knausgaard franzen enamel pin raw denim jianbing hexagon pork belly lomo gochujang truffaut. Banh mi keffiyeh adaptogen live-edge, 8-bit cardigan occupy disrupt lo-fi tousled pork belly. Try-hard glossier thundercats subway tile.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Category = "Announcements",
                AuthorId = context.Users.First().Id,
                Comments = new HashSet<Comment>()
                    {
                        new Comment() { Text = "tacos && thundercats"},
                        new Comment() { Text = "XOXO cornhole", Author = context.Users.First()}
                    },
                IsApproved = true



            });

            context.Posts.Add(new Post()
            {
                Title = "Thundercats glossier man bun +1",
                Body = "Man braid freegan fanny pack tumeric kombucha biodiesel synth tousled. Shabby chic sartorial pour-over offal. Kinfolk adaptogen cornhole microdosing, offal iPhone waistcoat seitan lomo. Brunch lomo you probably haven't heard of them, salvia tacos woke pinterest sriracha succulents everyday carry cred plaid tote bag post-ironic air plant. Four loko vape before they sold out, yuccie banjo adaptogen tousled hell of +1 aesthetic flexitarian edison bulb asymmetrical. Tacos tumeric bushwick XOXO woke forage, listicle chillwave cloud bread humblebrag single-origin coffee.",
                Category = "Announcements",
                AuthorId = context.Users.First().Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,

                Comments = new HashSet<Comment>()
                    {
                        new Comment() { Text = "Pork belly small batch sartorial, selvage master cleanse sriracha scenester?!?"}

                    }

            });

            context.Posts.Add(new Post()
            {
                Title = "Kogi williamsburg scenester pickled live-edge",
                Body = "Paleo bushwick vice, meggings cloud bread meditation pabst williamsburg. Tattooed actually pug poke, meditation drinking vinegar taxidermy keytar green juice craft beer. Quinoa squid lumbersexual XOXO cornhole vinyl, palo santo portland synth. Salvia selvage slow-carb four dollar toast master cleanse iPhone migas tilde irony messenger bag sriracha art party.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Category = "Reviews",
                AuthorId = context.Users.First().Id,
                Comments = new HashSet<Comment>()
                    {
                        new Comment() { Text = "Drinking vinegar 8-bit helvetica offal, deep v kitsch asymmetrical fanny pack cloud bread etsy crucifix cray coloring book."},
                        new Comment() { Text = "Fanny pack pitchfork pug, pour-over fixie meggings tacos thundercats art party occupy vaporware squid next level pop-up schlitz.", Author = context.Users.First()}
                    },
                IsApproved = true

            });

            context.Posts.Add(new Post()
            {
                Title = " iPhone kitsch single-origin coffee vice marfa coloring book.",
                Body = "Tbh disrupt cliche la croix. Mustache art party hella banh mi, schlitz quinoa iceland unicorn man bun lomo farm-to-table iPhone small batch selvage. Pork belly small batch sartorial, selvage master cleanse sriracha scenester. Authentic subway tile cloud bread sustainable, irony keytar kitsch chambray artisan pickled plaid aesthetic wolf cred. Microdosing cold-pressed 3 wolf moon food truck.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Category = "Other",
                AuthorId = context.Users.First().Id,
                Comments = new HashSet<Comment>()
                    {
                        new Comment() { Text = "Mumblecore schlitz raclette pug."},
                        new Comment() { Text = "Etsy schlitz dreamcatcher austin synth +1 kickstarter stumptown 90's vexillologist ethical portland shabby chic!", Author = context.Users.First()}
                    },
                IsApproved = true

            });

        }

        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUserName, string adminFullName, string adminPassword, string adminRole)
        {
            var adminUser = new User
            {
                UserName = adminUserName,
                FullName = adminFullName,
                Email = adminEmail
            };
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Create the 'admin' role

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            //Add teh admin user to the admin role
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));

            }
        }
        public void CreateStaffUser(ApplicationDbContext context, string staffEmail, string staffUserName, string staffFullName, string staffPassword, string staffRole)
        {
            var staffUser = new User
            {
                UserName = staffUserName,
                FullName = staffFullName,
                Email = staffEmail
            };
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };
            var userCreateResult = userManager.Create(staffUser, staffPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Create the staff role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(staffRole)) {
                var roleCreateResult = roleManager.Create(new IdentityRole(staffRole));
                if (!roleCreateResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", roleCreateResult.Errors));
                } 
            }

            //Add the staff user to the staff role
            var addStaffRoleResult = userManager.AddToRole(staffUser.Id, staffRole);
            if (!addStaffRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addStaffRoleResult.Errors));
            }
        }
        public void CreateSuspendedUser(ApplicationDbContext context, string customerEmail, string customerUserName, string customerFullName, string customerPassword, string customerRole)
        {
            var customerUser = new User
            {
                UserName = customerUserName,
                FullName = customerFullName,
                Email = customerEmail,
                LockoutEndDateUtc = DateTime.Now.AddDays(1),
                LockoutEnabled = true,
                IsSuspended = true             
            };
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };
            var userCreateResult = userManager.Create(customerUser, customerPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Create the customer role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(customerRole))
            {
                var roleCreateResult = roleManager.Create(new IdentityRole(customerRole));
                if (!roleCreateResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", roleCreateResult.Errors));
                }
            }

            //Add the customer user to the customer role
            var addCustomerRoleResult = userManager.AddToRole(customerUser.Id, customerRole);
            if (!addCustomerRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addCustomerRoleResult.Errors));
            }

        }


    }
}