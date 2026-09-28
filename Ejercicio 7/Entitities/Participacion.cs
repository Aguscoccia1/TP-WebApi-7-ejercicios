namespace Ejercicio7;

public class Participacion
{
    private int id;
    private int expedicionId;
    private int exploradorId;
    private string rol;

    public int Id {get{return this.id;} set{this.id = value;}}
    public int ExpedicionId {get{return this.expedicionId;} set{this.expedicionId = value;}}
    public int ExploradorId {get{return this.exploradorId;} set{this.exploradorId = value;}}
    public string Rol {get{return this.rol;} set{this.rol = value;}}
}