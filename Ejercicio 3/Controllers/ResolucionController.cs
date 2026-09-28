using System.Timers;
using Microsoft.AspNetCore.Mvc;
namespace Ejercicio3.Controllers;


[ApiController]
[Route("[controller]")]
public class ResolucionController : ControllerBase
{

    public static List<Resolucion> listaResolucion = new List<Resolucion>();
    private readonly ILogger<ResolucionController> _logger;

    public ResolucionController(ILogger<ResolucionController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult create([FromBody]Resolucion nuevoResolucion, int idDesafio, int idParticipante)
    {
        try
        {
            if(nuevoResolucion.Id <= 0)
            {
                return BadRequest("Ingrese un id valido");
            }


            var ResolucionExistente = listaResolucion.FirstOrDefault(p=> p.Id == nuevoResolucion.Id);

            if(nuevoResolucion.Id == ResolucionExistente.Id)
            {
                return BadRequest("El id de este Resolucion ya existe");
            }


            var desafioBuscado = DesafioController.listaDesafio.FirstOrDefault(d=> d.Id == idDesafio);
            
            if (desafioBuscado is null)
            {
                return NotFound("No se encontro un desafio con ese id");
            }

            if (nuevoResolucion.PuntajeObtenido > desafioBuscado.PuntajeMaximo)
            {
                return BadRequest("El puntaje obtenido es mayor al puntaje maximo del desafio");
            }

            if (desafioBuscado.Activo == false)
            {
                return BadRequest("Este desafio no esta activo");
            }


            var participanteBuscado = ParticipanteController.listaparticipante.FirstOrDefault(p=> p.Id == idParticipante);

            if(participanteBuscado is null)
            {
                return NotFound("Participante no encontrado");
            }

            if (participanteBuscado.Id == idParticipante)
            {
                return BadRequest("Este participante ya completo este desafio");
            }


            listaResolucion.Add(nuevoResolucion);
            return Ok("Resolucion registrado con exito");
        }
        catch(Exception ex)
        {
            return StatusCode (500, $"Error interno del servidor: {ex.Message}");
        }
    }



    
}
