using AccuApp32MVC.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DataModel.Finance;
using WebDB;
using PhlebotomyDB;
using InventoryDB;
using AccuApp.Services;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Collections.Generic;
using AccuApp32MVC.Models.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using DelegateDecompiler;
using AccuApp32MVC.Models;
using System.Linq;
using System.Data.SqlClient;
using AccuApp32.Services;

namespace AccuApp31MVC
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


            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpContextAccessor();

            int commandTimeout = 300;

            string password = System.Environment.GetEnvironmentVariable("DBPassword");

            var builder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("WebDB"));
            builder.Password = password;

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.ConnectionString, sqlServerOptionsAction: opts =>
                {
                    opts.EnableRetryOnFailure(
                         maxRetryCount: 10,
                         maxRetryDelay: TimeSpan.FromSeconds(5),
                         errorNumbersToAdd: null
                        ).CommandTimeout(commandTimeout);
                });
            });

            var builderWebDb = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("WebDB"));
            builderWebDb.Password = password;

            services.AddDbContext<WebdbContext>(options =>
            {
                options.UseSqlServer(builderWebDb.ConnectionString, sqlServerOptionsAction: opts =>
                {
                    opts.EnableRetryOnFailure(
                         maxRetryCount: 10,
                         maxRetryDelay: TimeSpan.FromSeconds(5),
                         errorNumbersToAdd: null
                        ).CommandTimeout(commandTimeout);
                });
            });

            var builderFinanceDb = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("FINANCE"));
            builderFinanceDb.Password = password;

            services.AddDbContext<FINANCEContext>(options =>
            {
                options.UseSqlServer(builderFinanceDb.ConnectionString, sqlServerOptionsAction: opts =>
                {
                    opts.EnableRetryOnFailure(
                         maxRetryCount: 10,
                         maxRetryDelay: TimeSpan.FromSeconds(5),
                         errorNumbersToAdd: null
                        ).CommandTimeout(commandTimeout);
                });
            });

            var builderPhlebotomyDb = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("Phlebotomy"));
            builderPhlebotomyDb.Password = password;

            services.AddDbContext<PhlebotomyContext>(options =>
            {
                options.UseSqlServer(builderPhlebotomyDb.ConnectionString, sqlServerOptionsAction: opts =>
                {
                    opts.EnableRetryOnFailure(
                         maxRetryCount: 10,
                         maxRetryDelay: TimeSpan.FromSeconds(5),
                         errorNumbersToAdd: null
                        ).CommandTimeout(commandTimeout);
                });
            });

            var builderInventoryDb = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("Inventory"));
            builderInventoryDb.Password = password;

            services.AddDbContext<InventoryContext>(options =>
            {
                options.UseSqlServer(builderInventoryDb.ConnectionString, sqlServerOptionsAction: opts =>
                {
                    opts.EnableRetryOnFailure(
                         maxRetryCount: 10,
                         maxRetryDelay: TimeSpan.FromSeconds(5),
                         errorNumbersToAdd: null
                        ).CommandTimeout(commandTimeout);
                });
            });


            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.AllowedUserNameCharacters += "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM!@#";
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddScoped<FINANCEContextProcedures>();
            services.AddScoped<WebdbContextProcedures>();
            services.AddScoped<InventoryContextProcedures>();

            services.AddControllersWithViews();

            services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.LoginPath = "/Identity/account/login";
                    options.LogoutPath = "/Identity/account/logout";
                });

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IFaxSender, FaxSender>();
            services.AddSingleton<IInvoiceCreator, InvoiceCreator>();
            services.AddSingleton<IDistanceCounter, DistanceCounter>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            var keys = new Dictionary<string, string>();
            keys.Add("SendGridUser", System.Environment.GetEnvironmentVariable("SendGridUser"));
            keys.Add("SendGridKey", System.Environment.GetEnvironmentVariable("SendGridKey"));
            var builder2 = new ConfigurationBuilder();
            builder2.AddInMemoryCollection(keys);
            services.Configure<AuthMessageSenderOptions>(builder2.Build());
            DelegateDecompiler.Configuration.Configure(new DelegateDecompilerConfiguration());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IServiceProvider svp)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            //add admin in empty database
            var umgr = svp.GetService<UserManager<IdentityUser>>();
            var rmgr = svp.GetService<RoleManager<IdentityRole>>();

            var fullAdminRole = rmgr.FindByNameAsync("Admin").Result;
            if (fullAdminRole == null)
            {
                fullAdminRole = new IdentityRole
                {
                    Name = "Admin"
                };
                rmgr.CreateAsync(fullAdminRole);
            }

            var admin = umgr.FindByNameAsync("kb@drensys.com").Result;
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "kb@drensys.com",
                    Email="kb@drensys.com",
                    EmailConfirmed=true
                };
                string password = "DrenTech@123!";
                var result = umgr.CreateAsync(admin, password).Result;

                var adminRole = rmgr.FindByNameAsync("Admin").Result;

                admin = umgr.FindByNameAsync("kb@drensys.com").Result;
                umgr.AddToRoleAsync(admin, "Admin");
            }
            else
            {
                var existAdminRole = umgr.GetRolesAsync(admin).Result.Any(t => t.ToUpper() == "ADMIN");
                if (!existAdminRole)
                {
                    var adminRole = rmgr.FindByNameAsync("Admin").Result;
                    admin = umgr.FindByNameAsync("kb@drensys.com").Result;
                    umgr.AddToRoleAsync(admin, "Admin");
                }
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
    public class DelegateDecompilerConfiguration : DefaultConfiguration
    {
        public override bool ShouldDecompile(MemberInfo memberInfo)
        {
            // Automatically decompile all NotMapped members
            return base.ShouldDecompile(memberInfo) || memberInfo.GetCustomAttributes(typeof(NotMappedAttribute), true).Length > 0;
        }
    }
}
