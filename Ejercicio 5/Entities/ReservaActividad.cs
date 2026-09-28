namespace Ejercicio5;

public class ReservaActividad
{
    private int id;
    private int idActividad;
    private int idParticipante;
    private DateTime fechaReserva;

    public int Id {get{return this.id;} set{this.id = value;}}
    public int IdActividad {get{return this.idActividad;} set{this.idActividad = value;}}
    public int IdParticipante {get{return this.idParticipante;} set{this.idParticipante = value;}}
    public DateTime FechaReserva {get{return this.fechaReserva;} set{this.fechaReserva = value;}}
}