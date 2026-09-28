using Microsoft.AspNetCore.Mvc;

namespace API2Grupo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ObjetosEspacialesController : ControllerBase
{
    private static readonly string[] tiposValidos = { "Asteroide", "Cometa", "Satelite", "Desconocido" };

    private static List<ObjetoEspacial> listObjetos = new List<ObjetoEspacial>()
    {
        new ObjetoEspacial{Id = 1, Nombre = "Apophis", Tipo = "Asteroide", Distancia = 30, NivelRiesgo = 4,FechaDescubrimiento = new DateTime(2004, 6, 19), Activo = true,
            ListObservaciones = new List<Observacion>
            {
                new Observacion { Id = 1, ObjetoEspacialId = 1, Fecha = new DateTime(2025, 1, 10), DistanciaMedida = 31, Velocidad = 30.7, Comentario = "Trayectoria estable" },
                new Observacion { Id = 2, ObjetoEspacialId = 1, Fecha = new DateTime(2025, 3, 2), DistanciaMedida = 30, Velocidad = 30.9, Comentario = "Leve acercamiento" }
            }},

        new ObjetoEspacial{Id = 2, Nombre = "Halley", Tipo = "Cometa", Distancia = 4000, NivelRiesgo = 1,FechaDescubrimiento = new DateTime(1758, 12, 25), Activo = true,
            ListObservaciones = new List<Observacion>
            {
                new Observacion { Id = 3, ObjetoEspacialId = 2, Fecha = new DateTime(2025, 2, 5), DistanciaMedida = 4000, Velocidad = 54, Comentario = "Sin novedades" }
            }},

        new ObjetoEspacial{Id = 3, Nombre = "Satelite Alfa", Tipo = "Satelite", Distancia = 0.0004, NivelRiesgo = 2,FechaDescubrimiento = new DateTime(2019, 8, 14), Activo = false},
        new ObjetoEspacial{Id = 4, Nombre = "Objeto X-99", Tipo = "Desconocido", Distancia = 60, NivelRiesgo = 5,FechaDescubrimiento = new DateTime(2025, 5, 1), Activo = true},
    };

    private readonly ILogger<ObjetosEspacialesController> _logger;

    public ObjetosEspacialesController(ILogger<ObjetosEspacialesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAllObjetos()
    {
        try
        {
            return Ok(listObjetos);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("id/{id}")]
    public IActionResult GetObjetoById(int id)
    {
        try
        {
            var objeto = listObjetos.FirstOrDefault(o => o.Id == id);

            if (objeto == null)
            {
                return NotFound("No existe un objeto espacial con ese id");
            }

            return Ok(objeto);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("nombre/{nombre}")]
    public IActionResult GetObjetosByNombre(string nombre)
    {
        try
        {
            var objetos = listObjetos.Where(o => o.Nombre.ToLower() == nombre.ToLower()).ToList();

            if (!objetos.Any())
            {
                return NotFound("No se encontraron objetos con ese nombre");
            }

            return Ok(objetos);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("tipo/{tipo}")]
    public IActionResult GetObjetosByTipo(string tipo)
    {
        try
        {
            var objetos = listObjetos.Where(o => o.Tipo.ToLower() == tipo.ToLower()).ToList();
            return Ok(objetos);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("riesgo/{nivelRiesgo}")]
    public IActionResult GetObjetosByRiesgo(int nivelRiesgo)
    {
        try
        {
            var objetos = listObjetos.Where(o => o.NivelRiesgo == nivelRiesgo).ToList();
            return Ok(objetos);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("DistanciaMenorA/{distancia}")]
    public IActionResult GetObjetosByLessDistancia(double distancia)
    {
        try
        {
            var objetos = listObjetos.Where(o => o.Distancia < distancia).ToList();
            return Ok(objetos);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("Observaciones/{idObjeto}")]
    public IActionResult GetObservacionesDeObjeto(int idObjeto)
    {
        try
        {
            var objeto = listObjetos.FirstOrDefault(o => o.Id == idObjeto);

            if (objeto == null)
            {
                return NotFound("No existe un objeto espacial con ese id");
            }

            return Ok(objeto.ListObservaciones);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("PromedioVelocidad")]
    public IActionResult GetPromedioVelocidad()
    {
        try
        {
            var observaciones = listObjetos.SelectMany(o => o.ListObservaciones).ToList();

            if (!observaciones.Any())
            {
                return NotFound("No hay observaciones registradas");
            }

            var promedio = observaciones.Average(ob => ob.Velocidad);
            return Ok(promedio);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("ConMasObservaciones")]
    public IActionResult GetObjetoConMasObservaciones()
    {
        try
        {
            var objeto = listObjetos.OrderByDescending(o => o.ListObservaciones.Count).FirstOrDefault();

            if (objeto == null)
            {
                return NotFound("No hay objetos registrados");
            }

            return Ok(objeto);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("SinObservaciones")]
    public IActionResult GetObjetosSinObservaciones()
    {
        try
        {
            var objetos = listObjetos.Where(o => !o.ListObservaciones.Any()).ToList();

            if (!objetos.Any())
            {
                return NotFound("Todos los objetos poseen observaciones");
            }

            return Ok(objetos);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("OrdenadosPorRiesgo")]
    public IActionResult GetObjetosOrdenadosPorRiesgo()
    {
        try
        {
            var objetos = listObjetos.OrderByDescending(o => o.NivelRiesgo).ToList();
            return Ok(objetos);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("Estadisticas")]
    public IActionResult GetEstadisticas()
    {
        try
        {
            var observaciones = listObjetos.SelectMany(o => o.ListObservaciones).ToList();
 
            double promedioVelocidad = 0;
            double promedioDistancia = 0;
            double promedioRiesgo = 0;
 
            if (observaciones.Any())
            {
                promedioVelocidad = observaciones.Average(ob => ob.Velocidad);
            }
 
            if (listObjetos.Any())
            {
                promedioDistancia = listObjetos.Average(o => o.Distancia);
                promedioRiesgo = listObjetos.Average(o => o.NivelRiesgo);
            }
 
            var estadisticas = new
            {
                TotalObjetos = listObjetos.Count,
                ObjetosActivos = listObjetos.Count(o => o.Activo),
                ObjetosInactivos = listObjetos.Count(o => !o.Activo),
                TotalObservaciones = observaciones.Count,
                ObjetosSinObservaciones = listObjetos.Count(o => !o.ListObservaciones.Any()),
                PromedioVelocidad = promedioVelocidad,
                PromedioDistancia = promedioDistancia,
                PromedioRiesgo = promedioRiesgo,
                CantidadPorTipo = listObjetos.GroupBy(o => o.Tipo).Select(g => new { Tipo = g.Key, Cantidad = g.Count() }).ToList()
            };
 
            return Ok(estadisticas);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("alertas")]
    public IActionResult GetAlertas([FromQuery] int riesgoMinimo = 4, [FromQuery] double distanciaMaxima = 100)
    {
        try
        {
            var alertas = listObjetos
                .Where(o => o.NivelRiesgo >= riesgoMinimo && o.Distancia < distanciaMaxima && o.Activo)
                .OrderByDescending(o => o.NivelRiesgo).ThenBy(o => o.Distancia).ToList();

            if (!alertas.Any())
            {
                return NotFound("No hay objetos que cumplan las condiciones de alerta");
            }

            return Ok(alertas);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpPost]
    public IActionResult CreateObjeto([FromBody] ObjetoEspacial nuevoObjeto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nuevoObjeto.Nombre))
            {
                return BadRequest("Ingrese un nombre valido");
            }

            if (!tiposValidos.Any(t => t.ToLower() == (nuevoObjeto.Tipo).ToLower()))
            {
                return BadRequest("El tipo debe ser: Asteroide, Cometa, Satelite o Desconocido");
            }

            if (nuevoObjeto.Distancia < 0)
            {
                return BadRequest("La distancia no puede ser negativa");
            }

            if (nuevoObjeto.NivelRiesgo < 1 || nuevoObjeto.NivelRiesgo > 5)
            {
                return BadRequest("El nivel de riesgo debe estar entre 1 y 5");
            }

            if (listObjetos.Any(o => o.Nombre.ToLower() == nuevoObjeto.Nombre.ToLower()))
            {
                return Conflict("Ya existe un objeto espacial con ese nombre");
            }


            if (nuevoObjeto.FechaDescubrimiento == default)
            {
                nuevoObjeto.FechaDescubrimiento = DateTime.Now;
            }

            nuevoObjeto.ListObservaciones = new List<Observacion>();

            listObjetos.Add(nuevoObjeto);

            return Created($"Api/ObjetoEspacial/listObjetos/{nuevoObjeto.Id}",nuevoObjeto);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpPost("Observacion")]
    public IActionResult CreateObservacion([FromBody] Observacion nuevaObservacion)
    {
        try
        {
            var objeto = listObjetos.FirstOrDefault(o => o.Id == nuevaObservacion.ObjetoEspacialId);

            if (objeto == null)
            {
                return NotFound("No se encontro un objeto espacial con ese id");
            }

            if (!objeto.Activo)
            {
                return Conflict("El objeto espacial no esta activo");
            }

            if (nuevaObservacion.DistanciaMedida < 0)
            {
                return BadRequest("La distancia medida no puede ser negativa");
            }

            if (nuevaObservacion.Velocidad < 0)
            {
                return BadRequest("La velocidad no puede ser negativa");
            }

            var todasLasObservaciones = listObjetos.SelectMany(o => o.ListObservaciones);

            if (nuevaObservacion.Fecha == default)
            {
                nuevaObservacion.Fecha = DateTime.Now;
            }

            objeto.ListObservaciones.Add(nuevaObservacion);

            return Created($"api/ObjetosEspaciales/Observaciones/{objeto.Id}", nuevaObservacion);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }
}
