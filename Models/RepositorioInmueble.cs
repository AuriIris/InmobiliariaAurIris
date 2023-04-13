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
            string query = @"INSERT INTO inmueble (tipo,direccion,uso,cantHamb,latitud,longitud,precio,estado,idPropietario) 
            VALUES (@tipo,@direccion,@uso,@cantHamb,@latitud,@longitud,@precio,@estado,@idPropietario);
            SELECT LAST_INSERT_ID();" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tipo", inmueble.Tipo); 
                command.Parameters.AddWithValue("@direccion", inmueble.Direccion); 
                command.Parameters.AddWithValue("@uso", inmueble.Uso); 
                command.Parameters.AddWithValue("@cantHamb", inmueble.CantHamb); 
                command.Parameters.AddWithValue("@latitud", inmueble.Latitud); 
                command.Parameters.AddWithValue("@longitud", inmueble.Longitud); 
                command.Parameters.AddWithValue("@precio", inmueble.Precio); 
                command.Parameters.AddWithValue("@estado", inmueble.Estado); 
                command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario); 
                
                connection.Open();
                int v = Convert.ToInt32(command.ExecuteScalar());
                res = v;
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
            var query = @"SELECT i.id,tipo,direccion,uso,cantHamb,latitud,longitud,precio,estado,idPropietario, p.nombre, p.apellido 
                FROM Inmueble i INNER JOIN Propietario p ON i.idPropietario = p.id";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble()
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            CantHamb = reader.GetInt32(nameof(Inmueble.CantHamb)),
                            Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                            Precio = reader.GetDouble(nameof(Inmueble.Precio)),
                            Estado = reader.GetInt32(nameof(Inmueble.Estado)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Duenio = new Propietario()
							{
                                Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
								Nombre = reader.GetString(nameof(Inmueble.Duenio.Nombre)),
								Apellido = reader.GetString(nameof(Inmueble.Duenio.Apellido)),
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
            var query = @"SELECT i.id,tipo,direccion,uso,cantHamb,latitud,longitud,precio,estado,idPropietario, p.nombre, p.apellido
            from inmueble  i JOIN Propietario p ON i.idPropietario = p.id
            WHERE i.id = @id";
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
                            Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            CantHamb = reader.GetInt32(nameof(Inmueble.CantHamb)),
                            Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                            Precio = reader.GetDouble(nameof(Inmueble.Precio)),
                            Estado = reader.GetInt32(nameof(Inmueble.Estado)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Duenio = new Propietario()
							{
                                Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
								Nombre = reader.GetString(nameof(Inmueble.Duenio.Nombre)),
								Apellido = reader.GetString(nameof(Inmueble.Duenio.Nombre)),
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
                estado=@estado,
                idPropietario=@idPropietario
                
            WHERE id=@id" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tipo", inmueble.Tipo); 
                command.Parameters.AddWithValue("@direccion", inmueble.Direccion); 
                command.Parameters.AddWithValue("@uso", inmueble.Uso); 
                command.Parameters.AddWithValue("@cantHamb", inmueble.CantHamb); 
                command.Parameters.AddWithValue("@latitud", inmueble.Latitud); 
                command.Parameters.AddWithValue("@longitud", inmueble.Longitud); 
                command.Parameters.AddWithValue("@precio", inmueble.Precio); 
                command.Parameters.AddWithValue("@estado", inmueble.Estado); 
                command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);
                command.Parameters.AddWithValue("@id", inmueble.Id); 
                
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
    public List<Inmueble> GetInmuxEstado(string estado)
{
    List<Inmueble> inmuebles = new List<Inmueble>();
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = @"SELECT i.id,tipo,direccion,uso,cantHamb,latitud,longitud,precio,estado,idPropietario, p.nombre, p.apellido 
            FROM Inmueble i INNER JOIN Propietario p ON i.idPropietario = p.id WHERE estado = @estado";
        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@estado", estado);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Inmueble inmueble = new Inmueble()
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                        CantHamb = reader.GetInt32(nameof(Inmueble.CantHamb)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        Precio = reader.GetDouble(nameof(Inmueble.Precio)),
                        Estado = reader.GetInt32(nameof(Inmueble.Estado)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        Duenio = new Propietario()
                        {
                            Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Nombre = reader.GetString(nameof(Inmueble.Duenio.Nombre)),
                            Apellido = reader.GetString(nameof(Inmueble.Duenio.Apellido)),
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
    public List<Inmueble> GetInmueblesDisponibles(DateTime fecha)
    {
        List<Inmueble> inmueblesDisponibles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT i.id, tipo, direccion, uso, cantHamb, latitud, longitud, precio, estado, idPropietario, p.nombre, p.apellido 
                            FROM Inmueble i 
                            INNER JOIN Propietario p ON i.idPropietario = p.id 
                            WHERE i.Estado = 1 AND i.Id NOT IN (
                                SELECT c.IdInmueble
                                FROM contrato c
                                WHERE (c.fecDesde <= @fecha AND c.fecHasta >= @fecha)
                            )";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fecha", fecha);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble()
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                        CantHamb = reader.GetInt32(nameof(Inmueble.CantHamb)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        Precio = reader.GetDouble(nameof(Inmueble.Precio)),
                        Estado = reader.GetInt32(nameof(Inmueble.Estado)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        Duenio = new Propietario()
                        {
                            Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Nombre = reader.GetString(nameof(Inmueble.Duenio.Nombre)),
                            Apellido = reader.GetString(nameof(Inmueble.Duenio.Apellido)),
                        }
                        };
                        inmueblesDisponibles.Add(inmueble);
                    }
                }
            }
        }
        return inmueblesDisponibles;
    }
    public List<Inmueble> GetInmueblesDisp()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT i.id, tipo, direccion, uso, cantHamb, latitud, longitud, precio, estado, idPropietario, p.nombre, p.apellido 
                            FROM Inmueble i 
                            INNER JOIN Propietario p ON i.idPropietario = p.id
                            WHERE Estado = 1";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble()
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            CantHamb = reader.GetInt32(nameof(Inmueble.CantHamb)),
                            Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                            Precio = reader.GetDouble(nameof(Inmueble.Precio)),
                            Estado = reader.GetInt32(nameof(Inmueble.Estado)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Duenio = new Propietario()
							{
                                Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
								Nombre = reader.GetString(nameof(Inmueble.Duenio.Nombre)),
								Apellido = reader.GetString(nameof(Inmueble.Duenio.Apellido)),
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
     public List<Inmueble> GetInmueblesXProp(int Prop)
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT i.id, tipo, direccion, uso, cantHamb, latitud, longitud, precio, estado, idPropietario, p.nombre, p.apellido 
                            FROM Inmueble i 
                            INNER JOIN Propietario p ON i.idPropietario = p.id
                            WHERE idPropietario = @idPropietario";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idPropietario", Prop);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble()
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            CantHamb = reader.GetInt32(nameof(Inmueble.CantHamb)),
                            Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                            Precio = reader.GetDouble(nameof(Inmueble.Precio)),
                            Estado = reader.GetInt32(nameof(Inmueble.Estado)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Duenio = new Propietario()
							{
                                Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
								Nombre = reader.GetString(nameof(Inmueble.Duenio.Nombre)),
								Apellido = reader.GetString(nameof(Inmueble.Duenio.Apellido)),
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



}