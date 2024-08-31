using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Filtro
{
    [Required(ErrorMessage = "Debe Seleccionar el tipo de uso.")]
    public string? Uso { get; set; }
    [Required(ErrorMessage = "Debe Seleccionar el tipo de inmueble.")]
    public string? Tipo { get; set; }
    [Required(ErrorMessage = "Debe ingresar la cantidad de ambientes.")]
    public int Ambientes {get;set;}
    [Required(ErrorMessage = "Debe ingresar precio minimo.")]
    public decimal? PrecioMin { get; set; }
    [Required(ErrorMessage = "Debe ingresar precio maximo.")]
    public decimal? PrecioMax { get; set; }
    [Required(ErrorMessage = "Debe Seleccionar fecha de inicio.")]
    public DateTime? FechaInicio { get; set; }
    [Required(ErrorMessage = "Debe Seleccionar fecha de inicio.")]
    public DateTime? FechaFin { get; set; }
}
