using MVC.Models;
using MySql.Data.MySqlClient;

namespace mvc.Models;
public class RepositorioPropietario
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioPropietario()
    {

    }
    public int Alta(Propietario propietario){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO propietario (dni,apellido,nombre,telefono,mail) 
            VALUES ( @dni, @apellido,@nombre, @telefono, @mail);
            SELECT LAST_INSERT_ID();" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dni", propietario.Dni); 
                command.Parameters.AddWithValue("@apellido", propietario.Apellido); 
                command.Parameters.AddWithValue("@nombre", propietario.Nombre); 
                command.Parameters.AddWithValue("@telefono", propietario.Telefono); 
                command.Parameters.AddWithValue("@mail", propietario.Mail); 
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }            
        }
        return res;
    }
    public List<Propietario> GetPropietarios()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, dni,apellido,nombre,telefono,mail
            from propietario";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Propietario propietario = new Propietario()
                        {
                            Id = reader.GetInt32(nameof(propietario.Id)),
                            Dni = reader.GetString(nameof(propietario.Dni)),
                            Apellido = reader.GetString(nameof(propietario.Apellido)),
                            Nombre = reader.GetString(nameof(propietario.Nombre)),
                            Telefono = reader.GetString(nameof(propietario.Telefono)),
                            Mail = reader.GetString(nameof(propietario.Mail)),
                            
                            // si fuese fecha seria GetDateTime
                        };
                        propietarios.Add(propietario);  //  add person  to  list     

                    }

                }
            }
            connection.Close();
        }
        return propietarios;
    }

  public Propietario GetPropietario(int id)
    {
        Propietario res = null;
        try{
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, dni,apellido,nombre,telefono,mail
            from propietario
            WHERE Id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                       res = new Propietario()
                        {
                             Id = reader.GetInt32(nameof(Propietario.Id)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Mail = reader.GetString(nameof(Propietario.Mail))
                           
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
    public int Modificar(Propietario propietario){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE propietario 
            SET 
                dni=@dni,
                apellido=@apellido,
                nombre=@nombre,
                telefono=@telefono,
                mail=@mail
                
            WHERE id=@id" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", propietario.Id); 
                command.Parameters.AddWithValue("@dni", propietario.Dni); 
                command.Parameters.AddWithValue("@apellido", propietario.Apellido); 
                command.Parameters.AddWithValue("@nombre", propietario.Nombre); 
                command.Parameters.AddWithValue("@telefono", propietario.Telefono); 
                command.Parameters.AddWithValue("@mail", propietario.Mail); 
                
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
            string query = @"DELETE FROM propietario WHERE Id=@id" ;
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