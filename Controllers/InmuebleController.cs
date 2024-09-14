using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_Cerutti.Models;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;

namespace Inmobiliaria_Cerutti.InmuebleController;

[Authorize]
public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;

    public InmuebleController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
    }



    public IActionResult Index()
    {
        var rinmuble = new RepositorioInmueble();
        var lista = rinmuble.GetInmuebles();
        return View(lista);
    }

    public IActionResult Crear()//formulario de alta
    {
        try
        {
            var rp = new RepositorioPropietario();
            ViewBag.Propietarios = rp.GetPropietarios();//obtengo los propietario y se los paso a la vista
            
            var rt = new RepositorioTipoInmueble();
            ViewBag.TipoInmuebles = rt.GetTipoInmuebles();
            return View();
        }
        catch (Exception ex)
        {
            // Devuelve una vista de error genérica
            ViewBag.ErrorMessage = ex.Message;
            return View("Error");
        }
    }

    [HttpPost]
    public IActionResult Guardar(Inmueble inmueble)
{
    try
    {
        var ri = new RepositorioInmueble();
        ri.AltaInmueble(inmueble);
        TempData["mensaje"] = "Inmueble guardado con éxito!";
        return RedirectToAction(nameof(Index));
    }
    catch (MySqlException ex) when (ex.Number == 1062)
    {
        // Error de duplicado
        TempData["error"] = "Error: Ya existe un Inmueble en esa dirección.";
        return RedirectToAction(nameof(Crear));
    }
    catch (Exception ex)
    {
        // Otros errores
        TempData["error"] = "Error: Debe seleccionar tipo de inmueble.";
        return RedirectToAction(nameof(Crear));
    }
}


    [Authorize(Roles = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        try
        {
            var ri = new RepositorioInmueble();
            ri.eliminarPropiedad(id);
            TempData["mensaje"] = "Inmueble Eliminado con exito!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["error"] = "Error: al eliminar el inmueble.";
            return RedirectToAction(nameof(Index));
        }


    }

    // GET: Inmueble/
    public ActionResult Editar(int id)
    {
        var ri = new RepositorioInmueble();
        var inmueble = ri.GetInmueble(id);
        var rp = new RepositorioPropietario();
        ViewBag.Propietarios = rp.GetPropietarios();
        var rt = new RepositorioTipoInmueble();
        ViewBag.TipoInmuebles = rt.GetTipoInmuebles();
        return View(inmueble);
    }
    // POST: Inmueble/Edit/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(int id, Inmueble inmueble)
    {
        try
        {
            var ri = new RepositorioInmueble();
            var rp = new RepositorioPropietario();
            
            inmueble.id_inmueble = id;
            ri.EditarInmueble(inmueble);
            TempData["Mensaje"] = "Inmueble editado correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            var rp = new RepositorioPropietario();
            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.Error = ex.Message;
            TempData["Error"] = "Error al editar el Inmueble";
            return View(inmueble);
        }
    }
}

