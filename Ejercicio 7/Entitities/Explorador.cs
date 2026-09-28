namespace Ejercicio7;

public class Explorador
{
    private int id;
    private string nombre;
    private string especialidad;
    private int experiencia;
    private bool disponibilidad;

    public int Id {get{return this.id;} set{this.id = value;}}
    public string Nombre {get{return this.nombre;} set{this.nombre = value;}}
    public string Especialidad {get{return this.especialidad;} set{this.especialidad = value;}}
    public int Experiencia {get{return this.experiencia;} set{this.experiencia = value;}}
    public bool Disponible {get{return this.disponibilidad;} set{this.disponibilidad = value;}}
}