using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MVC.Models;

public class Inmueble
{
    public enum enTipo
    {

        Casa = 1,
        Departamento = 2,

        Local = 3,

    }
    public enum enUso
    {
        Comercial = 1,
        Hogar = 2,
    }
    public enum enEstado
    {
        Disponible = 1,
        NoDisponible = 2,
    }

    public int Id { get; set; }
    public int  Tipo { get; set; }
    public String?  Direccion { get; set; }
    public int  Uso { get; set; }
    public int  CantHamb  { get; set; }
    public String?  Latitud { get; set; }
    public String?  Longitud { get; set; }
    public Double  Precio { get; set; }
    public int  Estado { get; set; }
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
    [NotMapped]
    public string TipoNombre => Tipo > 0 ? ((enTipo)Tipo).ToString() : ""; //deuelve el nombre del id del tipos

    public static IDictionary<int, string> ObtenerTipo() //devuelve la list de los tipos
    {
        SortedDictionary<int, string> tipo = new SortedDictionary<int, string>();
        Type tipoEnumTipo = typeof(enTipo);
        foreach (var valor in Enum.GetValues(tipoEnumTipo))
        {
            tipo.Add((int)valor, Enum.GetName(tipoEnumTipo, valor));
        }
        return tipo;
    }
    [NotMapped]
    public string UsoNombre => Uso > 0 ? ((enUso)Uso).ToString() : ""; //deuelve el nombre del id del uso

    public static IDictionary<int, string> ObtenerUso() //devuelve la list de los uso
    {
        SortedDictionary<int, string> uso = new SortedDictionary<int, string>();
        Type tipoEnumUso = typeof(enUso);
        foreach (var valor in Enum.GetValues(tipoEnumUso))
        {
            uso.Add((int)valor, Enum.GetName(tipoEnumUso, valor));
        }
        return uso;
    }
    public string EstadoNombre => Uso > 0 ? ((enEstado)Estado).ToString() : ""; //deuelve el nombre del id del uso

    public static IDictionary<int, string> ObtenerEstado() //devuelve la list de los uso
    {
        SortedDictionary<int, string> uso = new SortedDictionary<int, string>();
        Type tipoEnumEstado = typeof(enEstado);
        foreach (var valor in Enum.GetValues(tipoEnumEstado))
        {
            uso.Add((int)valor, Enum.GetName(tipoEnumEstado, valor));
        }
        return uso;
    }

}