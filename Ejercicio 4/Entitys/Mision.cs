namespace API2Grupo;

public class Mision
{
    private int id;
    private int droneId;
    private string description;
    private double distanceKm;
    private DateTime date;
    private bool completed = false;



    public int Id { get { return this.id; } set { this.id = value; } }
    public int DroneId { get { return this.droneId; } set { this.droneId = value; } }
    public string Description { get { return this.description; } set { this.description = value; } }
    public double DistanceKm { get { return this.distanceKm; } set { this.distanceKm = value; } }
    public DateTime Date { get { return this.date; } set { this.date = value; } }
    public bool Completed { get { return this.completed; } set { this.completed = value; } }


}
