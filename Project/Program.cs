using Core.Services.Interfaces;
using Core.Services;
using Datalayer.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Zarinpal.AspNetCore.Extensions;
using Zarinpal.AspNetCore.Enums;
using Rotativa.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;

namespace dastgire
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Warning);

            builder.Services.AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddZarinpal(options =>
            {
                options.MerchantId = "8542f6e7-5f9b-467f-9ef9-a2ab053301e0";
                options.ZarinpalMode = ZarinpalMode.Original;
            });

            builder.Services.AddHangfire((sp, config) =>
            {
                var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
                config.UseSqlServerStorage(connectionString,
                new SqlServerStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromMinutes(10),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(10),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),

                });

            });


            builder.Services.AddHangfireServer();

            #region Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";


            });

            #endregion

            #region IOC

            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IToolsService, ToolsService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IMediaService, MediaService>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<ISettingService, SettingService>();
            builder.Services.AddTransient<ICommentService, CommentService>();
            builder.Services.AddTransient<IPostService, PostService>();
            builder.Services.AddTransient<IPageService, PageService>();

            #endregion

            // Add services to the container.
            builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(
               options => options
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
              );

            app.UseDeveloperExceptionPage();
            app.UseAuthorization();

            //app.UseHangfireDashboard("/Tasks", new DashboardOptions
            //{
            //    Authorization = new[]
            //    {
            //        new HangfireCustomBasicAuthenticationFilter
            //        {
            //            User = "admin",
            //            Pass="WinVia@1591230!",
            //        }
            //    }
            //});

            app.MapControllerRoute(
               name: "MyArea",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseRotativa();

            app.Run();
        }
    }
}
