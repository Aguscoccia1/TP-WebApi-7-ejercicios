using Microsoft.AspNetCore.Mvc;
namespace Ejercicio7.Controllers;

[ApiController]
[Route("[controller]")]
public class ExploradorController : ControllerBase
{
    public static List<Explorador> listaExplorador = new List<Explorador>();
    private readonly ILogger<ExploradorController> _logger;

    public ExploradorController(ILogger<ExploradorController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult create([FromBody] Explorador nuevoExplorador)
    {
        try
        {
            if (nuevoExplorador.Id <= 0)
            {
                return BadRequest("Ingrese un id valido");
            }

            var exploradorExistente = listaExplorador.FirstOrDefault(e => e.Id == nuevoExplorador.Id);
            if (exploradorExistente is not null)
            {
                return BadRequest("El id de este explorador ya existe");
            }

            listaExplorador.Add(nuevoExplorador);
            return Ok("Explorador registrado con exito");
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
            return Ok(listaExplorador);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("expediciones/{idExplorador}")]
    public IActionResult getExpediciones(int idExplorador)
    {
        try
        {
            var participaciones = ParticipacionController.listaParticipacion.Where(p => p.ExploradorId == idExplorador);

            var expediciones = ExpedicionController.listaExpedicion.Where(e => participaciones.Any(p => p.ExpedicionId == e.Id));

            if (!expediciones.Any())
            {
                return NotFound("Este explorador no participa en ninguna expedicion");
            }

            return Ok(expediciones);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("especialidad/{especialidad}")]
    public IActionResult getByEspecialidad(string especialidad)
    {
        try
        {
            var exploradores = listaExplorador.Where(e => e.Especialidad.ToLower() == especialidad.ToLower());

            if (!exploradores.Any())
            {
                return NotFound("No se encontraron exploradores con esa especialidad");
            }

            return Ok(exploradores);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("disponibles")]
    public IActionResult getDisponibles()
    {
        try
        {
            var exploradores = listaExplorador.Where(e => e.Disponible == true);

            if (!exploradores.Any())
            {
                return NotFound("No hay exploradores disponibles");
            }

            return Ok(exploradores);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("masParticipaciones")]
    public IActionResult getMasParticipaciones()
    {
        try
        {
            var ordenados = listaExplorador.OrderByDescending(e => ParticipacionController.listaParticipacion.Count(p => p.ExploradorId == e.Id));

            if (!ordenados.Any())
            {
                return NotFound("No hay exploradores registrados");
            }

            return Ok(ordenados);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("especialidadesMasUsadas")]
    public IActionResult getEspecialidadesMasUsadas()
    {
        try
        {
            var ordenados = listaExplorador.OrderByDescending(e => ParticipacionController.listaParticipacion.Count(p => listaExplorador.Any(x => x.Id == p.ExploradorId && x.Especialidad == e.Especialidad)));

            return Ok(ordenados);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}