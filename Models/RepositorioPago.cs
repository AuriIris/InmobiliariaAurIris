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
            var query = @"SELECT Id, monto,fecha,idContrato
            from pago";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pago pago = new Pago()
                        {
                            Id = reader.GetInt32(nameof(pago.Id)),
                            Monto = reader.GetDouble(nameof(pago.Monto)),
                            Fecha = reader.GetDateTime(nameof(pago.Fecha)),
                            IdContrato = reader.GetInt32(nameof(pago.IdContrato))
                            
                            
                            // si fuese fecha seria GetDateTime
                        };
                        pagos.Add(pago);  //  add person  to  list     

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
            var query = @"SELECT Id, monto,fecha,idContrato
            from pago
            WHERE Id = @id";
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