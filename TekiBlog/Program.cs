using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TekiBlog.Data;
using TekiBlog.Models;

namespace TekiBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            try
            {
                // Define scope to get service
                var scope = host.Services.CreateScope();
                // Get database context , user manager and role manager service
                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                ctx.Database.EnsureCreated();
                // Create admin Role and user Role
                var adminRole = new IdentityRole("Admin");
                var userRole = new IdentityRole("User");

                if (!ctx.Roles.Any()) // If there are no any role 
                {
                    // Add admine and user role to DB
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(userRole).GetAwaiter().GetResult();
                }

                if (!ctx.Users.Any(u => u.UserName == "Admin")) // If there are no user with name admin
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = "phamhoangbao@gmail.com",
                        FirstName = "Pham Hoang",
                        LastName = "Bao",
                    };
                    // Create new user name Admin and password is password
                    userMgr.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
                    // Add new admin role to this user
                    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
