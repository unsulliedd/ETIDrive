using ETIDrive_Data;
using ETIDrive_Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Service container.
builder.Services.AddControllersWithViews();

// Configures ASP.NET Core Identity for the application with Entity Framework for data storage.
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ETIDriveContext>();

// Development connection string
string path = Directory.GetCurrentDirectory();
var connectionString = builder.Configuration.GetConnectionString("Development")?.Replace("[DataDirectory]", path);

// Using the SQL Server database with the provided connection string.
builder.Services.AddDbContext<ETIDriveContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
