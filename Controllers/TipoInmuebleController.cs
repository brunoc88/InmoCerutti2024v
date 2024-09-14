using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

[Authorize]
public class TipoInmuebleController : Controller
{
    private readonly ILogger<TipoInmuebleController> _logger;

    public TipoInmuebleController(ILogger<TipoInmuebleController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var rtipo = new RepositorioTipoInmueble();
        var tipos = rtipo.GetTipoInmuebles();
        return View(tipos);
    }

    public IActionResult Alta(int id)
    {
        if(id == 0){
            return View();
        }else{
            var rtipo = new RepositorioTipoInmueble();
            var tipo = rtipo.GetTipoInmueble(id);
            return View(tipo);
        }
        
    }

    [HttpPost]
    public IActionResult Alta(TipoInmueble tipoInmueble)
    {
        if(tipoInmueble.id_tipoInmueble == 0){

        try
        {
            var rtipo = new RepositorioTipoInmueble();
            rtipo.Alta(tipoInmueble);
            TempData["Mensaje"] = "Tipo de inmueble Guardado!";
            return RedirectToAction("Index");
        }
        catch (MySqlException ex)
        {
            TempData["Error"] = "Ya existe!";
            return View();
        }
        }else{
            try{
                var rtipo = new RepositorioTipoInmueble();
                rtipo.ModificarTipoInmueble(tipoInmueble);
                TempData["Mensaje"] = "Tipo de inmueble Modificado!";
                return RedirectToAction("Index");
            }catch(MySqlException){
                TempData["Error"] = "Ya existe!";
                return View();
            }
        }
    }

    [Authorize(Roles ="Administrador")]
    public IActionResult Eliminar(int id){
        try{
            var rtipo = new RepositorioTipoInmueble();
            rtipo.Eliminar(id);
            TempData["Mensaje"] = "Tipo de inmueble Eliminado!";
            return RedirectToAction("Index");

        }catch(MySqlException ex){
            TempData["Error"] = "Se produjo un Error al Eliminar!";
            return RedirectToAction("Index");
        }
    }
}