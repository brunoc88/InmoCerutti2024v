
using System.Globalization;
using Inmobiliaria_Cerutti.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
       try{
        if(propietario.id_propietario>0){
            rp.ModificarPropietario(propietario);
            TempData["mensaje"] = "Propietario modificado con exito!";
            return RedirectToAction("Index");
        }else{
            rp.AltaPropietario(propietario);
            TempData["mensaje"] = "Propietario agregado con exito!";
            return RedirectToAction(nameof(Index));
        }}
        catch (MySqlException ex) when(ex.Number == 1062){
            string campoDuplicado = "";

    if (ex.Message.Contains(nameof(Propietario.Dni)))
    {
        campoDuplicado = $"DNI '{propietario.Dni}'";
    }else if(ex.Message.Contains(nameof(Propietario.Telefono))){
        campoDuplicado = $"Telefono '{propietario.Telefono}'";
    }else{
        campoDuplicado = $"Email'{propietario.Email}'";
    }
    

    TempData["error"] = $"Error: El {campoDuplicado} ya está registrado.";
    return RedirectToAction(nameof(Index));
        }
        
    }

    public IActionResult Eliminar(int id){
        RepositorioPropietario rp = new RepositorioPropietario();
        rp.EliminarPropietario(id);
        TempData["mensaje"] = "Propietario eliminado con exito!";
        return RedirectToAction(nameof(Index));
    }

 
    
}