namespace MVC.Models;

public class Inmueble
{
    public int Id { get; set; }
    public String?  Tipo { get; set; }
    public String?  Direccion { get; set; }
    public String?  Uso { get; set; }
    public int  CantHamb  { get; set; }
    public String?  Latitud { get; set; }
    public String?  Longitud { get; set; }
    public Double  Precio { get; set; }
    public String?  Estado { get; set; }
    public int IdPropietario{ get; set; }
    public Propietario? Duenio { get; set;} 

    public Inmueble()
    {
    }
    public override string ToString()
		{
			//return $"{Apellido}, {Nombre}";
			return $"{Tipo} {Direccion}";
		}

}