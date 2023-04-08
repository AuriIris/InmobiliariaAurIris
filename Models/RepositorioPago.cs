using MVC.Models;
using MySql.Data.MySqlClient;

namespace mvc.Models;
public class RepositorioPago
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioPago()
    {

    }
    public int Alta(Pago pago){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO pago (monto,fecha,idContrato) 
            VALUES (@monto,@fecha,@idContrato);
            SELECT LAST_INSERT_ID();" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@monto", pago.Monto); 
                command.Parameters.AddWithValue("@fecha", pago.Fecha); 
                command.Parameters.AddWithValue("@idContrato", pago.IdContrato); 
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }            
        }
        return res;
    }
    public List<Pago> GetPagos()
{
    List<Pago> pagos = new List<Pago>();
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = @"SELECT pago.id, pago.fecha, pago.monto,pago.idContrato, contrato.idInmueble, inmueble.tipo, inmueble.direccion, contrato.idInquilino, inquilino.nombre, inquilino.apellido
                    FROM pago
                    JOIN contrato ON pago.idContrato = contrato.id
                    JOIN inmueble ON contrato.idInmueble = inmueble.id
                    JOIN inquilino ON contrato.idInquilino = inquilino.id;";

        using (var command = new MySqlCommand(query, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Pago pago = new Pago()
                    {
                        Id = reader.GetInt32("id"),
                        Monto = reader.GetDouble("monto"),
                        Fecha = reader.GetDateTime("fecha"),
                        IdContrato = reader.GetInt32("idContrato"),
                        Contrato1 = new Contrato(){
                            Id = reader.GetInt32("idContrato"),
                            Inmueble1= new Inmueble(){
                                Id = reader.GetInt32("idInmueble"),
                                Tipo = reader.GetInt32("tipo"),
                                Direccion = reader.GetString("direccion"),

                            },
                            Inquilino1= new Inquilino(){
                                Id = reader.GetInt32("idInquilino"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido")
                            }
                            
                        }

                        
                    };
                    pagos.Add(pago);  // add pago to list
                }
            }
        }
        connection.Close();
    }
    return pagos;
}
  public Pago GetPago(int id)
    {
        Pago res = null;
        try{
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   

            var query = @"SELECT pago.Id,pago.fecha, pago.monto,pago.idContrato
                        FROM pago
                        ";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                       res = new Pago()
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            Monto = reader.GetDouble(nameof(Pago.Monto)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato))
                           
                            // si fuese fecha seria GetDateTime
                        };

                    }

                }
            }
            connection.Close();}
        }
        catch(Exception ex){
            Console.WriteLine(ex);
        }
        return res;
    }
    public int Modificar(Pago pago){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE pago 
            SET 
                monto=@monto,
                fecha=@fecha,
                idContrato=@idContrato
                
            WHERE id=@id" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@monto", pago.Monto); 
                command.Parameters.AddWithValue("@fecha", pago.Fecha); 
                command.Parameters.AddWithValue("@idContrato", pago.IdContrato); 
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }            
        }
        return res;
    }
     public int Eliminar(int id ){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"DELETE FROM pago WHERE Id=@id" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id); 
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }            
        }
        return res;
    }

}