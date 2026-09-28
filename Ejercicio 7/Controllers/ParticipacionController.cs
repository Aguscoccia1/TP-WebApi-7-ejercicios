using Microsoft.AspNetCore.Mvc;
namespace Ejercicio7.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipacionController : ControllerBase
{
    public static List<Participacion> listaParticipacion = new List<Participacion>();
    private readonly ILogger<ParticipacionController> _logger;

    public ParticipacionController(ILogger<ParticipacionController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult create([FromBody] Participacion nuevaParticipacion)
    {
        try
        {
            if (nuevaParticipacion.Id <= 0)
            {
                return BadRequest("Ingrese un id valido");
            }

            var participacionExistente = listaParticipacion.FirstOrDefault(p => p.Id == nuevaParticipacion.Id);
            if (participacionExistente is not null)
            {
                return BadRequest("El id de esta participacion ya existe");
            }

            var expedicionBuscada = ExpedicionController.listaExpedicion.FirstOrDefault(e => e.Id == nuevaParticipacion.ExpedicionId);
            if (expedicionBuscada is null)
            {
                return NotFound("No se encontro la expedicion");
            }

            var exploradorBuscado = ExploradorController.listaExplorador.FirstOrDefault(e => e.Id == nuevaParticipacion.ExploradorId);
            if (exploradorBuscado is null)
            {
                return NotFound("No se encontro el explorador");
            }

            if (exploradorBuscado.Disponible == false)
            {
                return BadRequest("El explorador no esta disponible");
            }

            var yaAsignado = listaParticipacion.Any(p => p.ExploradorId == nuevaParticipacion.ExploradorId && p.ExpedicionId == nuevaParticipacion.ExpedicionId);
            if (yaAsignado)
            {
                return BadRequest("El explorador ya esta asignado a esta expedicion");
            }

            int cantidad = listaParticipacion.Count(p => p.ExpedicionId == nuevaParticipacion.ExpedicionId);
            if (cantidad >= expedicionBuscada.Capacidad)
            {
                return BadRequest("La expedicion alcanzo su capacidad maxima");
            }

            var participacionesDelExplorador = listaParticipacion.Where(p => p.ExploradorId == nuevaParticipacion.ExploradorId);

            var haySuperposicion = ExpedicionController.listaExpedicion.Any(e => participacionesDelExplorador.Any(p => p.ExpedicionId == e.Id) && expedicionBuscada.FechaInicio <= e.FechaFin && expedicionBuscada.FechaFin >= e.FechaInicio);

            if (haySuperposicion)
            {
                return BadRequest("El explorador ya participa en una expedicion con fechas superpuestas");
            }

            listaParticipacion.Add(nuevaParticipacion);
            return Ok("Explorador asignado con exito");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet]
    public IActionResult getAll()
    {
        try
        {
            return Ok(listaParticipacion);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}