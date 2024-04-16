using Microsoft.EntityFrameworkCore;
using GestionEmpledo.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BaseContext>(options =>
                            options.UseMySql(
                                builder.Configuration.GetConnectionString("MySqlConnection"),
                                Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.20-mysql")));
// Agrega IHttpContextAccessor al contenedor de servicios
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();