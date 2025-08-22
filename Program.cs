using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskHive.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Server=localhost,1433;Database=TaskHiveMvcDb;User Id=sa;Password=MyStrongPass123!;TrustServerCertificate=True;";

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<Users, IdentityRole>(options=>
{
    options.User.RequireUniqueEmail = true;
}
)
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<TaskHiveDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

