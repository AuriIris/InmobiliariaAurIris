namespace MVC.Models;

public class Persona
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Telefono { get; set; }
    public string? Mail { get; set; }
    public string? Dni { get; set; }
    public Persona(){
        Nombre="";
    }
   public override string ToString()
		{
			//return $"{Apellido}, {Nombre}";
			return $"{Nombre} {Apellido}";
		}
}
