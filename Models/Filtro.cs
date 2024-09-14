using System.ComponentModel.DataAnnotations;

public class Filtro
{
    [Required(ErrorMessage = "Debe seleccionar el tipo de uso.")]
    public string? Uso { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el tipo de inmueble.")]
    public int? Tipo { get; set; }

    [Required(ErrorMessage = "Debe ingresar la cantidad de ambientes.")]
    [Range(1, 50, ErrorMessage = "La cantidad de ambientes debe estar entre 1 y 50.")]
    public int Ambientes { get; set; }

    [Required(ErrorMessage = "Debe ingresar el precio mínimo.")]
    [Range(0, double.MaxValue, ErrorMessage = "El precio mínimo debe ser un valor positivo.")]
    public decimal? PrecioMin { get; set; }

    [Required(ErrorMessage = "Debe ingresar el precio máximo.")]
    [Range(0, double.MaxValue, ErrorMessage = "El precio máximo debe ser un valor positivo.")]
    public decimal? PrecioMax { get; set; }

    [Required(ErrorMessage = "Debe seleccionar la fecha de inicio.")]
    public DateTime? FechaInicio { get; set; }

    [Required(ErrorMessage = "Debe seleccionar la fecha de finalización.")]
    public DateTime? FechaFin { get; set; }
}
