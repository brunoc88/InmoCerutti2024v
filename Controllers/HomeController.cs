using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_Cerutti.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria_Cerutti.UsuarioController;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(string email, string clave)
    {
        try
        {
            var ru = new RepositorioUsuario();
            var usuario = ru.GetUsuarioByEmail(email);

            if (usuario == null || usuario.Estado == false)
            {
                TempData["error"] = "Correo electrónico no registrado o cuenta inactiva.";
                return View();
            }

            // Verifico la contraseña usando el salt almacenado
            var hashedPassword = HashPassword(clave, usuario.Salt);

            if (usuario.Clave != hashedPassword)
            {
                TempData["error"] = "Contraseña incorrecta.";
                return View();
            }

            // Autenticación exitosa
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, usuario.Email),
        new Claim(ClaimTypes.Name, usuario.Nombre),
        new Claim(ClaimTypes.Role, usuario.Rol)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");

        }
        catch (Exception ex)
        {
            if (email == null && clave == null)
            {
                TempData["error"] = "Debe ingresar un usuario y contraseña.";
                return View();
            }
            else if (email == null)
            {
                TempData["error"] = "Debe ingresar un usuario.";
                return View();
            }
            else
            {
                TempData["error"] = "Debe ingresar una contraseña.";
                return View();
            }
        }

    }

    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Home");
    }


    private static string HashPassword(string password, string salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 1000,
            numBytesRequested: 256 / 8));
    }


    private static string GenerateSalt(int size = 32)
    {
        var rng = new RNGCryptoServiceProvider();
        var saltBytes = new byte[size];
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }
    [Authorize]
    public IActionResult Perfil()
    {
        // Obtengo el email del usuario autenticado desde los claims
        var email = User.FindFirst(ClaimTypes.Email)?.Value;



        // Buscar el usuario por su email en la base de datos
        var ru = new RepositorioUsuario();
        var usuario = ru.GetUsuarioByEmail(email);



        // Paso el usuario a la vista de perfil
        return View(usuario);
    }

   
 [Authorize]
public IActionResult Editar(int id)
{
    var ru = new RepositorioUsuario();
    var usuarioExistente = ru.GetUsuarioById(id);

    // Obtener el usuario autenticado
    var email = User.FindFirst(ClaimTypes.Email)?.Value;
    var usuarioAutenticado = ru.GetUsuarioByEmail(email);

    // Si el usuario autenticado no es administrador y no es el mismo usuario, denegar acceso
    if (usuarioAutenticado == null || (usuarioAutenticado.Rol != "Administrador" && usuarioAutenticado.Id != id))
    {
        TempData["error"] = "No tienes permiso para editar este perfil.";
        return RedirectToAction("Perfil");
    }

    // Asegurar que el rol no sea modificado
    usuarioExistente.Rol = usuarioAutenticado.Rol;

    return View(usuarioExistente);
}


[Authorize]
[HttpPost]
public IActionResult Editar(Usuario usuario, bool eliminarAvatar)
{
    try
    {
        var ru = new RepositorioUsuario();
        var usuarioAutenticado = ru.GetUsuarioByEmail(User.FindFirst(ClaimTypes.Email)?.Value);

        // Verificar si el usuario autenticado puede modificar este perfil
        if (usuarioAutenticado == null || (usuarioAutenticado.Rol != "Administrador" && usuarioAutenticado.Id != usuario.Id))
        {
            TempData["error"] = "No tienes permiso para modificar este perfil.";
            return RedirectToAction("Perfil");
        }

        // Obtener los datos del usuario actual de la base de datos
        var usuarioExistente = ru.GetUsuarioById(usuario.Id);

        // Manejar el avatar
        if (usuario.AvatarFile != null && usuario.AvatarFile.Length > 0)
        {
            var avatarsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");

            if (!Directory.Exists(avatarsPath))
            {
                Directory.CreateDirectory(avatarsPath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(usuario.AvatarFile.FileName);
            var filePath = Path.Combine(avatarsPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                usuario.AvatarFile.CopyTo(stream);
            }

            // Actualizar la URL del avatar
            usuario.AvatarUrl = "/avatars/" + uniqueFileName;
        }
        else if (eliminarAvatar && !string.IsNullOrEmpty(usuarioExistente.AvatarUrl))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuarioExistente.AvatarUrl.TrimStart('/'));

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            usuario.AvatarUrl = ""; // Dejar al usuario sin avatar
        }
        else
        {
            usuario.AvatarUrl = usuarioExistente.AvatarUrl; // Mantener el avatar existente si no se elimina ni cambia
        }

        // Si se proporciona una nueva contraseña, hashearla
        if (!string.IsNullOrEmpty(usuario.Clave))
        {
            usuario.Salt = GenerateSalt();
            usuario.Clave = HashPassword(usuario.Clave, usuario.Salt);
        }
        else
        {
            usuario.Clave = usuarioExistente.Clave; // Mantener la contraseña actual
            usuario.Salt = usuarioExistente.Salt;   // Mantener el salt actual
        }

        // El rol no debe cambiar, mantener el rol original
        usuario.Rol = usuarioExistente.Rol;

        // Actualizar el usuario
        ru.EditarUsuario(usuario);

        TempData["mensaje"] = "Perfil actualizado con éxito!";
        return RedirectToAction("Perfil");
    }
    catch (MySqlException ex) when (ex.Number == 1062)
    {
        TempData["error"] = "Error: El mail elegido ya está en uso, cambie el mail.";
        return View(usuario);
    }
    catch (Exception ex)
    {
        TempData["error"] = "Error al modificar el perfil: " + ex.Message;
        return View(usuario);
    }
}
}

internal class RNGCryptoServiceProvider
{
    public RNGCryptoServiceProvider()
    {
    }

    internal void GetBytes(byte[] saltBytes)
    {
        throw new NotImplementedException();
    }
}