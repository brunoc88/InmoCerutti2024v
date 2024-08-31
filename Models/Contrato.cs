using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Contrato
{
    public int id_contrato { get; set; }
    public Inquilino? inquilino { get; set; }
    public Inmueble? inmueble { get; set; }
    [Required(ErrorMessage = "Debe ingresar una Precio.")]
    public decimal Precio { get; set; }

    public int id_inmueble { get; set; }
    [ForeignKey(nameof(id_inmueble))]
    public int id_inquilino { get; set; }
    [ForeignKey(nameof(id_inquilino))]

    // Agregamos las fechas de inicio y finalización del contrato
    [Required(ErrorMessage = "Debe Agregar Fecha de Inicio.")]
    public DateTime FechaInicio { get; set; }
    [Required(ErrorMessage = "Debe Agregar Fecha de Finalizacion.")]
    public DateTime FechaFin { get; set; }
    public bool estado {get;set;}
}
