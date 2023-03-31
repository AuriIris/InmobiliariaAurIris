using MVC.Models;
using MySql.Data.MySqlClient;

namespace mvc.Models;
public class ReposotorioInmueble
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public ReposotorioInmueble()
    {

    }
    public int Alta(Inmueble inmueble){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO inmueble (tipo,direccion,uso,cantHamb,latitud,longitud,precio,disponible,idPropietario) 
            VALUES (@tipo,@direccion,@uso,@cantHamb,@latitud,@longitud,@precio,@disponible,@idPropietario);
            SELECT LAST_INSERT_ID();" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tipo", inmueble.Tipo); 
                command.Parameters.AddWithValue("@direccion", inmueble.Direccion); 
                command.Parameters.AddWithValue("@uso", inmueble.Uso); 
                command.Parameters.AddWithValue("@cantHab", inmueble.CantHamb); 
                command.Parameters.AddWithValue("@latitud", inmueble.Latitud); 
                command.Parameters.AddWithValue("@longitud", inmueble.Longitud); 
                command.Parameters.AddWithValue("@precio", inmueble.Precio); 
                command.Parameters.AddWithValue("@disponible", inmueble.Disponible); 
                command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario); 
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }            
        }
        return res;
    }
    public List<Inmueble> GetInmuebles()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id,tipo,direccion,uso,cantHamb,latitud,longitud,precio,disponible,idPropietario"+" p.Nombre, p.Apellido" +
					" FROM Inmuebles i INNER JOIN Propietario p ON i.PropietarioId = p.IdPropietario";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble()
                        {
                            Id = reader.GetInt32(0),
                            Tipo = reader.GetString(1),
                            Direccion = reader.GetString(2),
                            Uso = reader.GetString(3),
                            CantHamb = reader.GetInt32(4),
                            Latitud = reader.GetString(5),
                            Longitud = reader.GetString(6),
                            Precio = reader.GetDouble(7),
                            Disponible = reader.GetString(8),
                            IdPropietario = reader.GetInt32(9),
                            Duenio = new Propietario()
							{
                                Id = reader.GetInt32(9),
								Nombre = reader.GetString(10),
								Apellido = reader.GetString(11),
							}
                            // si fuese fecha seria GetDateTime
                        };
                        inmuebles.Add(inmueble);  //  add person  to  list     

                    }

                }
            }
            connection.Close();
        }
        return inmuebles;
    }

  public Inmueble GetInmueble(int id)
    {
        Inmueble res = null;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id,tipo,direccion,uso,cantHamb,latitud,longitud,precio,disponible,idPropietario
            from inmueble  i JOIN Propietario p ON i.idPropietario = p.id
            WHERE Id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                       res = new Inmueble()
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            CantHamb = reader.GetInt32(nameof(Inmueble.CantHamb)),
                            Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                            Precio = reader.GetDouble(nameof(Inmueble.Precio)),
                            Disponible = reader.GetString(nameof(Inmueble.Disponible)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Duenio = new Propietario
							{
								Id = reader.GetInt32("PropietarioId"),
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido"),
							}
                           
                            // si fuese fecha seria GetDateTime
                        };

                    }
                }
            }
            connection.Close();
        }
        return res;
    }
    public int Modificar(Inmueble inmueble){
        int res =0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE inmueble 
            SET 
                tipo=@tipo,
                direccion=@direccion,
                uso=@uso,
                cantHamb=@cantHamb,
                latitud=@latitud,
                longitud=@longitud,
                precio=@precio,
                disponible=@disponible,
                idPropietario=@idPropietario
                
            WHERE id=@id" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tipo", inmueble.Tipo); 
                command.Parameters.AddWithValue("@direccion", inmueble.Direccion); 
                command.Parameters.AddWithValue("@uso", inmueble.Uso); 
                command.Parameters.AddWithValue("@cantHab", inmueble.CantHamb); 
                command.Parameters.AddWithValue("@latitud", inmueble.Latitud); 
                command.Parameters.AddWithValue("@longitud", inmueble.Longitud); 
                command.Parameters.AddWithValue("@precio", inmueble.Precio); 
                command.Parameters.AddWithValue("@disponible", inmueble.Disponible); 
                command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);
                
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
            string query = @"DELETE FROM inmueble WHERE Id=@id" ;
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