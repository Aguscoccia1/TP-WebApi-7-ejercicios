namespace Ejercicio5;

public class Actividad
{
    private int id;
    private string nombre;
    private string tipo;
    private TimeSpan horario;
    private int duracionMinutos;
    private int capacidad;
    private bool activo;


    public int Id {get{return this.id;} set{this.id = value;}}
    public string Nombre {get{return this.nombre;} set{this.nombre = value;}}
    public string Tipo {get{return this.tipo;} set{this.tipo = value;}} 
    public TimeSpan Horario {get{return this.horario;} set{this.horario = value;}}
    public int DuracionMinutos {get{return this.duracionMinutos;} set{this.duracionMinutos = value;}}
    public int Capacidad {get{return this.capacidad;} set{this.capacidad = value;}}
    public bool Activo {get{return this.activo;} set{this.activo = value;}}

}
