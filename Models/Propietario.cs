using System.ComponentModel.DataAnnotations;
public class Propietario
{
    [Key]
    public int id_propietario { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string? Nombre { get; set; }
    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string? Apellido { get; set; }
    [Required(ErrorMessage = "El DNI es obligatorio")]
    public string? Dni { get; set; }
    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo electrónico no válido")]

    public string? Email { get; set; }
    [Required(ErrorMessage = "El teléfono es obligatorio")]
    public string? Telefono { get; set; }
    public bool estado {get;set;}
    public string? clave {get;set;}//se adiere por el registro en la app movil

}