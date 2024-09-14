using Inmobiliaria_Cerutti.UsuarioController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Iana;
namespace Inmobiliaria_Cerutti.PagoController;

[Authorize]
public class PagoController : Controller
{
    private readonly ILogger<PagoController> _logger;

    public PagoController(ILogger<PagoController> logger)
    {
        _logger = logger;
    }


    public IActionResult Index()
    {
        var rpago = new RepositorioPago();
        var pagos = rpago.GetPagos();
        return View(pagos);
    }
    public IActionResult buscar()
    {
        return View();
    }
    public IActionResult buscarDni(string dni)
    {
        var rc = new RepositorioContrato();
        var contratos = rc.GetContratoPorDni(dni);

        if (contratos != null && contratos.Any()) // Verificamos si la lista tiene elementos
        {
            ViewBag.Contratos = contratos; // Pasamos los contratos a la vista usando ViewBag
            return View("Pagar");
        }else if(dni == null){
            TempData["Error"] = "Error: Ingrese un Dni!";
            return View("buscar");
        }
        else
        {
            TempData["Error"] = "Error: No hay contratos registrados";
            return View("buscar");
        }
        
    }

 public IActionResult buscarEmail(string email)
    {
        var rc = new RepositorioContrato();
        var contratos = rc.GetContratoPorEmail(email);

        if (contratos != null && contratos.Any()) // Verificamos si la lista tiene elementos
        {
            ViewBag.Contratos = contratos; // Pasamos los contratos a la vista usando ViewBag
            return View("Pagar");
        }else if(email == null){
            TempData["Error"] = "Error: Ingrese un Email!";
            return View("buscar");
        }
        else
        {
            TempData["Error"] = "Error: No hay contratos registrados";
            return View("buscar");
        }
    }


    public IActionResult Pagar(int id_contrato)
    {
        ViewBag.IdContrato = id_contrato;
        return View();
    }



    [HttpPost]
    public IActionResult Pagar(Pago pago)
    {

        var rpago = new RepositorioPago();
        try
        {
            rpago.Pagar(pago);
            TempData["mensaje"] = "Pago realizado con éxito.";
            return RedirectToAction("Index");
        }
        catch (MySqlException ex)
        {
            TempData["error"] = $"Error al realizar el pago: {ex.Message}";
            return View(pago);
        }

    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        var rpago = new RepositorioPago();
        try
        {
            rpago.EliminarPago(id);
            TempData["mensaje"] = "Pago eliminado con éxito.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["error"] = $"Error al eliminar el pago: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }

    }

public IActionResult Editar(int id)
{
    var rpago = new RepositorioPago();
    try
    {
        var pago = rpago.GetPago(id);
        if (pago == null)
        {
            TempData["error"] = "Pago no encontrado.";
            return RedirectToAction(nameof(Index));
        }
        return View(pago); // Pasa el modelo a la vista
    }
    catch (Exception ex)
    {
        TempData["error"] = $"Error al obtener el pago: {ex.Message}";
        return RedirectToAction(nameof(Index));
    }
}

[HttpPost]
public IActionResult Editar(Pago pago)
{
    try
    {
        var rpago = new RepositorioPago();
        rpago.EditarPago(pago);
        TempData["mensaje"] = "Pago Modificado!";
        return RedirectToAction(nameof(Index)); 
    }
    catch (Exception ex)
    {
        TempData["error"] = $"No se pudo editar el pago: {ex.Message}";
        return RedirectToAction(nameof(Index)); 
    }
}


}