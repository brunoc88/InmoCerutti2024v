using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using K4os.Hash.xxHash;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Iana;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class PropietarioController : ControllerBase
{
    private readonly DataContext contexto;
    private readonly IConfiguration config;
    private readonly ILogger<PropietarioController> logger;

    public PropietarioController(DataContext contexto, IConfiguration config, ILogger<PropietarioController> logger) // Agrega logger al constructor
    {
        this.contexto = contexto;
        this.config = config;
        this.logger = logger; // Asigna el logger
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginView loginView)
    {
        try
        {
            // Hashear la contraseña ingresada
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginView.Clave,
                salt: Encoding.ASCII.GetBytes(config["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));

            // Buscar al propietario por email
            var propietario = await contexto.Propietario.FirstOrDefaultAsync(x => x.Email == loginView.Email);

            // Verificar si el propietario existe y si la contraseña es correcta
            if (propietario == null || propietario.clave != hashed)
            {
                return BadRequest("Nombre de usuario o clave incorrecta");
            }
            string mail = propietario.Email;
            string nombre = propietario.Nombre + " " + propietario.Apellido;
            logger.LogInformation("Email del usuario: {Email}", mail); //
                                                                       // Crear los claims para el token JWT
            var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, mail), // Cambiar de ClaimTypes.Email a ClaimTypes.Name
    new Claim("FullName", nombre),
    new Claim(ClaimTypes.Role, "Propietario"),
};


            // Generar el token JWT
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: config["TokenAuthentication:Issuer"],
                audience: config["TokenAuthentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60), // Token válido por 60 minutos
                signingCredentials: credenciales
            );

            // Devolver el token
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    //crear Propietario
    // POST api/Propietario
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Propietario entidad)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // Si no se envía el valor de 'estado', se asigna el valor por defecto.
                entidad.estado = entidad.estado == default ? true : entidad.estado;
                // Aplicar el hash a la clave antes de guardar
                entidad.clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: entidad.clave,
                    salt: Encoding.ASCII.GetBytes(config["Salt"]),  // Usa el salt configurado en tu app
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,  // Iteraciones
                    numBytesRequested: 256 / 8));  // Tamaño del hash

                // Guardar el propietario con la clave hasheada
                await contexto.Propietario.AddAsync(entidad);
                await contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = entidad.id_propietario }, entidad);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    private object Get()
    {
        throw new NotImplementedException();
    }


    // GET api/Propietario/perfil
    [HttpGet("perfil")]
    [Authorize]//es el token para que no de 401
    public IActionResult perfil()
    {
        logger.LogInformation("llamaste a perfil!"); //

        try
        {
            // Obtener el email del usuario a partir del token JWT
            var email = User.Identity?.Name;

            // Registrar el valor del email en la consola
            logger.LogInformation("Email del usuario: {Email}", email); //

            if (email == null)
            {
                return BadRequest("No se pudo obtener el usuario desde el token.");
            }

            // Buscar al propietario por email
            var propietario = contexto.Propietario.SingleOrDefault(p => p.Email == email);

            if (propietario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Retornar la información del propietario
            return Ok(new
            {
                propietario.id_propietario,
                propietario.Nombre,
                propietario.Apellido,
                propietario.Email,
                propietario.Dni,
                propietario.Telefono,
                propietario.clave
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/Propietario
    [HttpPut]
    [Authorize]//es el token para que no de 401
    public async Task<IActionResult> Put([FromBody] Propietario entidad)
    {
        try
        {
            if (ModelState.IsValid)
            {

                Propietario original = await contexto.Propietario.FindAsync(entidad.id_propietario);
                if (original == null)
                {
                    return NotFound("Propietario no encontrado");
                }

                // Desvincula la entidad original para evitar el conflicto de rastreo
                contexto.Entry(original).State = EntityState.Detached;

                // Aquí realiza la lógica de actualización de la clave y otros campos
                if (String.IsNullOrEmpty(entidad.clave))
                {
                    entidad.clave = original.clave;
                }
                else
                {
                    entidad.clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: entidad.clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                }

                // Realiza la actualización y guarda los cambios
                contexto.Propietario.Update(entidad);
                await contexto.SaveChangesAsync();
                return Ok(entidad);

            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public class LoginView
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Clave { get; set; }
    }




}
