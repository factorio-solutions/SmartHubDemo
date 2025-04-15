using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportTasksTest.Data;
using TransportTasksTest.OPCUAModule;

public class Program
{
    public static OPCMonitor Monitor { get; set; }

    private static void Main(string[] args)
    {
        Monitor = new OPCMonitor();
        Monitor.Monitor(null);

        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseUrls("http://0.0.0.0:7042");

        builder.Services.AddHttpsRedirection(options =>
        {
            options.HttpsPort = null; // Disable HTTPS redirection
        });

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "MissionStateCallback",
            pattern: "MissionStateCallback/{action=Index}/{id?}",
            defaults: new { controller = "MissionStateCallback" }
        );

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );


        app.MapRazorPages();

        app.Run();
    }
}

// Fix for CS1061: Define the extension method in a static class.
