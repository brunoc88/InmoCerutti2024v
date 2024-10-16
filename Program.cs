using System.Configuration;
using System.Security.Claims;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


//creo el builder
//Este objeto permite configurar los servicios de la aplicación y cargar las configuraciones.
var builder = WebApplication.CreateBuilder(args);

// Agregar configuración desde appsettings.json
//La variable configuration carga todas las configuraciones definidas en appsettings.json 
//para ser utilizadas más adelante, como la cadena de conexión a la base de datos.
var configuration = builder.Configuration;

// Add services to the container.
//AddControllersWithViews() agrega soporte para controladores con vistas (MVC), lo que te permite definir controladores que gestionan vistas en HTML.
builder.Services.AddControllersWithViews();

//autenticación usando cookies, que almacenarán las credenciales del usuario.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => //validacion con cookies
{
    options.LoginPath = "/Home/Login"; //pagina para loguearse
    options.LogoutPath = "/Home/Logout";//pagina para desloguearse
    options.AccessDeniedPath = "/Home/Privacy";//pagina de acceso denegado
});


//Aquí defines políticas de autorización, específicamente que solo los usuarios con el rol de Administrador pueden acceder a ciertas áreas de la aplicación.
//Se usa ClaimTypes.Role para especificar qué rol es requerido (en este caso, "Administrador").
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("Administador", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Administrador");//indico que para administrador solo puede ser admin.
    });
    /*option.AddPolicy("Empleado",policy=>{
        policy.RequireRole("Administrador", "Empleado");//indico que un empleado puede ser un admin o simple empleado.
    });*/
    //ambos metodos son equivalentes
});

//conexion a DB solo con Entity Framework
//proveedor Pomelo
//Usa la configuración de conexión definida en appsettings.json bajo DefaultConnection para conectarse a la base de datos.
//ServerVersion.AutoDetect() detecta automáticamente la versión de MySQL que estás utilizando.

builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
    )
);


//Se construye la aplicación a partir del builder.
var app = builder.Build();

//Habilitar CORS

app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
    });



// Configure the HTTP request pipeline.
//Manejo de Errores y Seguridad
//Si la aplicación no está en modo de desarrollo, se configura un controlador de excepciones (ExceptionHandler) para redirigir a la página /Home/Error en caso de errores.
//HSTS es una característica de seguridad que obliga al navegador a usar HTTPS para todas las comunicaciones con el servidor.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); //Redirige automáticamente todas las solicitudes HTTP a HTTPS.
app.UseStaticFiles();//Permite servir archivos estáticos (como imágenes, CSS, JavaScript) desde la carpeta wwwroot.

app.UseRouting();//Habilita el enrutamiento, lo que permite que ASP.NET Core dirija las solicitudes a los controladores y acciones correctos.

app.UseAuthentication();//habilito autenticacion
app.UseAuthorization();//habilito autorizacion
//para que las rutas estén protegidas según las políticas definidas.
/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();*/

//Aquí defines la ruta predeterminada para la aplicación
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();//Finalmente, la aplicación se ejecuta

