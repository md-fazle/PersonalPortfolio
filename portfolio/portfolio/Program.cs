using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using portfolio.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure the database context with a connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomPolicy", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://127.0.0.1:5500") // Adjust this to match your frontend URL
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials(); // Optional, only if cookies/authentication are required
    });
});

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enforces the use of HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serves static files from wwwroot

app.UseRouting(); // Enables routing

// Use the CORS policy defined above
app.UseCors("CustomPolicy");

app.UseAuthorization(); // Adds authorization middleware

// Configure the default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map API endpoints for controllers
app.MapControllers();

app.Run(); // Starts the application
