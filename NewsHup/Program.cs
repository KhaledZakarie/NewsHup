namespace NewsHup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // // Add services to the container.
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            //U have to run the  following command: Install-Package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation



            var app = builder.Build();

            // Disable Browser Link for now
            //if (app.Environment.IsDevelopment())
            //{
            //    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //}
            //else
            //{
            //    builder.Services.AddControllersWithViews();
            //}



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Add the admin route
            app.MapControllerRoute(
                name: "admin",
                pattern: "admin",
                defaults: new { controller = "Home", action = "Dashboard" });

            // Existing default route

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
