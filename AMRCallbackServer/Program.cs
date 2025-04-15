namespace AMRCallbackServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure the app to listen on all network interfaces
            builder.WebHost.UseUrls("http://0.0.0.0:7042");

            // Disable HTTPS redirection to avoid SSL-related issues
            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = null; // Disable HTTPS redirection
            });

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
