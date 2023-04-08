using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVC.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
namespace mvc.Models;

	public class RepositorioUsuarios 
	{
            string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
		public RepositorioUsuarios() 
		{

		}

		public int Alta(Usuarios e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string query = @"INSERT INTO Usuario 
					(Nombre, Apellido, Avatar, Mmail, Clave, Rol) 
					VALUES (@nombre, @apellido, @avatar, @mail, @clave, @rol);
					LAST_INSERT_ID para mysql";
				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					if (String.IsNullOrEmpty(e.Avatar))
						command.Parameters.AddWithValue("@avatar", DBNull.Value);
					else
						command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@mail", e.Email);
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@rol", e.Rol);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.Id = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string query = "DELETE FROM Usuario WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Usuarios e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string query = @"UPDATE Usuario
					SET Nombre=@nombre, Apellido=@apellido, Avatar=@avatar, Mail=@mail, Clave=@clave, Rol=@rol
					WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@mail", e.Email);
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@rol", e.Rol);
					command.Parameters.AddWithValue("@id", e.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Usuarios> ObtenerTodos()
		{
			IList<Usuarios> res = new List<Usuarios>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string query = @"
					SELECT Id, Nombre, Apellido, Avatar, mail, Clave, Rol
					FROM Usuario";
				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usuarios e = new Usuarios
						{
							Id = reader.GetInt32("Id"),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Avatar = reader.GetString("Avatar"),
							Email = reader.GetString("Mail"),
							Clave = reader.GetString("Clave"),
							Rol = reader.GetInt32("Rol"),
						};
						res.Add(e);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Usuarios ObtenerPorId(int id)
		{
			Usuarios? e = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string query = @"SELECT 
					Id, Nombre, Apellido, Avatar, Mail, Clave, Rol 
					FROM Usuario
					WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@id", id);
                    connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuarios
						{
							Id = reader.GetInt32("Id"),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Avatar = reader.GetString("Avatar"),
							Email = reader.GetString("Mail"),
							Clave = reader.GetString("Clave"),
							Rol = reader.GetInt32("Rol"),
						};
					}
					connection.Close();
				}
			}
			return e;
		}

		public Usuarios ObtenerPorEmail(string email)
		{
			Usuarios? e = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string query = @"SELECT
					Id, Nombre, Apellido, Avatar, Mail, Clave, Rol FROM Usuario
					WHERE Mail=@mail";
				using (MySqlCommand command = new MySqlCommand(query, connection))
				{   
                    command.Parameters.AddWithValue("@mail", email);
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuarios
						{
							Id = reader.GetInt32("Id"),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Avatar = reader.GetString("Avatar"),
							Email = reader.GetString("Mail"),
							Clave = reader.GetString("Clave"),
							Rol = reader.GetInt32("Rol"),
						};
					}
					connection.Close();
				}
			}
			return e;
		}
	}

