using Inmobiliaria_Cerutti.UsuarioController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Iana;
namespace Inmobiliaria_Cerutti.PagoController;

[Authorize]
public class PagoController : Controller{
    private readonly ILogger<PagoController> _logger;

    public PagoController(ILogger<PagoController> logger)
    {
        _logger = logger;
    }


    public IActionResult Index(){
        var rpago = new RepositorioPago();
        var pagos = rpago.GetPagos();
        return View(pagos);
    }
    public IActionResult buscar(){
        return View();
    }
   public IActionResult buscarDni(string dni)
{
    var rc = new RepositorioContrato();
    var contratos = rc.GetContratoPorDni(dni);

    if (contratos != null)
    {
        ViewBag.Contratos = contratos; // Pasamos los contratos a la vista usando ViewBag
        return View("Pagar");
    }
    else
    {
        TempData["Error"] = "Error: Dni no registrado";
        return View("buscar");
    }

}

    
public IActionResult Pagar(int id_contrato)
{
    ViewBag.IdContrato = id_contrato; // Asegúrate de pasar solo el ID
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
    catch (Exception ex)
    {
        TempData["error"] = $"Error al realizar el pago: {ex.Message}";
        return View(pago);
    }

}

[Authorize(Roles = "Administrador")]
public IActionResult Eliminar(int id){
    var rpago = new RepositorioPago();
    try{
        rpago.EliminarPago(id);
        TempData["mensaje"] = "Pago eliminado con éxito.";
        return View(nameof(Index));
    }catch(Exception ex){
        TempData["error"] = $"Error al eliminar el pago: {ex.Message}";
        return View(nameof(Index));
    }
    
}

   
}