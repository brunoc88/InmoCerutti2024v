using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_Cerutti.Models;
using System.Linq.Expressions;

namespace Inmobiliaria_Cerutti.InmuebleController;

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
            return View();
        }
        catch (Exception ex)
        {
            // Devuelve una vista de error genérica
            ViewBag.ErrorMessage = ex.Message;
            return View("Error");
        }
    }


    public IActionResult guardar(Inmueble inmueble)
    {
        try
        {
            var ri = new RepositorioInmueble();
            ri.AltaInmueble(inmueble);
            TempData["mensaje"] = "Inmueble agregado con exito!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["error"] = "Error: al inmueble ya está registrado.";
            return RedirectToAction(nameof(Crear));
        }


    }

    public IActionResult Eliminar(int id)
    {
        try
        {
            var ri = new RepositorioInmueble();
            ri.eliminarPropiedad(id);
            TempData["mensaje"] = "Inmueble Eliminado con exito!";
            return RedirectToAction(nameof(Index));
        }catch(Exception ex){
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
        return View(inmueble);
    }
    // POST: Inmueble/Edit/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(int id, Inmueble inmueble)
    {
        var ri = new RepositorioInmueble();
        try
        {
            inmueble.id_inmueble = id;
            ri.EditarInmueble(inmueble);
            TempData["Mensaje"] = "Datos guardados correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            var rp = new RepositorioPropietario();
            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.Error = ex.Message;
            ViewBag.StackTrate = ex.StackTrace;
            return View(inmueble);
        }
    }


}