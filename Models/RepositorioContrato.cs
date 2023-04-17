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
            string query = @"INSERT INTO contrato (fecDesde,fecHasta,idInquilino,idInmueble) 
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
            var query = @"SELECT c.Id, fecDesde,fecHasta,idInquilino,idInmueble,inm.tipo, inm.direccion, inq.nombre, inq.apellido 
            from contrato c INNER JOIN Inmueble inm INNER JOIN Inquilino inq ON c.idInquilino=inq.id and c.idInmueble=inm.id";
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
                            Inquilino1 = new Inquilino()
							{
                                Id = reader.GetInt32(nameof(contrato.IdInquilino)),
								Nombre = reader.GetString(nameof(contrato.Inquilino1.Nombre)),
								Apellido = reader.GetString(nameof(contrato.Inquilino1.Apellido)),
							},
                            IdInmueble = reader.GetInt32(nameof(contrato.IdInmueble)),
                            Inmueble1 = new Inmueble()
							{
                                Id = reader.GetInt32(nameof(contrato.IdInmueble)),
								Tipo = reader.GetInt32(nameof(contrato.Inmueble1.Tipo)),
								Direccion = reader.GetString(nameof(contrato.Inmueble1.Direccion)),
							}
                            
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
            var query = @"SELECT Id, fecDesde,fecHasta,idInquilino,idInmueble
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
                fecDesde=@fecDesde,
                fecHasta=@fecHasta,
                idInquilino=@idInquilino,
                idInmueble=@idInmueble
                
            WHERE id=@id" ;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fecDesde", contrato.FecDesde); 
                command.Parameters.AddWithValue("@fecHasta", contrato.FecHasta); 
                command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino); 
                command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble); 
                command.Parameters.AddWithValue("@id", contrato.Id); 
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }            
        }
        return res;
    }
    public int Eliminar(int id)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"DELETE FROM contrato WHERE Id=@id";
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

    public List<Contrato> GetContListarXfecha(DateTime fechaInicio, DateTime fechaFin)
{
    List<Contrato> contratos = new List<Contrato>();
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = @"SELECT c.Id, fecDesde, fecHasta, idInquilino, idInmueble, inm.tipo, inm.direccion, inq.nombre, inq.apellido 
                        FROM contrato c 
                        INNER JOIN Inmueble inm ON c.idInmueble = inm.id
                        INNER JOIN Inquilino inq ON c.idInquilino = inq.id
                        WHERE (fecDesde <= @fechaInicio AND fecHasta >= @fechaInicio) 
                                 OR (fecDesde <= @fechaFin AND fecHasta >= @fechaFin) 
                                 OR (fecDesde >= @fechaInicio AND fecHasta <= @fechaFin)
                       ";
using (     MySqlCommand command = new MySqlCommand(query, connection))        {
            command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            command.Parameters.AddWithValue("@fechaFin", fechaFin);
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
                        Inquilino1 = new Inquilino()
                        {
                            Id = reader.GetInt32(nameof(contrato.IdInquilino)),
                            Nombre = reader.GetString(nameof(contrato.Inquilino1.Nombre)),
                            Apellido = reader.GetString(nameof(contrato.Inquilino1.Apellido)),
                        },
                        IdInmueble = reader.GetInt32(nameof(contrato.IdInmueble)),
                        Inmueble1 = new Inmueble()
                        {
                            Id = reader.GetInt32(nameof(contrato.IdInmueble)),
                            Tipo = reader.GetInt32(nameof(contrato.Inmueble1.Tipo)),
                            Direccion = reader.GetString(nameof(contrato.Inmueble1.Direccion)),
                        }
                    };
                    contratos.Add(contrato);
                }
            }
        }
    }
    return contratos;
}
    public List<Contrato> GetContratosPorInmueble(int inmuebleId)
{
    List<Contrato> contratos = new List<Contrato>();
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = @"SELECT c.Id, c.fecDesde, c.fecHasta, inq.id AS inquilinoId, inq.nombre AS inquilinoNombre, inq.apellido AS inquilinoApellido 
                    FROM contrato c 
                    INNER JOIN Inquilino inq ON c.idInquilino = inq.id 
                    WHERE c.idInmueble = @inmuebleId;";
        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@inmuebleId", inmuebleId);
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
                        IdInquilino = reader.GetInt32("inquilinoId"),
                        Inquilino1 = new Inquilino()
                        {
                            Id = reader.GetInt32("inquilinoId"),
                            Nombre = reader.GetString("inquilinoNombre"),
                            Apellido = reader.GetString("inquilinoApellido"),
                        }
                    };
                    contratos.Add(contrato);
                }
            }
        }
    }
    return contratos;
}
}