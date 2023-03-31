namespace MVC.Models;

public class Propietario
{
    public int Id { get; set; }
    public string? Dni { get; set; }
    public string? Apellido { get; set; }
    public string? Nombre { get; set; }
    public string? Telefono { get; set; }
    public string? Mail { get; set; }
    
    public Propietario(){
        Nombre="";
    }
    public Propietario(string nombre){
        Nombre=nombre;

    }
    public override string ToString()
		{
			//return $"{Apellido}, {Nombre}";
			return $"{Nombre} {Apellido}";
		}
}
