using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace Inmobiliaria_Cerutti.InquilinoController;

public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;

    public InquilinoController(ILogger<InquilinoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioInquilino ri = new RepositorioInquilino();
        var lista = ri.GetInquilinos();
        return View(lista);
    }
[HttpGet]
    public IActionResult Editar(int id){//devuelve el formulario
        if(id>0){
            RepositorioInquilino ri = new RepositorioInquilino();
            var inquilino = ri.GetInquilino(id);
            return View(inquilino);
        }
        return View();
    }
[HttpPost]
[ValidateAntiForgeryToken] //medida de seguridad para que no me mande datos desde una misma direccion
    public IActionResult Guardar(Inquilino inquilino){
        RepositorioInquilino ri = new RepositorioInquilino();
        if(inquilino.id_inquilino>0){
            ri.ModificarInquilino(inquilino);
            TempData["mensaje"] = "Inquilino modificado con exito!";
            return RedirectToAction("Index");
        }else{
            ri.AltaInquilino(inquilino);
            TempData["mensaje"] = "Inquilino agregado con exito!";
            return RedirectToAction(nameof(Index));
        }
        
    }

    public IActionResult Eliminar(int id){
        RepositorioInquilino ri = new RepositorioInquilino();
        ri.EliminarInquilino(id);
        return RedirectToAction(nameof(Index));
    }

    
}