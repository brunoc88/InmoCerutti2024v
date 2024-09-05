using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => //validacion con cookies
{
    options.LoginPath = "/Usuario/Login";
    options.LogoutPath = "/Usuario/Logout";
    options.AccessDeniedPath = "/Home/Privacy";
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("Administador", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Administrador");//indico que para administrador solo pede ser admin.
    });
    /*option.AddPolicy("Empleado",policy=>{
        policy.RequireRole("Administrador", "Empleado");//indico que un empleado puede ser un admin o simple empleado.
    });*/
    //ambos metodos son equivalentes
});

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

app.UseAuthentication();//habilito autenticacion
app.UseAuthorization();//habilito autorizacion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
