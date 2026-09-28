namespace API2Grupo;

public class Drone
{
    private int id;
    private int code;
    private string model;
    private int batery;
    private string status;
    private List<Mision> listMisiones = new List<Mision>();

    public int Id { get { return this.id; } set { this.id = value; } }
    public int Code { get { return this.code; } set { this.code = value; } }
    public string Model { get { return this.model; } set { this.model = value; } }
    public int Batery { get { return this.batery; } set { this.batery = value; } }
    public string Status { get { return this.status; } set { this.status = value; } }
    public List<Mision> ListMisiones { get { return this.listMisiones; } set { this.listMisiones = value; } }

}
/*
Estados posibles:
* Disponible
● EnMision
● Mantenimiento*/