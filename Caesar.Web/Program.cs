using Caesar.Core.Interfaces;
using Caesar.Core.Services;
using Caesar.Infrastructure.Data;
using Caesar.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Caesar.Web.Validators;

namespace Caesar.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<IMenuItemService, MenuItemService>();
        builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

        builder.Services.AddDbContext<CaesarDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

        // Session
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // FluentValidation setup
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterViewModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LoginViewModelValidator>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Menu}/{action=Index}/{id?}");

        app.Run();
    }
}
