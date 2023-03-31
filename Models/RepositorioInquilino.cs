using MVC.Models;
using MySql.Data.MySqlClient;

namespace mvc.Models;
public class ReposotorioInquilino
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public ReposotorioInquilino()
    {

    }
    public int Alta(Inquilino inquilino){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO inquilino (dni,apellido,nombre,telefono,mail) 
            VALUES ( @dni, @apellido,@nombre, @telefono, @mail);
            SELECT LAST_INSERT_ID();" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dni", inquilino.Dni); 
                command.Parameters.AddWithValue("@apellido", inquilino.Apellido); 
                command.Parameters.AddWithValue("@nombre", inquilino.Nombre); 
                command.Parameters.AddWithValue("@telefono", inquilino.Telefono); 
                command.Parameters.AddWithValue("@mail", inquilino.Mail); 
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }            
        }
        return res;
    }
    public List<Inquilino> GetInquilinos()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, dni,apellido,nombre,telefono,mail
            from inquilino";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino inquilino = new Inquilino()
                        {
                            Id = reader.GetInt32(nameof(inquilino.Id)),
                            Dni = reader.GetString(nameof(inquilino.Dni)),
                            Apellido = reader.GetString(nameof(inquilino.Apellido)),
                            Nombre = reader.GetString(nameof(inquilino.Nombre)),
                            Telefono = reader.GetString(nameof(inquilino.Telefono)),
                            Mail = reader.GetString(nameof(inquilino.Mail)),
                            
                            // si fuese fecha seria GetDateTime
                        };
                        inquilinos.Add(inquilino);  //  add person  to  list     

                    }

                }
            }
            connection.Close();
        }
        return inquilinos;
    }

  public Inquilino GetInquilino(int id)
    {
        Inquilino res = null;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id, dni,apellido,nombre,telefono,mail
            from inquilino
            WHERE Id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                       res = new Inquilino()
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Dni = reader.GetString(nameof(Inquilino.Dni)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Mail = reader.GetString(nameof(Inquilino.Mail))
                           
                            // si fuese fecha seria GetDateTime
                        };

                    }
                }
            }
            connection.Close();
        }
        return res;
    }
    public int Modificar(Inquilino inquilino){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE inquilino 
            SET 
                dni=@dni,
                apellido=@apellido,
                nombre=@nombre,
                telefono=@telefono,
                mail=@mail
                
            WHERE id=@id" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", inquilino.Id); 
                command.Parameters.AddWithValue("@dni", inquilino.Dni); 
                command.Parameters.AddWithValue("@apellido", inquilino.Apellido); 
                command.Parameters.AddWithValue("@nombre", inquilino.Nombre); 
                command.Parameters.AddWithValue("@telefono", inquilino.Telefono); 
                command.Parameters.AddWithValue("@mail", inquilino.Mail); 
                
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
            string query = @"DELETE FROM inquilino WHERE Id=@id" ;
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