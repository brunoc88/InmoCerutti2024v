using System.ComponentModel.DataAnnotations;

public class TipoInmueble{
    public int id_tipoInmueble {get;set;}
    [Required(ErrorMessage = "Ingrese un tipo de inmueble!")]
    public string? tipoNombre {get;set;}
    public bool Estado{get;set;}
}