namespace Ejercicio3;


public class Participante
{
    private int id;
    private string nombre;
    private string email;
    private int nivel;


    public int Id {get{return this.id;} set { this.id = value;}}
    public string Nombre {get{return this.nombre;} set { this.nombre = value;}}
    public string Email {get{return this.email;} set{this.email = value;}}
    public int Nivel {get{return this.nivel;} set{this.nivel = value;}}

}
