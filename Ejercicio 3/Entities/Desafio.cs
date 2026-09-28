namespace Ejercicio3;

public class Desafio
{
    private int id;
    private string titulo;
    private string dificultad;
    private double puntajeMaximo;
    private bool activo;




    public int Id {get{return this.id;} set { this.id = value;}}
    public string Titulo {get{return this.titulo;} set { this.titulo = value;}}
    public string Dificultad {get{return this.dificultad;} set{this.dificultad = value;}}
    public double PuntajeMaximo {get{return this.puntajeMaximo;} set{this.puntajeMaximo = value;}}
    public bool Activo {get{return this.activo;} set{ this.activo = value;}}


}
