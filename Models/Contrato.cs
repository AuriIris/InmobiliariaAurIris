namespace MVC.Models;
public class Contrato
{
    public int Id { get; set; }
    public DateTime FecDesde { get; set; }
    public DateTime FecHasta { get; set; }

    public int IdInquilino { get; set; }

    public int IdInmueble {get; set;}

    public Contrato()
    {
        
    }
}