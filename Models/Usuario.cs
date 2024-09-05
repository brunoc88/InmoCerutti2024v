using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Clave { get; set; }
    public string AvatarUrl { get; set; }
    public IFormFile AvatarFile { get; set; }
    public string Rol { get; set; }
    public bool Estado {get;set;}= true; // seteo en 1 de entrada al crear

     public string Salt { get; set; }
 

}

