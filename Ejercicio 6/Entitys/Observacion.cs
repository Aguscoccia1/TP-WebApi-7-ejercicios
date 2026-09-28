namespace API2Grupo;
 
public class Observacion
{
    private int id;
    private int objetoEspacialId;
    private DateTime fecha;
    private double distanciaMedida;
    private double velocidad;
    private string comentario;
 
    public int Id { get { return this.id; } set { this.id = value; } }
    public int ObjetoEspacialId { get { return this.objetoEspacialId; } set { this.objetoEspacialId = value; } }
    public DateTime Fecha { get { return this.fecha; } set { this.fecha = value; } }
    public double DistanciaMedida { get { return this.distanciaMedida; } set { this.distanciaMedida = value; } }
    public double Velocidad { get { return this.velocidad; } set { this.velocidad = value; } }
    public string Comentario { get { return this.comentario; } set { this.comentario = value; } }
 
}
 