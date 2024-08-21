
using Inmobiliaria_Cerutti.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_Cerutti.PropietarioController;

public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;

    public PropietarioController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
    }

   public IActionResult Index()
{
    RepositorioPropietario rp = new RepositorioPropietario();
    var lista = rp.GetPropietarios(); // Aquí asegúrate de que GetPropietarios esté siendo llamado
    return View(lista);
}

[HttpGet]
    public IActionResult Editar(int id){//devuelve el formulario
        if(id>0){
            RepositorioPropietario rp = new RepositorioPropietario();
            var propietario = rp.GetPropietario(id);
            return View(propietario);
        }
        return View();
    }
[HttpPost]
    public IActionResult Guardar(Propietario propietario){
       RepositorioPropietario rp = new RepositorioPropietario();
        if(propietario.id_propietario>0){
            rp.ModificarPropietario(propietario);
            return RedirectToAction("Index");
        }else{
            rp.AltaPropietario(propietario);
            return RedirectToAction(nameof(Index));
        }
        
    }

    public IActionResult Eliminar(int id){
        RepositorioPropietario rp = new RepositorioPropietario();
        rp.EliminarPropietario(id);
        return RedirectToAction(nameof(Index));
    }

 
    
}