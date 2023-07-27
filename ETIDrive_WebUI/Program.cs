using ETIDrive_Data;
using ETIDrive_Data.Abstract;
using ETIDrive_Data.Concrete;
using ETIDrive_Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Service container.
builder.Services.AddControllersWithViews();

// Configures ASP.NET Core Identity for the application with Entity Framework for data storage.
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ETIDriveContext>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

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

// Main Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
// Account Route
app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Account}/{action=Login}");
// Admin Route
app.MapControllerRoute(
    name: "adminpanel",
    pattern: "{controller=Admin}/{action=AdminPanel}");
app.MapControllerRoute(
    name: "users",
    pattern: "{controller=Admin}/{action=Users}");
app.MapControllerRoute(
    name: "createUser",
    pattern: "{controller=Admin}/{action=CreateUser}");
app.MapControllerRoute(
    name: "editUser",
    pattern: "{controller=Admin}/{action=EditUser}/{id?}");
app.MapControllerRoute(
    name: "roles",
    pattern: "{controller=Admin}/{action=Roles}");
app.MapControllerRoute(
    name: "createRole",
    pattern: "{controller=Admin}/{action=CreateRole}");
app.MapControllerRoute(
    name: "editRole",
    pattern: "{controller=Admin}/{action=EditRole}/{id?}");
app.MapControllerRoute(
    name: "department",
    pattern: "{controller=Admin}/{action=Departments}");
app.MapControllerRoute(
    name: "createDepartment",
    pattern: "{controller=Admin}/{action=CreateDepartment}");
app.Run();
