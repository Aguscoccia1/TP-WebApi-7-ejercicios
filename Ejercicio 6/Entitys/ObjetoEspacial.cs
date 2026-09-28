namespace API2Grupo;
 
public class ObjetoEspacial
{
    private int id;
    private string nombre;
    private string tipo;
    private double distancia;
    private int nivelRiesgo;
    private DateTime fechaDescubrimiento;
    private bool activo = true;
    private List<Observacion> listObservaciones = new List<Observacion>();
 
    public int Id { get { return this.id; } set { this.id = value; } }
    public string Nombre { get { return this.nombre; } set { this.nombre = value; } }
    public string Tipo { get { return this.tipo; } set { this.tipo = value; } }
    public double Distancia { get { return this.distancia; } set { this.distancia = value; } }
    public int NivelRiesgo { get { return this.nivelRiesgo; } set { this.nivelRiesgo = value; } }
    public DateTime FechaDescubrimiento { get { return this.fechaDescubrimiento; } set { this.fechaDescubrimiento = value; } }
    public bool Activo { get { return this.activo; } set { this.activo = value; } }
    public List<Observacion> ListObservaciones { get { return this.listObservaciones; } set { this.listObservaciones = value; } }
 
}
/*
Tipos posibles:
* Asteroide
* Cometa
* Satelite
* Desconocido
 
NivelRiesgo: de 1 (muy bajo) a 5 (critico)
Distancia: en millones de km
*/
 