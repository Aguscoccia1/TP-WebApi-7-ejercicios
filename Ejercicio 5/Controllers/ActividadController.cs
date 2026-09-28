using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Ejercicio5.Controllers;

[ApiController]
[Route("[controller]")]
public class ActividadController : ControllerBase
{

    public static List<Actividad> listaActividades = new List<Actividad>();
    private readonly ILogger<ActividadController> _logger;

    public ActividadController(ILogger<ActividadController> logger)
    {
        _logger = logger;
    }


    [HttpPost]

    public IActionResult create([FromBody] Actividad nuevaActividad)
    {
        try
        {
            if(nuevaActividad.Id <=0)
            {
                return BadRequest("Ingrese un id válido");
            }
            
            var actividadExistente = listaActividades.FirstOrDefault(a=> a.Id == nuevaActividad.Id);

            if (actividadExistente is not null)
            {
                return BadRequest("El id de esta actividad ya existe");
            }
            
            listaActividades.Add(nuevaActividad);
            return Ok("Actividad registrada con exito");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet]
    public IActionResult getAll()
    {
        try
        {
           return Ok(listaActividades); 
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }



    [HttpGet("Participantes en actividad/{idActividad}")]

    public IActionResult getParticipantes(int idActividad)
    {
        try
        {
            var reservas = ReservaActividadController.listaReservaActividades.Where(r=> r.IdActividad == idActividad);

            var participantes = ParticipanteController.listaParticipantes.Where(p => reservas.Any(r => r.IdParticipante == p.Id));

            if (!participantes.Any())
            {
                return NotFound("No se encontro participante en esta actividad");
            }

            return Ok(participantes);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet("nombre/{nombre}")]

    public IActionResult getByNombre(string nombre)
    {
        try
        {
            var actividadBuscada = listaActividades.FirstOrDefault(a=> a.Nombre.ToLower() == nombre.ToLower());

            if (actividadBuscada is null)
            {
                return NotFound("No se encontro una actividad con ese nombre");                
            }

            return Ok(actividadBuscada);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("tipo/{tipo}")]


    public IActionResult getByTipo(string tipo)
    {
        try
        {
            var actividadBuscada = listaActividades.Where(a=> a.Tipo.ToLower() == tipo.ToLower());

            if (!actividadBuscada.Any())
            {
                return NotFound("No se encontro ninguna actividad de este tipo");
            }

           return Ok(actividadBuscada); 
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }



    [HttpGet("LugarDisponible")]

    public IActionResult GetDisponibilidad()
    {
        try
        {
            var actividad = listaActividades.Where(a=> a.Activo == true && ReservaActividadController.listaReservaActividades.Count(r=> r.IdActividad == a.Id) < a.Capacidad);

            if (!actividad.Any())
            {
                return NotFound("No se encontraron actividades disponibles");
            }

            return Ok(actividad); 
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet("Completas")]


    public IActionResult GetCompletas()
    {
        try
        {
            var actividadBuscada = listaActividades.Where(a=> a.Activo == true && ReservaActividadController.listaReservaActividades.Count(r=> r.IdActividad == a.Id)== a.Capacidad);

            if (!actividadBuscada.Any())
            {
                return NotFound("No se encontraron actividades completas");
            }

            return Ok();
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet("ordenadas")]

    public IActionResult GetOrdenadas()
    {
        try
        {
            var ActividadOrdenada = listaActividades.OrderByDescending(a=> ReservaActividadController.listaReservaActividades.Count(r=> r.IdActividad == a.Id) );
            
            return Ok(ActividadOrdenada);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet("MasParticipantes")]

    public IActionResult getMasParticipantes()
    {
        try
        {
            var actividadBuscada = listaActividades.Max(a=> ReservaActividadController.listaReservaActividades.Count(r=> r.IdActividad == a.Id));

            return Ok(actividadBuscada);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("porcentajeOcupacion/{idActividad}")]
    public IActionResult getPorcentajeOcupacion(int idActividad)
    {
        try
        {
            var actividadBuscada = listaActividades.FirstOrDefault(a => a.Id == idActividad);

            if (actividadBuscada is null)
            {
                return NotFound("No se encontro la actividad");
            }

            int inscriptos = ReservaActividadController.listaReservaActividades.Count(r => r.IdActividad == idActividad);
            double porcentaje = (double)inscriptos / actividadBuscada.Capacidad * 100;

            return Ok(porcentaje);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}
