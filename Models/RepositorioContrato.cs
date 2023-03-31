using MVC.Models;
using MySql.Data.MySqlClient;

namespace mvc.Models;
public class RepositorioContrato
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioContrato()
    {

    }
    public int Alta(Contrato contrato){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO contrato (fecDes,fecHas,idInquilino,idInmueble) 
            VALUES (@fecDes,@fecHas,@idInquilino,@idInmueble);
            SELECT LAST_INSERT_ID();" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fecDes", contrato.FecDesde); 
                command.Parameters.AddWithValue("@fecHas", contrato.FecHasta); 
                command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino); 
                command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble); 
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }            
        }
        return res;
    }
    public List<Contrato> GetContratos()
    {
        List<Contrato> contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, fecDes,fecHas,idInquilino,idInmueble
            from contrato";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato()
                        {
                            Id = reader.GetInt32(nameof(contrato.Id)),
                            FecDesde = reader.GetDateTime(nameof(contrato.FecDesde)),
                            FecHasta = reader.GetDateTime(nameof(contrato.FecHasta)),
                            IdInquilino = reader.GetInt32(nameof(contrato.IdInquilino)),
                            IdInmueble = reader.GetInt32(nameof(contrato.IdInmueble)),
                            
                            // si fuese fecha seria GetDateTime
                        };
                        contratos.Add(contrato);  //  add person  to  list     

                    }

                }
            }
            connection.Close();
        }
        return contratos;
    }

  public Contrato GetContrato(int id)
    {
        Contrato res = null;
        try{
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, fecDes,fecHas,idInquilino,idInmueble
            from contrato
            WHERE Id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                       res = new Contrato()
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            FecDesde = reader.GetDateTime(nameof(Contrato.FecDesde)),
                            FecHasta = reader.GetDateTime(nameof(Contrato.FecHasta)),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                           
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
    public int Modificar(Contrato contrato){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE contrato 
            SET 
                fecDes=@fecDes,
                fecHas=@fecHas,
                idInquilino=@idInquilino,
                idInmueble=@idInmueble
                
            WHERE id=@id" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fecDes", contrato.FecDesde); 
                command.Parameters.AddWithValue("@fecHas", contrato.FecHasta); 
                command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino); 
                command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble); 
                
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
            string query = @"DELETE FROM contrato WHERE Id=@id" ;
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