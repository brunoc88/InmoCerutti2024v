using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[Controller]")]
[ApiController]
public class InmuebleController: Controller{
    private readonly DataContext Contexto;
     public InmuebleController(DataContext dataContext){
        this.Contexto = dataContext;
     }

//inmuebles del propietario
[HttpGet("{id}")]
     public async Task<IActionResult> get(int id)
{
    try
    {
        var misInmuebles = await Contexto.inmueble
            .Where(x => x.id_propietario == id)
            .Select(x => new {x.Direccion,
            x.Ambientes,
            x.estado})  // Aquí seleccionamos la propiedades 'direccion','ambientes','estado'
            .ToListAsync();  // Usamos el método asincrónico

        return Ok(misInmuebles);  // Retornamos el resultado con un 200 OK
    }
    catch (Exception e)
    {
        return StatusCode(500, e.Message);  // En caso de error, retornamos un 500 con el mensaje de error
    }
}

}