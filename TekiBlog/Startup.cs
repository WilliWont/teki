using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataObjects;
using BusinessObjects;
using ActionServices;
using ValidationUtilities;
using TekiBlog.ViewModels;

namespace TekiBlog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Register DB Context. This method is add many of service revelant to DBContext to service collection . 
            // We can use this service any time by using dependency injection
            services.AddDbContext<ApplicationDBContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Register my app service. This includes CRUD function in every repository.
            services.AddTransient<IService, Service>();

            services.AddTransient<IGenericValidationUtil<CreateArticleViewModel>, 
                                GenericValidationUtil<CreateArticleViewModel>>();

            // Register Identity service . This method is add many of service revelant to Identity to service collection
            // like UserManager , RoleManager or SigninManager.
            // We can use this service any time by using dependency injection
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password require validations
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDBContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Error/403";
            });

            // adds configuration
            services.AddSingleton<IConfiguration>(Configuration);

            // Add service for MVC 
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Article}/{action=Home}/{id?}");
            });
        }
    }
}
