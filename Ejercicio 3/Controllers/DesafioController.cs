using System.Runtime.Serialization;
using System.Timers;
using Microsoft.AspNetCore.Mvc;
namespace Ejercicio3.Controllers;


[ApiController]
[Route("[controller]")]
public class DesafioController : ControllerBase
{

    public static List<Desafio> listaDesafio = new List<Desafio>();
    private readonly ILogger<DesafioController> _logger;

    public DesafioController(ILogger<DesafioController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult create([FromBody]Desafio nuevoDesafio)
    {
        try
        {
            if (nuevoDesafio.Id <= 0)
            {
                return BadRequest("Ingrese un id valido");
            }

            var desafioExistente = listaDesafio.FirstOrDefault(d=> d.Id == nuevoDesafio.Id);
            if (desafioExistente.Id == nuevoDesafio.Id )
            {
                return BadRequest("El id de este desafio ya existe");
            }
            
            listaDesafio.Add(nuevoDesafio);
            return Ok("Desafio registrado con exito");
        }
        catch(Exception ex)
        {
            return StatusCode (500, $"Error interno del servidor: {ex.Message}");
        }
    }

    

    [HttpGet("Desafios activos")]

    public IActionResult GetByActivo()
    {
        try
        {

            var desafioBuscado = listaDesafio.Where(d=> d.Activo == true);

            if(desafioBuscado is null)
            {
                return NotFound("No hay desafios activos");
            }

            return Ok(desafioBuscado);
        }
        catch(Exception ex)
        {
            return StatusCode (500, $"Error interno del servidor: {ex.Message}");
        }
    }



    [HttpGet("Desafios por {dificultad}")]

    public IActionResult getByDifficulty(string dificultad)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dificultad))
            {
                return BadRequest("Ingrese una dificultad");
            }

            var desafioBuscado = listaDesafio.Where(d=> d.Dificultad.ToLower() == dificultad.ToLower());

            if(desafioBuscado is null)
            {
                return NotFound("Desafio con esa dificultad no encontrada");
            }

            return Ok(desafioBuscado); 
        }
        catch(Exception ex)
        {
            return StatusCode (500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet("Desafiopromedio{idDesafio}")]

    public IActionResult getPromedio(int idDesafio)
    {
        try
        {

            var resoluciones = ResolucionController.listaResolucion.Where(r => r.DesafioId == idDesafio);
            if(resoluciones is null)
            {
                return NotFound("No se encontro");
            }

            var promedio = resoluciones.Average(r=> r.PuntajeObtenido);
            return Ok(promedio);
        }
        catch(Exception ex)
        {
            return StatusCode (500, $"Error interno del servidor: {ex.Message}");
        }
    }




}
