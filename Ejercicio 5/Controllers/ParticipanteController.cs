using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Ejercicio5.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipanteController : ControllerBase
{

    public static List<Participante> listaParticipantes = new List<Participante>();
    private readonly ILogger<ParticipanteController> _logger;

    public ParticipanteController(ILogger<ParticipanteController> logger)
    {
        _logger = logger;
    }


    [HttpPost]

    public IActionResult create([FromBody] Participante nuevaParticipante)
    {
        try
        {
            if(nuevaParticipante.Id <=0)
            {
                return BadRequest("Ingrese un id válido");
            }
            
            var ParticipanteIdExistente = listaParticipantes.FirstOrDefault(a=> a.Id == nuevaParticipante.Id);

            if (ParticipanteIdExistente is not null)
            {
                return BadRequest("El id de esta Participante ya existe");
            }

            var participanteMailExistente = listaParticipantes.FirstOrDefault(p=> p.Email == nuevaParticipante.Email);

            if (participanteMailExistente is not null)
            {
                return BadRequest("El Email de este participante ya esta registrado");
            }


            return Ok("Participante registrado con exito");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet("actividades/{idParticipante}")]
    public IActionResult getActividades(int idParticipante)
    {
        try
        {
            var reservas = ReservaActividadController.listaReservaActividades.Where(r => r.IdParticipante == idParticipante);

            var actividades = ActividadController.listaActividades.Where(a => reservas.Any(r => r.IdActividad == a.Id));

            if (!actividades.Any())
            {
                return NotFound("Este participante no esta inscripto en ninguna actividad");
            }

            return Ok(actividades);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }




}
