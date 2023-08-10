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
builder.Services.AddScoped<IFolderRepository, FolderRepository>();

// Development connection string
string path = Directory.GetCurrentDirectory();
var connectionString = builder.Configuration.GetConnectionString("Development")?.Replace("[DataDirectory]", path);

//Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(61);
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = ".ETIMaden.Account.Cookie"
    };
});

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
    pattern: "{controller=Folder}/{action=UserFolder}");
app.MapControllerRoute(
    name: "createDepartment",
    pattern: "{controller=Folder}/{action=CreateFolder}/{id?}");
// Account Route
app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Account}/{action=Login}");
app.MapControllerRoute(
    name: "accessDenied",
    pattern: "{controller=Account}/{action=AccessDenied}");
// Folder Route
app.MapControllerRoute(
    name: "FodlerUserList",
    pattern: "{controller=Folder}/{action=GetUserList}");
app.MapControllerRoute(
    name: "folderContent",
    pattern: "{controller=Folder}/{action=FolderContent}/{id?}");
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
