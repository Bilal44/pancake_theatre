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
                var adminEmail = "admin@admin.com";
                var adminUserName = adminEmail;
                var adminFullName = "System Admin";
                var adminPassword = adminEmail;
                string adminRole = "Administrator";


                var staffEmail = "staff@staff.com";
                var staffUserName = staffEmail;
                var staffFullName = "Mr Staff";
                var staffPassword = staffEmail;
                string staffRole = "Staff";

                var customerEmail = "customer@customer.com";
                var customerUserName = customerEmail;
                var customerFullName = "Mr Customer";
                var customerPassword = customerEmail;
                string customerRole = "Customer";


                CreateAdminUser(context, adminEmail, adminUserName, adminFullName, adminPassword, adminRole);
                CreateStaffUser(context, staffEmail, staffUserName, staffFullName, staffPassword, staffRole);
                CreateCustomerUser(context, customerEmail, customerUserName, customerFullName, customerPassword, customerRole);
                CreateSeveralPosts(context);
                context.SaveChanges();
            }
        }
        private void CreateSeveralPosts(ApplicationDbContext context)
        {

            context.Posts.Add(new Post()
            {
                Title = "This is an announcement",
                Body = "Lorem Ipsum",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Category = "Announcements",
                AuthorId = context.Users.First().Id,
                Comments = new HashSet<Comment>()
                    {
                        new Comment() { Text = "This is a comment"},
                        new Comment() { Text = "This is another comment", Author = context.Users.First()}
                    },
                IsApproved = true



            });

            context.Posts.Add(new Post()
            {
                Title = "This is the second announcement",
                Body = "Two Ipsum",
                Category = "Announcements",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,

                Comments = new HashSet<Comment>()
                    {
                        new Comment() { Text = "This is the second comment"}

                    }

            });

            context.Posts.Add(new Post()
            {
                Title = "This is a review",
                Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Category = "Reviews",
                AuthorId = context.Users.First().Id,
                Comments = new HashSet<Comment>()
                    {
                        new Comment() { Text = "This is a comment"},
                        new Comment() { Text = "This is another comment", Author = context.Users.First()}
                    },
                IsApproved = true

            });

            context.Posts.Add(new Post()
            {
                Title = "This is OTHER STUFF",
                Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Category = "Other",
                AuthorId = context.Users.First().Id,
                Comments = new HashSet<Comment>()
                    {
                        new Comment() { Text = "This is a comment"},
                        new Comment() { Text = "This is another comment", Author = context.Users.First()}
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
            var roleCreateResult = roleManager.Create(new IdentityRole(staffRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            //Add the staff user to the staff role
            var addStaffRoleResult = userManager.AddToRole(staffUser.Id, staffRole);
            if (!addStaffRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addStaffRoleResult.Errors));
            }
        }
        public void CreateCustomerUser(ApplicationDbContext context, string customerEmail, string customerUserName, string customerFullName, string customerPassword, string customerRole)
        {
            var customerUser = new User
            {
                UserName = customerUserName,
                FullName = customerFullName,
                Email = customerEmail,
                IsSuspended = false
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
            var roleCreateResult = roleManager.Create(new IdentityRole(customerRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
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