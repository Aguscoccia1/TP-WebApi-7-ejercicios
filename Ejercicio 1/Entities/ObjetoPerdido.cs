namespace SistemasAlumnos.API;

public class ObjetoPerdido
{
    private int id;
    private string descripcion;
    private string categoria;
    private string lugarEncontrado;
    private DateTime fechaEncontrada;
    private bool reclamado = false;
    private string nombrePersonaQueRetiro;



    public int Id{get{return this.id;} set{this.id =value;}}
    public string Descripcion{get{return this.descripcion;} set{this.descripcion =value;}}
    public string Categoria{get{return this.categoria;} set{this.categoria =value;}}
    public string LugarEncontrado{get{return this.lugarEncontrado;} set{this.lugarEncontrado =value;}}
    public DateTime FechaEncontrada{get{return this.fechaEncontrada;} set{this.fechaEncontrada =value;}}
    public bool Reclamado{get{return this.reclamado;} set{this.reclamado =value;}}  
    public string NombrePersonaQueRetiro{get{return this.nombrePersonaQueRetiro;} set{this.nombrePersonaQueRetiro =value;}}

}
