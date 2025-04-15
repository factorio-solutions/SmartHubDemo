using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportTasksTest.Data;
using TransportTasksTest.OPCUAModule;

public class Program
{
    public static OPCMonitor Monitor { get; set; }

    private static void Main(string[] args)
    {
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
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        // Fix for CS1002 and CS1061: Ensure the method is called correctly with a semicolon and is defined as an extension method.
        //app.MapMissionStateCallback();

        app.Run();

        Monitor = new OPCMonitor();
        Monitor.Monitor(null);
    }
}

// Fix for CS1061: Define the extension method in a static class.
public static class WebApplicationExtensions
{
    public static void MapMissionStateCallback(this WebApplication app)
    {
        app.MapPost("/missionStateCallback", async (context) =>
        {
            try
            {
                // Read the request body
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                // Log the received payload
                Console.WriteLine($"Received callback: {requestBody}");

                // Respond with a success status
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync("Callback received successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors
                Console.WriteLine($"Error processing callback: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Error processing callback.");
            }
        });
    }
}