using System.Data.Common;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace API2Grupo.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicionController : ControllerBase
{

    private static List<Estacion> listEstaciones = new List<Estacion>()
  {
    new Estacion {Id = 1,Nombre = "Estacion 1", Localidad = "Cordoba", Activa = true, ListMediciones = {new Medicion { Id = 1, EstacionId = 1, Temperatura = 25.5, Humedad = 60, VelocidadViento = 10, FechaHora = DateTime.Now }}},
    new Estacion {Id = 2,Nombre = "Estacion 2", Localidad = "Rosario", Activa = false, ListMediciones = {}}
  };
    private readonly ILogger<MedicionController> _logger;

    public MedicionController(ILogger<MedicionController> logger)
    {
        _logger = logger;
    }



    [HttpPost("Estacion")]
    public IActionResult CreateStation([FromBody] Estacion nuevaEstacion)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nuevaEstacion.Nombre))
            {
                return BadRequest("Ingresa un nombre valido.");
            }

            listEstaciones.Add(nuevaEstacion);
            return Created($"Api/Estacion/{nuevaEstacion.Id}", nuevaEstacion);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpPost("Medicion")]
    public IActionResult CreateMedition([FromBody] Medicion nuevaMedicion)
    {
        try
        {
            if (nuevaMedicion.EstacionId <= 0)
            {
                return BadRequest("Ingresa un id valido");
            }


            if (nuevaMedicion.Humedad > 100 || nuevaMedicion.Humedad < 0)
            {
                return BadRequest("Ingrese una humedad valida");
            }

            if (nuevaMedicion.VelocidadViento < 0)
            {
                return BadRequest("La velocidad del viento no puede ser negativa");
            }

            if (nuevaMedicion.FechaHora > DateTime.Now)
            {
                return BadRequest("No se puede ingresar una fecha futura");
            }


            var estacion = listEstaciones.FirstOrDefault(e => e.Id == nuevaMedicion.EstacionId);

            if (estacion == null)
            {
                return NotFound("No existe una estacion con ese id");
            }

            if (estacion.Activa == false)
            {
                return Conflict("La estacion esta inactiva, no puede recibir mediciones");
            }

            estacion.ListMediciones.Add(nuevaMedicion);
            return Created($"Api/Medicion/{nuevaMedicion.Id}", nuevaMedicion);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var todasLasMediciones = listEstaciones.SelectMany(e => e.ListMediciones).ToList();

            return Ok(todasLasMediciones);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("{idEstacion}/Medicion")]
    public IActionResult GetMeditionsByIdStation(int idEstacion)
    {
        try
        {
            var estacion = listEstaciones.FirstOrDefault(e => e.Id == idEstacion);

            if (estacion == null)
            {
                return NotFound("No existe una estacion con ese id");
            }

            return Ok(estacion.ListMediciones);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("{localidad}/Estacion")]
    public IActionResult GetStationByLocality(string localidad)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(localidad))
            {
                return BadRequest("Ingrese una localidad valida");
            }

            var estaciones = listEstaciones.Where(e => e.Localidad == localidad);
            if (!estaciones.Any())
            {
                return NotFound("No hay ninguna estacion en esa localdidad");
            }

            return Ok(estaciones);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }

    }



    [HttpGet("TemperaturaMayor/{tempParametro}")]
    public IActionResult GetTemperatureLarger(double tempParametro)
    {
        try
        {
            var tempsMayores = listEstaciones.SelectMany(e => e.ListMediciones).Where(m => m.Temperatura > tempParametro).ToList();

            if (!tempsMayores.Any())
            {
                return NotFound("No hay temperaturas mayores a esa");
            }

            return Ok(tempsMayores);

        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("VerMedicionesOrdenadasPorTemperatura")]
    public IActionResult GetMeditionsOrderedByTemperature()
    {
        try
        {
            var mediciones = listEstaciones.SelectMany(e => e.ListMediciones).OrderByDescending(m => m.Temperatura).ToList();

            return Ok(mediciones);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("TemperaturaPromedio")]
    public IActionResult GetAverageTemperature()
    {
        try
        {
            var mediciones = listEstaciones.SelectMany(e => e.ListMediciones).ToList();

            if (!mediciones.Any())
            {
                return NotFound("No hay mediciones cargadas para calcular el promedio");
            }

            var promedio = mediciones.Average(m => m.Temperatura);

            return Ok(promedio);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("TemperaturaMaxima")]
    public IActionResult GetMaxTemperature()
    {
        try
        {
            var mediciones = listEstaciones.SelectMany(e => e.ListMediciones).ToList();

            if (!mediciones.Any())
            {
                return NotFound("No hay mediciones cargadas");
            }

            var max = mediciones.Max(m => m.Temperatura);

            return Ok(max);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("TemperaturaMinima")]
    public IActionResult GetMinTemperature()
    {
        try
        {
            var mediciones = listEstaciones.SelectMany(e => e.ListMediciones).ToList();

            if (!mediciones.Any())
            {
                return NotFound("No hay mediciones cargadas");
            }

            var min = mediciones.Min(m => m.Temperatura);

            return Ok(min);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("CantidadMedicionesPorEstacion")]
    public IActionResult GetMeditionCountByStation()
    {
        try
        {
            var conteoPorEstacion = listEstaciones.Select(e => new
            {
                EstacionId = e.Id,
                Nombre = e.Nombre,
                CantidadMediciones = e.ListMediciones.Count
            }).ToList();

            if (!conteoPorEstacion.Any())
            {
                return NotFound("No hay estaciones cargadas");
            }

            return Ok(conteoPorEstacion);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }









}
