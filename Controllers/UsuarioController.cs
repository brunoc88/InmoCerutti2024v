using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

public class UsuarioController : Controller
{


    public UsuarioController()
    {

    }

    public IActionResult Index()
    {
        var ru = new RepositorioUsuario();
        var usuarios = ru.GetUsuarios();
        return View(usuarios);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }

    private static string GenerateSalt(int size = 32)
    {
        var rng = new RNGCryptoServiceProvider();
        var saltBytes = new byte[size];
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
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



    [HttpPost]
    public IActionResult Crear(Usuario usuario)
    {
        try
        {
            if (usuario.AvatarFile != null && usuario.AvatarFile.Length > 0)
            {
                // Ruta de la carpeta de avatares
                var avatarsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");

                // Creo la carpeta avatars si no existe
                if (!Directory.Exists(avatarsPath))
                {
                    Directory.CreateDirectory(avatarsPath);
                }

                // Genero un nombre único para el archivo del avatar
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(usuario.AvatarFile.FileName);

                // Ruta completa donde se guardará el archivo
                var filePath = Path.Combine(avatarsPath, uniqueFileName);

                // Guardo el archivo
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    usuario.AvatarFile.CopyTo(stream);
                }

                // Guardo la ruta del nuevo avatar en la base de datos
                usuario.AvatarUrl = "/avatars/" + uniqueFileName;
            }
            else
            {
                // Si no se selecciona un avatar, se asigna un valor por defecto o vacío
                usuario.AvatarUrl = "";
            }

            // Generar Salt y hashear la contraseña
            usuario.Salt = GenerateSalt();
            usuario.Clave = HashPassword(usuario.Clave, usuario.Salt);

            var ru = new RepositorioUsuario();
            ru.CrearUsuario(usuario);
            TempData["Mensaje"] = "Usuario Creado con exito!";
            return RedirectToAction("Index");

        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            TempData["Error"] = "Error: El usuario ya esta registrado! ";
            return View();
        }

    }


    public IActionResult Login(){
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(string email, string clave)
    {
        try{
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
        
        }catch(Exception ex){
            if(email == null && clave == null){
                TempData["error"] = "Debe ingresar un usuario y contraseña.";
                return View();
            }else if(email == null){
                TempData["error"] = "Debe ingresar un usuario.";
                return View();
            }else{
                TempData["error"] = "Debe ingresar una contraseña.";
                return View();
            }
        }
        
    }

    public IActionResult eliminar(int id)
    {
        try
        {
            var ru = new RepositorioUsuario();
            ru.EliminarUsuario(id);
            TempData["Mensaje"] = "Usuario Eliminado con exito!";
            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException ex)
        {
            TempData["error"] = "Error al eliminar el usuario.";
            return RedirectToAction(nameof(Index));
        }

    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var ru = new RepositorioUsuario();
        var usuario = ru.GetUsuarioById(id);
        return View(usuario);
    }

    // Acción POST para actualizar el usuario admin

    [HttpPost]
   public IActionResult Editar(Usuario usuario, bool eliminarAvatar)
{
    try
    {
        var ru = new RepositorioUsuario();
        var usuarioExistente = ru.GetUsuarioById(usuario.Id);

        // Manejar el archivo del avatar si se sube uno nuevo
        if (usuario.AvatarFile != null && usuario.AvatarFile.Length > 0)
        {
            // Ruta de la carpeta de avatares
            var avatarsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");

            // Creo la carpeta avatars si no existe
            if (!Directory.Exists(avatarsPath))
            {
                Directory.CreateDirectory(avatarsPath);
            }

            // Genero un nombre único para el archivo del avatar
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(usuario.AvatarFile.FileName);

            // Ruta completa donde se guardará el archivo
            var filePath = Path.Combine(avatarsPath, uniqueFileName);

            // Guardo el archivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                usuario.AvatarFile.CopyTo(stream);
            }

            // Guardo la ruta del nuevo avatar en la base de datos
            usuario.AvatarUrl = "/avatars/" + uniqueFileName;
        }
        else if (eliminarAvatar && !string.IsNullOrEmpty(usuarioExistente.AvatarUrl))
        {
            // Si se debe eliminar el avatar existente
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuarioExistente.AvatarUrl.TrimStart('/'));

            // Eliminar el archivo del servidor
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Limpiar la URL del avatar en la base de datos
            usuario.AvatarUrl = "";
        }
        else
        {
            // Mantener el avatar existente si no se elimina ni cambia
            usuario.AvatarUrl = usuarioExistente.AvatarUrl;
        }

        // Generar Salt y hashear la contraseña si se proporciona una nueva
        if (!string.IsNullOrEmpty(usuario.Clave))
        {
            usuario.Salt = GenerateSalt();
            usuario.Clave = HashPassword(usuario.Clave, usuario.Salt);
        }
        else
        {
            // Mantener la contraseña y el salt existente si no se proporciona una nueva
            usuario.Clave = usuarioExistente.Clave;
            usuario.Salt = usuarioExistente.Salt;
        }

        ru.EditarUsuario(usuario);
        TempData["mensaje"] = "Usuario modificado con éxito!";

        return RedirectToAction("Index");
    }
    catch (MySqlException ex) when (ex.Number == 1062)
    {
        TempData["error"] = "Error: El mail elegido ya está en uso, cambie el mail.";
        return View(usuario);
    }
    catch (Exception ex)
    {
        TempData["error"] = "Error al modificar el usuario: " + ex.Message;
        return View(usuario);
    }
}


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

    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Usuario");
    }

}


