using Microsoft.AspNetCore.Mvc;

[Route("api/[Controller]")]
[ApiController]
public class TestController: Controller{
     private readonly DataContext Contexto;
     public TestController(DataContext dataContext){
        this.Contexto = dataContext;
     }

     //Get: api/<controller>
     [HttpGet]
     public async Task<IActionResult> Get()
		{
			try
			{
				//throw new Exception("Ocurrió algo");
				return Ok(new
				{
					Mensaje = "Éxito",
					Error = 0,
					Resultado = new
					{
						Clave = "Key",
						Valor = new Random().Next(0, 10000)
					},
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new { Mensaje = ex.Message, Error = 1 });
			}
		}

    // GET api/<controller>/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			return Ok(Contexto.Propietario.Find(id));
		}
}