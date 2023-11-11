using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using AuthorManagement_CRUD.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AuthorDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("appCon")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();