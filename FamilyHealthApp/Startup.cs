using FamilyHealthApp.Models;
using FamilyHealthApp.Repository;
using FamilyHealthApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyHealthApp
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
            // PostreSQL setup
            var connectionString = Configuration.GetConnectionString("FamilyHealthApp");
            services.AddDbContext<ProfileContext>(options =>
                options.UseNpgsql(connectionString));
            
            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ProfileContext>()
                .AddDefaultTokenProviders();

            // Less complicated password for testing
            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            
            // Makes Mail service avaliable for controller
            services.AddTransient<IEmailSend, MessegeSend>();
            // Makes ProfileReposity avaliable for controller
            services.AddTransient<ProfileRepository>();
            // Email service
            services.Configure<MessegeSenderOptions>(Configuration);
            // As MVC projekt
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ProfileContext context)
        { 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Main}/{action=Index}/{id?}");
            });


        }
    }
}