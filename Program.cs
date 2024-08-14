using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

public class Program
{
    // Entry point of the application
    public static void Main(string[] args)
    {
        // Create and run the web host
        CreateHostBuilder(args).Build().Run();
    }

    // Method to configure and build the host
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args) // Default configuration provided by ASP.NET Core
            .ConfigureWebHostDefaults(webBuilder => // Configure the web server settings
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    // Configure the server to listen on port 5000 for incoming requests
                    serverOptions.ListenAnyIP(5000);
                });
                webBuilder.Configure(app =>
                {
                    // Serve static files from a specified directory
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(
                            // Set the directory from which to serve the static files
                            Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\wwwroot\\images")),
                        RequestPath = "" // Serve files at the root of the application
                    });

                    // Enable routing in the application
                    app.UseRouting();

                    // Configure the application's endpoints
                    app.UseEndpoints(endpoints =>
                    {
                        // Define a route that responds to GET requests at the root URL "/"
                        endpoints.MapGet("/", async context =>
                        {
                            // Set the response content type to HTML
                            context.Response.ContentType = "text/html";

                            // Write an HTML response that includes the three images
                            await context.Response.WriteAsync(@"
                                <html>
                                <body>
                                    <h1>Here are 3 images from my travel abroad</h1>
                                    <img src='/Chisinau.jpg' alt='Image of an orange cat' width='390' height='520'/>
                                    <img src='/Baku.jpg' alt='Image of an old car' width='520' height='390'/>
                                    <img src='/Prizren.jpg' alt='Image of big green leaves' width='390' height='520'/>
                                </body>
                                </html>");
                        });
                    });
                });
            });
}
