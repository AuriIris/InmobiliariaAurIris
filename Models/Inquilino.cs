namespace MVC.Models;

public class Inquilino
{
    public int Id { get; set; }
    public string? Dni { get; set; }
    public string? Apellido { get; set; }
    public string? Nombre { get; set; }
    public string? Telefono { get; set; }
    public string? Mail { get; set; }
    
    public Inquilino(){
        Nombre="";
    }
    public Inquilino(string nombre){
        Nombre=nombre;

    }
    public override string ToString()
		{
			//return $"{Apellido}, {Nombre}";
			return $"{Nombre} {Apellido}";
		}
}
