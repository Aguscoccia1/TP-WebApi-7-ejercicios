using Microsoft.AspNetCore.Mvc;

namespace API2Grupo.Controllers;

[ApiController]
[Route("[controller]")]
public class DroneController : ControllerBase
{
    private static List<Drone> listDrones = new List<Drone>()
   {
        new Drone {Id = 1, Code = 100, Model = "A5", Batery = 100, Status = "disponible"},
        new Drone {Id = 2, Code = 101, Model = "A5", Batery = 80, Status = "en mision"},
        new Drone {Id = 3, Code = 102, Model = "A5", Batery = 10, Status = "mantenimiento"},
   };

    private readonly ILogger<DroneController> _logger;

    public DroneController(ILogger<DroneController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAllDrones()
    {
        try
        {
            return Ok(listDrones);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("id/{id}")]
    public IActionResult GetDroneById(int id)
    {
        try
        {
            var drone = listDrones.FirstOrDefault(d => d.Id == id);

            if (drone == null)
            {
                return NotFound($"No existe un drone con ese id");
            }

            return Ok(drone);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("Disponibles")]
    public IActionResult GetDronesDisponibles()
    {
        try
        {
            var dronesDisponibles = listDrones.Where(d => d.Status.ToString() == "disponible");
            return Ok(dronesDisponibles);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("BateriaMenorA/{baterypercentaje}")]
    public IActionResult GetDronesByLessBatery(int baterypercentaje)
    {
        try
        {
            var drones = listDrones.Where(d => d.Batery < baterypercentaje);
            return Ok(drones);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("modelo/{model}")]
    public IActionResult GetDronesByModel(string model)
    {
        try
        {
            var drones = listDrones.Where(d => d.Model.ToString() == model.ToString());
            return Ok(drones);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("MisionesRealizadas/{idDrone}")]
    public IActionResult GetMissionesDoneForDrone(int idDrone)
    {
        try
        {
            var dron = listDrones.FirstOrDefault(d => d.Id == idDrone);

            if (dron == null)
            {
                return NotFound("No existe un dron con ese id");
            }

            var misionesCompletadas = dron.ListMisiones.Where(m => m.Completed == true).ToList();

            return Ok(misionesCompletadas);


        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("DistanciaTotalRecorrida")]
    public IActionResult GetTotalDistanceForEachDrone()
    {
        try
        {
            var distancias = listDrones.Select(d => new
            {
                DroneId = d.Id,
                DistanciaTotalKm = d.ListMisiones.Sum(m => m.DistanceKm)
            }).ToList();


            return Ok(distancias);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("DronConMasKmRecorridos")]
    public IActionResult GetDroneWithMoreKm()
    {
        try
        {
            var droneConMasKm = listDrones.Select(d => new
            {
                DroneId = d.Id,
                DistanciaTotalKm = d.ListMisiones.Sum(m => m.DistanceKm)
            }).OrderByDescending(x => x.DistanciaTotalKm).FirstOrDefault();

            if (droneConMasKm == null)
            {
                return NotFound("No hay drones registrados");
            }

            return Ok(droneConMasKm);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("PromedioDeBateria")]
    public IActionResult GetBateryAverageFromEveryDrones()
    {
        try
        {
            var drones = listDrones.Where(d => d.Status.ToLower() == "disponible").ToList();
            if (!drones.Any())
            {
                return NotFound("No hay drones disponibles");
            }

            var promedio = drones.Average(d => d.Batery);
            return Ok(promedio);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpGet("MisionesPendientesOrdenadasPorFecha")]
    public IActionResult GetPendentMissionsOrderByDate()
    {
        try
        {
            var misionesPendientes = listDrones.SelectMany(d => d.ListMisiones).Where(m => m.Completed == false).OrderBy(m => m.Date).ToList();

            if (!misionesPendientes.Any())
            {
                return NotFound("No hay misiones pendientes");
            }

            return Ok(misionesPendientes);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpPost]
    public IActionResult CreateDrone([FromBody] Drone nuevoDrone)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nuevoDrone.Model))
            {
                return BadRequest("Ingrese un modelo valido");
            }

            if (nuevoDrone.Batery < 0 || nuevoDrone.Batery > 100)
            {
                return BadRequest("La bateria debe estar entre 0 y 100");
            }

            // El sistema asigna el Id, ignorando lo que mande el cliente
            nuevoDrone.Id = listDrones.Any() ? listDrones.Max(d => d.Id) + 1 : 1;

            listDrones.Add(nuevoDrone);

            return CreatedAtAction(nameof(GetDroneById), new { id = nuevoDrone.Id }, nuevoDrone);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpPost("Mision")]
    public IActionResult CreateMision([FromBody] Mision nuevaMision)
    {
        try
        {
            var drone = listDrones.FirstOrDefault(d => d.Id == nuevaMision.DroneId);
            if (drone == null)
            {
                return NotFound("No se encontro un dron con ese id");
            }

            if (drone.Status.ToString() != "disponible")
            {
                return Conflict("El dron no está disponible");
            }

            if (drone.Batery < 30)
            {
                return Conflict("El dron no tiene la bateria suficiente");
            }

            if (drone.Status.ToString() == "en mision")
            {
                return Conflict("El dron ya tiene una mision activa");
            }

            drone.ListMisiones.Add(nuevaMision);

            drone.Status = "en mision";

            return Created($"Api/Drone/ListMisiones/{nuevaMision.Id}", nuevaMision);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpPut("{id}")]
    public IActionResult UpdateDrone(int id, [FromBody] Drone droneUpdated)
    {
        try
        {
            var drone = listDrones.FirstOrDefault(d => d.Id == id);

            if (drone == null)
            {
                return NotFound($"No existe un drone con Id {id}");
            }

            if (string.IsNullOrWhiteSpace(droneUpdated.Model))
            {
                return BadRequest("Ingrese un modelo valido");
            }

            if (droneUpdated.Batery < 0 || droneUpdated.Batery > 100)
            {
                return BadRequest("La bateria debe estar entre 0 y 100");
            }

            drone.Code = droneUpdated.Code;
            drone.Model = droneUpdated.Model;
            drone.Batery = droneUpdated.Batery;
            drone.Status = droneUpdated.Status;

            return Ok(drone);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }



    [HttpPut("ActualizarMision/{idMision}")]
    public IActionResult UpdateMissionStatus(int idMision)
    {
        var mision = listDrones.SelectMany(d => d.ListMisiones).FirstOrDefault(m => m.Id == idMision);

        if (mision == null)
        {
            return NotFound($"No existe una mision con ese id");
        }

        mision.Completed = true;

        var drone = listDrones.FirstOrDefault(d => d.Id == mision.DroneId);
        if (drone != null)
        {
            drone.Status = "disponible";
        }

        return Ok(mision);
    }



    [HttpDelete("{id}")]
    public IActionResult DeleteDrone(int id)
    {
        try
        {
            var drone = listDrones.FirstOrDefault(d => d.Id == id);

            if (drone == null)
            {
                return NotFound($"No existe un drone con Id {id}");
            }

            listDrones.Remove(drone);

            return NoContent();
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error en el sistema: {ex.Message}");
        }
    }
}


