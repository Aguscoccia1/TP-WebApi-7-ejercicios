using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Ejercicio5.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservaActividadController : ControllerBase
{

    public static List<ReservaActividad> listaReservaActividades = new List<ReservaActividad>();
    private readonly ILogger<ReservaActividadController> _logger;

    public ReservaActividadController(ILogger<ReservaActividadController> logger)
    {
        _logger = logger;
    }


    [HttpPost]

    public IActionResult create([FromBody] ReservaActividad nuevaReservaActividad)
    {
        try
        {
            if(nuevaReservaActividad.Id <=0)
            {
                return BadRequest("Ingrese un id válido");
            }
            
            var ReservaActividadExistente = listaReservaActividades.FirstOrDefault(a=> a.Id == nuevaReservaActividad.Id);

            if (ReservaActividadExistente is not null)
            {
                return BadRequest("El id de esta ReservaActividad ya existe");
            }

            
            var actividadExistente = ActividadController.listaActividades.FirstOrDefault(a=> a.Id == ReservaActividadExistente.IdActividad);
            
            if(actividadExistente is null)
            {
                return NotFound("No se encontro una actividad con ese id");
            }

            if (actividadExistente.Activo)
            {
                return BadRequest("Esta actividad no esta activa");
            }

            var ParticipanteExistente = ParticipanteController.listaParticipantes.FirstOrDefault(p=> p.Id == ReservaActividadExistente.IdParticipante);

            if (ParticipanteExistente is null)
            {
                return NotFound("No se encontro un participante con ese id");
            }

            var ParticipanteYaReservado = listaReservaActividades.FirstOrDefault(r=> r.IdParticipante == nuevaReservaActividad.IdParticipante && r.IdActividad == nuevaReservaActividad.IdActividad);

            if (ParticipanteYaReservado is not null)
            {
                return BadRequest("Participante ya registrado en esta actividad");
            }

            int cantidadReservas = listaReservaActividades.Count(r => r.IdActividad == nuevaReservaActividad.IdActividad);

            if (cantidadReservas > actividadExistente.Capacidad)
            {
                return BadRequest("Se alcanzo la capacidad maxima de participantes");
            }

            return Ok("Reserva de la Actividad registrado con exito");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]

    public IActionResult delete(int id)
    {
        try
        {
            var reservaBuscada = listaReservaActividades.FirstOrDefault(r=> r.Id == id);
            if (reservaBuscada is null)
            {
                return NotFound("Reserva no encontrada");
            }
            

            listaReservaActividades.Remove(reservaBuscada);
            return Ok("Reserva borrada");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }



    
}
