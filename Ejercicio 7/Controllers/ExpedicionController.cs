using Microsoft.AspNetCore.Mvc;
namespace Ejercicio7.Controllers;

[ApiController]
[Route("[controller]")]
public class ExpedicionController : ControllerBase
{
    public static List<Expedicion> listaExpedicion = new List<Expedicion>();
    private readonly ILogger<ExpedicionController> _logger;

    public ExpedicionController(ILogger<ExpedicionController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult create([FromBody] Expedicion nuevaExpedicion)
    {
        try
        {
            if (nuevaExpedicion.Id <= 0)
            {
                return BadRequest("Ingrese un id valido");
            }

            var expedicionExistente = listaExpedicion.FirstOrDefault(e => e.Id == nuevaExpedicion.Id);
            if (expedicionExistente is not null)
            {
                return BadRequest("El id de esta expedicion ya existe");
            }

            if (nuevaExpedicion.Capacidad <= 0)
            {
                return BadRequest("La capacidad debe ser mayor a 0");
            }

            if (nuevaExpedicion.FechaFin < nuevaExpedicion.FechaInicio)
            {
                return BadRequest("La fecha de fin no puede ser anterior a la fecha de inicio");
            }

            listaExpedicion.Add(nuevaExpedicion);
            return Ok("Expedicion registrada con exito");
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
            return Ok(listaExpedicion);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("integrantes/{idExpedicion}")]
    public IActionResult getIntegrantes(int idExpedicion)
    {
        try
        {
            var participaciones = ParticipacionController.listaParticipacion.Where(p => p.ExpedicionId == idExpedicion);

            var exploradores = ExploradorController.listaExplorador.Where(e => participaciones.Any(p => p.ExploradorId == e.Id));

            if (!exploradores.Any())
            {
                return NotFound("Esta expedicion no tiene integrantes");
            }

            return Ok(exploradores);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("activas")]
    public IActionResult getActivas()
    {
        try
        {
            var expediciones = listaExpedicion.Where(e => e.FechaInicio <= DateTime.Now && e.FechaFin >= DateTime.Now);

            if (!expediciones.Any())
            {
                return NotFound("No hay expediciones activas");
            }

            return Ok(expediciones);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("futuras")]
    public IActionResult getFuturas()
    {
        try
        {
            var expediciones = listaExpedicion.Where(e => e.FechaInicio > DateTime.Now);

            if (!expediciones.Any())
            {
                return NotFound("No hay expediciones futuras");
            }

            return Ok(expediciones);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("completas")]
    public IActionResult getCompletas()
    {
        try
        {
            var expediciones = listaExpedicion.Where(e => ParticipacionController.listaParticipacion.Count(p => p.ExpedicionId == e.Id) == e.Capacidad);

            if (!expediciones.Any())
            {
                return NotFound("No hay expediciones completas");
            }

            return Ok(expediciones);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("cantidad/{idExpedicion}")]
    public IActionResult getCantidad(int idExpedicion)
    {
        try
        {
            var expedicionBuscada = listaExpedicion.FirstOrDefault(e => e.Id == idExpedicion);
            if (expedicionBuscada is null)
            {
                return NotFound("No se encontro la expedicion");
            }

            int cantidad = ParticipacionController.listaParticipacion.Count(p => p.ExpedicionId == idExpedicion);
            return Ok(cantidad);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("ocupacion/{idExpedicion}")]
    public IActionResult getOcupacion(int idExpedicion)
    {
        try
        {
            var expedicionBuscada = listaExpedicion.FirstOrDefault(e => e.Id == idExpedicion);
            if (expedicionBuscada is null)
            {
                return NotFound("No se encontro la expedicion");
            }

            int cantidad = ParticipacionController.listaParticipacion.Count(p => p.ExpedicionId == idExpedicion);
            double porcentaje = (double)cantidad / expedicionBuscada.Capacidad * 100;
            return Ok(porcentaje);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("ordenadas")]
    public IActionResult getOrdenadas()
    {
        try
        {
            var ordenadas = listaExpedicion.OrderByDescending(e => e.FechaInicio);
            return Ok(ordenadas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}