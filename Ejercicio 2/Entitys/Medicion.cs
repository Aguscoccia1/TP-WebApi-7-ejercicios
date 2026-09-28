namespace API2Grupo;

public class Medicion
{
    private int id;
    private int estacionId;
    private double temperatura;
    private double humedad;
    private double velocidadViento;
    private DateTime fechaHora;


    public int Id { get { return this.id; } set { this.id = value; } }
    public int EstacionId { get { return this.estacionId; } set { this.estacionId = value; } }
    public double Temperatura { get { return this.temperatura; } set { this.temperatura = value; } }
    public double Humedad { get { return this.humedad; } set { this.humedad = value; } }
    public double VelocidadViento { get { return this.velocidadViento; } set { this.velocidadViento = value; } }
    public DateTime FechaHora { get { return this.fechaHora; } set { this.fechaHora = value; } }

}