using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MVC.Models;


public enum enRoles
{
    
    Administrador = 1,
    Empleado = 2,
}

public class Usuarios
{
    [Key]
    [Display(Name = "Código")]
    public int Id { get; set; }
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Apellido { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Clave { get; set; }
    public string Avatar { get; set; } // es la ruta donde va a estar guardada la foto, es lo que se garda en la BD
    [NotMapped]//Para EF
    public IFormFile AvatarFile { get; set; }
    //[NotMapped]//Para EF
    //public byte[] AvatarFileContent { get; set; }
    //[NotMapped]//Para EF
    //public string AvatarFileName { get; set; }
    public int Rol { get; set; }
    [NotMapped]//Para EF
    public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : ""; //deuelve el nombre del id del Rol

    public static IDictionary<int, string> ObtenerRoles() //devuelve la list de los roles
    {
        SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
        Type tipoEnumRol = typeof(enRoles);
        foreach (var valor in Enum.GetValues(tipoEnumRol))
        {
            roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
        }
        return roles;
    }
}
