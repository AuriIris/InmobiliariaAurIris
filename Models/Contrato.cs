namespace MVC.Models;
public class Contrato
{
    public int Id { get; set; }
    public DateTime FecDesde { get; set; }
    public DateTime FecHasta { get; set; }

    public int IdInquilino { get; set; }

    public Inquilino Inquilino1 { get; set; }

    public int IdInmueble {get; set;}

    public Inmueble Inmueble1{ get; set; }
    public Contrato()
    {
        
    }
    public override string ToString()
		{
			//return $"{Apellido}, {Nombre}";
			//return $"{Id} {FecDesde},   {FecHasta}, {IdInquilino}, {IdInmueble}";
      return $"{Inquilino1.Apellido},{Inquilino1.Nombre},{Inmueble1.Tipo},{Inmueble1.Direccion}";                                                     
		}
    public bool ContratoVencido(Contrato contrato)
{
    return contrato.FecHasta < DateTime.Today;
}
}