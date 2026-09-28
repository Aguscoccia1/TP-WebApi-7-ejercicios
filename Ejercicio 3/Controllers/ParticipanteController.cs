using System.Net.Http.Headers;
using System.Timers;
using Microsoft.AspNetCore.Mvc;
namespace Ejercicio3.Controllers;


[ApiController]
[Route("[controller]")]
public class ParticipanteController : ControllerBase
{

    public static List<Participante> listaparticipante = new List<Participante>();
    private readonly ILogger<ParticipanteController> _logger;

    public ParticipanteController(ILogger<ParticipanteController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult create([FromBody]Participante nuevoParticipante)
    {
        try
        {
            if(nuevoParticipante.Id <= 0)
            {
                return BadRequest("Ingrese un id valido");
            }

            var participanteExistente = listaparticipante.FirstOrDefault(p=> p.Id == nuevoParticipante.Id);
            if(nuevoParticipante.Id == participanteExistente.Id)
            {
                return BadRequest("El id de este participante ya existe");
            }

            if(string.IsNullOrWhiteSpace(nuevoParticipante.Email))
            {
                return BadRequest("Ingrese un Email");
            }

            var participanteBuscado = listaparticipante.FirstOrDefault (p=> p.Email == nuevoParticipante.Email);

            if(nuevoParticipante.Email == participanteBuscado.Email)
            {
                return BadRequest("El email de este participante ya esta registrado");
            }

            listaparticipante.Add(nuevoParticipante);
            return Ok("Participante registrado con exito");
        }
        catch(Exception ex)
        {
            return StatusCode (500, $"Error interno del servidor: {ex.Message}");
        }
    }



    [HttpGet("resoluciones/{idParticipante}")]

    public IActionResult getResoluciones (int idParticipante)
    {
        try
        {
            var participanteBuscado = ResolucionController.listaResolucion.Where(p=> p.ParticipanteId == idParticipante);

            if (participanteBuscado is null)
            {
                return NotFound("Este participante no realizo ninguna resolucion ");
            }

           return Ok(participanteBuscado);   
        }
        catch(Exception ex)
        {
            return StatusCode (500, $"Error interno del servidor: {ex.Message}");
        }
    }



    [HttpGet("{puntaje}")]

    public IActionResult getPuntaje(double puntaje)
    {
        try
        {

            var resoluciones = ResolucionController.listaResolucion.Where(r => r.PuntajeObtenido > puntaje);

            var participantes = listaparticipante.Where(p => resoluciones.Any(r => r.ParticipanteId == p.Id));
                    
            if (participantes is null)
            {
                return NotFound($"No se encontraron participantes con un puntaje mayor a: {puntaje}");
            }
            return Ok(participantes);
        } 
        catch(Exception ex)
        {
            return StatusCode (500, $"Error interno del servidor: {ex.Message}");
        }
    }





    [HttpGet("Mayorpuntaje")]

    public IActionResult getMayorPuntaje()
    {
        try
        {
           
            var ordenado = listaparticipante.OrderByDescending(p => ResolucionController.listaResolucion.Where(r => r.ParticipanteId == p.Id).Sum(r => r.PuntajeObtenido));

            if (ordenado is null)
            {
                return NotFound("No hay participantes registrados");
            }

            return Ok(ordenado);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet("OrdenarPorPuntaje")]
    public IActionResult getOrdenadoPorPuntaje()
    {
        try
        {
            var ordenado = listaparticipante.OrderByDescending(p => ResolucionController.listaResolucion.Where(r => r.ParticipanteId == p.Id).Sum(r => r.PuntajeObtenido));

            if (ordenado is null)
            {
                return NotFound("No hay participantes registrados");
            }

            return Ok(ordenado);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet("DesafiosNoResueltos/{idParticipante}")]
    public IActionResult getDesafiosNoResueltos(int idParticipante)
    {
        try
        {
            var participanteBuscado = listaparticipante.FirstOrDefault(p => p.Id == idParticipante);

            if (participanteBuscado is null)
            {
                return NotFound("Participante no encontrado");
            }

            var resolucionesDelParticipante = ResolucionController.listaResolucion.Where(r => r.ParticipanteId == idParticipante);

            var desafiosNoResueltos = DesafioController.listaDesafio.Where(d => !resolucionesDelParticipante.Any(r => r.DesafioId == d.Id));

            return Ok(desafiosNoResueltos);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

}
