using System.ComponentModel;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace SistemasAlumnos.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ObjetoPerdidoController : ControllerBase
{

    List<ObjetoPerdido> listObjetoPerdido = new List<ObjetoPerdido>()
    {
        new ObjetoPerdido{Id=1, Descripcion = "Telefono", Categoria = "Dispositivos",LugarEncontrado = "Baño",  FechaEncontrada = new DateTime(2020, 12, 2) ,Reclamado = true, NombrePersonaQueRetiro ="EstebanQuito"  },
        new ObjetoPerdido{Id=2, Descripcion = "Llaves", Categoria = "Accesorios",LugarEncontrado = "Aula 1",  FechaEncontrada = new DateTime(2003, 2, 2) ,Reclamado = false, NombrePersonaQueRetiro =""  },
    };

    private readonly ILogger<ObjetoPerdidoController> _logger;

    public ObjetoPerdidoController(ILogger<ObjetoPerdidoController> logger)
    {
        _logger = logger;
    }


    [HttpPost]

    public IActionResult Create([FromBody]ObjetoPerdido nuevoObjetoPerdido)
    {
        try
        {

            if(string.IsNullOrWhiteSpace(nuevoObjetoPerdido.Descripcion))
            {
                return BadRequest("Ingrese una descripcion");
            }

            if (nuevoObjetoPerdido.FechaEncontrada > DateTime.Now   )
            {
                return BadRequest("Ingrese una fecha correcta");
            }

            if (nuevoObjetoPerdido.Reclamado == true)
            {
                if (string.IsNullOrWhiteSpace(nuevoObjetoPerdido.NombrePersonaQueRetiro))
                {
                    return BadRequest("Si el objeto fue reclamado, ingrese la persona que lo retiro");
                }
            }

            listObjetoPerdido.Add(nuevoObjetoPerdido);        
            return Ok();
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }




    [HttpGet]

    public IActionResult GetAll()

    {
        try
        {
            return Ok(listObjetoPerdido);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }




    [HttpGet("{id}")]

    public IActionResult GetById(int id)
    {
        try
        {

            var objetoBuscado = listObjetoPerdido.FirstOrDefault( o => o.Id == id);

            if(objetoBuscado != null)
            {
                return Ok(objetoBuscado);
            }

            return NotFound("Objeto no encontrado");

        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }




    [HttpPut("{id}")]

    public IActionResult Update(int id, [FromBody] ObjetoPerdido objetoPerdidoActualizado)
    {
        try
        {
            var objetoBuscado = listObjetoPerdido.FirstOrDefault( o => o.Id == id);

            if (objetoBuscado is null)
            {
                return NotFound("Objeto no encontrado");
            }

            if (objetoBuscado.FechaEncontrada > DateTime.Now   )
            {
                return BadRequest("Ingrese una fecha correcta");
            }
           
            if(string.IsNullOrWhiteSpace(objetoBuscado.Descripcion))
            {
                return BadRequest("Ingrese una descripcion");
            }

            if (objetoBuscado.Reclamado == true)
            {
                if (string.IsNullOrWhiteSpace(objetoBuscado.NombrePersonaQueRetiro))
                {
                    return BadRequest("Si el objeto fue reclamado, ingrese la persona que lo retiro");
                }
            }
            
            objetoBuscado.Descripcion = objetoPerdidoActualizado.Descripcion;
            objetoBuscado.Categoria = objetoPerdidoActualizado.Categoria;
            objetoBuscado.LugarEncontrado = objetoPerdidoActualizado.LugarEncontrado;
            objetoBuscado.FechaEncontrada = objetoPerdidoActualizado.FechaEncontrada;
            objetoBuscado.Reclamado = objetoPerdidoActualizado.Reclamado;
            objetoBuscado.NombrePersonaQueRetiro = objetoPerdidoActualizado.NombrePersonaQueRetiro;

             return Ok();  
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]

    public IActionResult delete (int id)
    {
        try
        {
            var objetoBuscado = listObjetoPerdido.FirstOrDefault(o=> o.Id == id);

            if (objetoBuscado is null)
            {
                return NotFound("Objeto no encontrado");
            }

            listObjetoPerdido.Remove(objetoBuscado);
            return Ok("Objeto eliminado");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }

       
    }




    [HttpGet("{Descripcion}")]    

    public IActionResult GetByDescription(string Descripcion)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                return NotFound("Descripcion vacia");
            }

            var objetoBuscado = listObjetoPerdido.FirstOrDefault(o=> o.Descripcion.ToLower() == Descripcion.ToLower());

            if (objetoBuscado is null)
            {
                return NotFound("Objeto no encontrado");
            }
            return Ok(objetoBuscado);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }



    [HttpGet("{Categoria}")]

    public IActionResult GetByCategory(string Categoria)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Categoria))
            {
                return NotFound("Categoria vacia");
            }

            var objetoBuscado = listObjetoPerdido.Where(o=> o.Categoria.ToLower() == Categoria.ToLower());

            if (objetoBuscado is null)
            {
                return NotFound("Objeto no encontrado");
            }

            return Ok(objetoBuscado);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }


    [HttpGet("objetos no reclamados")]
    

    public IActionResult GetObjectDontClaimed()
    {
        try
        {
            var objetoNoReclamado = listObjetoPerdido.Where(o=> o.Reclamado == false);

            if (objetoNoReclamado is null)
            {
                return NotFound("Objeto no encontrado");
            }
            return Ok(objetoNoReclamado);

        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }


    [HttpGet("Objetos encontrados despues de {fecha}")]

    public IActionResult GetByDate (DateTime fecha)
    {
        try
        {
            if (fecha > DateTime.Now   )
            {
                return BadRequest("Ingrese una fecha correcta");
            }

            var objetoBuscado = listObjetoPerdido.Where(o=> o.FechaEncontrada > fecha);
            
            if (objetoBuscado is null)
            {
                return NotFound("Objeto no encontrado");
            }

            return Ok(objetoBuscado);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }


    [HttpGet("Orden por antiguedad")]

    public IActionResult GetAntiguedad()
    {
        try
        { 
            var objetoBuscado = listObjetoPerdido.OrderBy(o=> o.FechaEncontrada);

            if (objetoBuscado is null)
            {
                return NotFound("Objeto no encontrado");
            }

            return Ok(objetoBuscado);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }

    [HttpPut("MarcarReclamado/{id}")]

    public IActionResult updateClaimed(int id, string nombrePersonaQueRetiro)
    {
        try
        {

            if (string.IsNullOrWhiteSpace(nombrePersonaQueRetiro)  )
            {
                return NotFound("Nombre de la persona que retiro esta vacio");
            }   

            var objetoBuscado = listObjetoPerdido.FirstOrDefault(o=> o.Id == id);

            if (objetoBuscado is null)
            {
                return NotFound("Objeto no encontrado");
            }
    

            objetoBuscado.Reclamado = true;
            objetoBuscado.NombrePersonaQueRetiro  = nombrePersonaQueRetiro;
            return Ok("Objeto actualizado");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error en el servidor: {ex.Message}");
        }
    }
}

