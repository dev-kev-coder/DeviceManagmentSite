namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //// Simple GET returning a string
            //app.MapGet("/hello", () => "Hello World!");

            //// GET with route parameter
            //app.MapGet("/users/{id}", (int id) =>
            //{
            //    // Example static lookup
            //    var users = new[]
            //    {
            //        new { Id = 1, Name = "Alice" },
            //        new { Id = 2, Name = "Bob" }
            //    };
            //    var user = users.FirstOrDefault(u => u.Id == id);

            //    return user is not null ? Results.Ok(user) : Results.NotFound();
            //});

            app.MapControllers();
            app.UseAuthorization();
            app.MapRazorPages();
            app.Run();
        }
    }
}
