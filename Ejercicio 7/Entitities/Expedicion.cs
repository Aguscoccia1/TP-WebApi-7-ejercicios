namespace Ejercicio7;

public class Expedicion
{
    private int id;
    private string nombre;
    private string destino;
    private DateTime fechainicio;
    private DateTime fechafin;
    private int capacidad;
    private string estado;
    

    public int Id {get{return this.id;} set{this.id = value;}}
    public string Nombre {get{return this.nombre;} set{this.nombre = value;}}
    public string Destino {get{return this.destino;} set{this.destino = value;}}
    public DateTime FechaInicio {get{return this.fechainicio;} set{this.fechainicio = value;}}
    public DateTime FechaFin {get{return this.fechafin;} set{this.fechafin = value;}}
    public int Capacidad {get{return this.capacidad;} set{this.capacidad = value;}}
    public string Estado {get{return this.estado;} set{this.estado = value;}}
}
