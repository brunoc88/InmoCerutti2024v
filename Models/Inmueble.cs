using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum UsoInmueble
{
    Comercial,
    Residencial
}

public enum TipoInmueble
{
    Local,
    Deposito,
    Casa,
    Departamento,
    Otro
}

public class Inmueble
{
    public int id_inmueble { get; set; }

    [Required(ErrorMessage = "Debe ingresar una Direccion.")]
    public string Direccion { get; set; }

    [Required(ErrorMessage = "Debe Elegir el tipo de Uso.")]
    public UsoInmueble Uso { get; set; }

    [Required(ErrorMessage = "Debe Elegir el tipo de Inmueble.")]
    public TipoInmueble Tipo { get; set; }

    [Required(ErrorMessage = "Debe detallar la cantidad de ambientes.")]
    public int Ambientes { get; set; }
    [Required]
    [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90 grados.")]
    public decimal? Latitud { get; set; }

    [Required]
    [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180 grados.")]
    public decimal? Longitud { get; set; }

    [Required(ErrorMessage = "Debe Ingresar el precio.")]
    [DataType(DataType.Currency)]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "Debe Elegir el Propietario")] // Asegura que el propietario siempre esté asociado
    [Display(Name = "Dueño")]
    public int? id_propietario { get; set; }

    [ForeignKey(nameof(id_propietario))]
    public Propietario? duenio { get; set; }
}


