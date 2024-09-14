using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Pago
{
    public int id_pago { get; set; }
    [Required(ErrorMessage = "Selecione una fecha!")]
    public DateTime FechaDePago { get; set; }
    [Required(ErrorMessage = "Ingrese motivo!")]
    public string? Motivo { get; set; }
    [Required(ErrorMessage = "Ingrese importe!")]
    public decimal Importe { get; set; }
    public int id_contrato { get; set; }
    [ForeignKey(nameof(id_contrato))]
    public Contrato? contrato { get; set; }
    public bool Estado { get; set; }
}

