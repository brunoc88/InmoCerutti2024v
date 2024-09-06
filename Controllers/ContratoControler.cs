using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Pkcs;
using MySql.Data.MySqlClient;
using Mysqlx;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria_Cerutti.ContratoController;

[Authorize]
public class ContratoController : Controller
{
    private readonly ILogger<ContratoController> _logger;


    public ContratoController(ILogger<ContratoController> logger)
    {
        _logger = logger;

    }

    public IActionResult Index()//contratos hechos y agregar
    {
        var rc = new RepositorioContrato();
        var contratos = rc.GetContratos();
        return View(contratos);
    }

    public IActionResult Buscar()
    {
        return View();
    }

    [HttpPost]
   public IActionResult Listado(Filtro filtro)
{
    var rc = new RepositorioContrato();
    var inmuebles = rc.GetInmueblesDisponibles(filtro);
    
    if (inmuebles != null && inmuebles.Count > 0)
    {
        return View(inmuebles);
    }
    else
    {
        TempData["error"] = "Error: No se encontraron inmublemes!.";
        return RedirectToAction(nameof(Buscar));
    }
}


    public IActionResult alta(int id)
    {
        var repoInmueble = new RepositorioInmueble();
        var inmueble = repoInmueble.GetInmueble(id);

        var repoInquilino = new RepositorioInquilino();
        var inquilinos = repoInquilino.GetInquilinos();

        ViewBag.Inmueble = inmueble;
        ViewBag.Inquilinos = inquilinos;

        var contrato = new Contrato
        {
            id_inmueble = inmueble.id_inmueble,
            
        };

        return View(contrato);
    }

    [HttpPost]
    public IActionResult alta(Contrato contrato){
        var rc = new RepositorioContrato();
        rc.CrearContrato(contrato);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Eliminar(int id){
        var rc = new RepositorioContrato();
        rc.EliminarContrato(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Editar(int id){
        var rc = new RepositorioContrato();
        var contrato = rc.GetContrato(id);
        return View(contrato);
    }


    [HttpPost] //arreglado
    public IActionResult Editar(Contrato contrato){
        var rc = new RepositorioContrato();
        rc.EditarContrato(contrato);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Detalles(int id){
        var rc = new RepositorioContrato();
        var contrato = rc.GetContrato(id);
        return View(contrato);
    }
}