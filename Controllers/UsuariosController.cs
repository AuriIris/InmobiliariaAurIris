using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using mvc.Models;
using MVC.Models;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inmobiliaria_.Net_Core.Controllers
{
	public class UsuariosController : Controller
	{
		private readonly IConfiguration configuration;
		private readonly IWebHostEnvironment environment;
		private readonly RepositorioUsuarios repositorio;
	
		public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment)
		{
			this.configuration = configuration;
			this.environment = environment;
			repositorio = new RepositorioUsuarios();
		}
		
		// GET: Usuarios
		[Authorize(Policy = "Administrador")]
		public ActionResult Index()
		{
			ViewData["Title"] = "Indice";
			var usuarios = repositorio.ObtenerTodos();
			return View(usuarios);
		}

		// GET: Usuarios/Details/5
		[Authorize(Policy = "Administrador")]
		public ActionResult Details(int id)
		{
			var e = repositorio.ObtenerPorId(id);
			return View(e);
		}

		// GET: Usuarios/Create
		[Authorize(Policy = "Administrador")]
		public ActionResult Create()
		{
			ViewBag.Roles = Usuarios.ObtenerRoles();
			return View();
		}

		// POST: Usuarios/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
		public ActionResult Create(Usuarios u)
		{
			
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
								password: u.Clave,
								salt: System.Text.Encoding.ASCII.GetBytes("Super_Secreta_es_la_clave_de_esta_APP_shhh"),
								prf: KeyDerivationPrf.HMACSHA1,
								iterationCount: 1000,
								numBytesRequested: 256 / 8));
				u.Clave = hashed;
				//u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;
				var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
				int res = repositorio.Alta(u);
				// Console.WriteLine("Estoy fuera");
				// Console.WriteLine(u.AvatarFile);
				// Console.WriteLine(u.Id);
				if (u.AvatarFile != null && u.Id > 0)
				{ Console.WriteLine("Estoy dentro");
					string wwwPath = environment.WebRootPath;
					Console.WriteLine(wwwPath);
					string path = Path.Combine(wwwPath, "Uploads");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					//Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
					string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
					string pathCompleto = Path.Combine(path, fileName);
					u.Avatar = Path.Combine("/Uploads", fileName);
					// Esta operación guarda la foto en memoria en la ruta que necesitamos
					using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						u.AvatarFile.CopyTo(stream);
					}
					repositorio.ModificarAvatar(u);
				}
				return RedirectToAction(nameof(Index));
			
		}

		// GET: Usuarios/Edit/5
		[Authorize]
		public ActionResult Perfil()
		{
			ViewData["Title"] = "Mi perfil";
			var u = repositorio.ObtenerPorEmail(User.Identity.Name);
			ViewBag.Roles = Usuarios.ObtenerRoles();
			return View("Edit", u);
		}

		// GET: Usuarios/Edit/5
		[Authorize(Policy = "Administrador")]
		public ActionResult Edit(int id)
		{
			ViewData["Title"] = "Editar usuario";
			var u = repositorio.ObtenerPorId(id);
			ViewBag.Roles = Usuarios.ObtenerRoles();
			return View(u);
		}

		// POST: Usuarios/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult Edit(int id, Usuarios u)
		{
			var vista = nameof(Edit);//de que vista provengo
				if (!User.IsInRole("Administrador"))//no soy admin
				{
					vista = nameof(Perfil);//solo puedo ver mi perfil
					var usuarioActual = repositorio.ObtenerPorEmail(User.Identity.Name);
					if (usuarioActual.Id != id)//si no es admin, solo puede modificarse él mismo
					{
						return RedirectToAction(nameof(Index), "Home");
				}}
				
				repositorio.Modificacion(u);
				
				// TODO: Add update logic here

				return RedirectToAction(vista);
			
		}

		// GET: Usuarios/Delete/5
		[Authorize(Policy = "Administrador")]
		public ActionResult Delete(int id)
		{
			var entidad = repositorio.ObtenerPorId(id);
			return View(entidad);
		}

		// POST: Usuarios/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
		public ActionResult Delete(int id, Usuarios usuario)
		{
			try
			{
				// TODO: Add delete logic here
				var ruta = Path.Combine(environment.WebRootPath, "Uploads", $"avatar_{id}" + Path.GetExtension(usuario.Avatar));
				if (System.IO.File.Exists(ruta))
					System.IO.File.Delete(ruta);
					repositorio.Baja(id);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		[Authorize]
		public IActionResult Avatar()
		{
			var u = repositorio.ObtenerPorEmail(User.Identity.Name);
			string fileName = "avatar_" + u.Id + Path.GetExtension(u.Avatar);
			string wwwPath = environment.WebRootPath;
			string path = Path.Combine(wwwPath, "Uploads");
			string pathCompleto = Path.Combine(path, fileName);

			//leer el archivo
			byte[] fileBytes = System.IO.File.ReadAllBytes(pathCompleto);
			//devolverlo
			return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
		}

		[Authorize]
		public string AvatarBase64()
		{
			var u = repositorio.ObtenerPorEmail(User.Identity.Name);
			string fileName = "avatar_" + u.Id + Path.GetExtension(u.Avatar);
			string wwwPath = environment.WebRootPath;
			string path = Path.Combine(wwwPath, "Uploads");
			string pathCompleto = Path.Combine(path, fileName);

			//leer el archivo
			byte[] fileBytes = System.IO.File.ReadAllBytes(pathCompleto);
			//devolverlo
			return Convert.ToBase64String(fileBytes);
		}

		[Authorize]
		[HttpPost("[controller]/[action]/{fileName}")]
		public IActionResult FromBase64([FromBody] string imagen, [FromRoute] string fileName)
		{
			//arma el path
			string wwwPath = environment.WebRootPath;
			string path = Path.Combine(wwwPath, "Uploads");
			string pathCompleto = Path.Combine(path, fileName);
			//convierto a arreglo de bytes
			var bytes = Convert.FromBase64String(imagen);
			//lo escribe
			System.IO.File.WriteAllBytes(pathCompleto, bytes);
			return Ok();
		}

		[Authorize]
		public ActionResult Foto()
		{
			try
			{
				var u = repositorio.ObtenerPorEmail(User.Identity.Name);
				var stream = System.IO.File.Open(
						Path.Combine(environment.WebRootPath, u.Avatar.Substring(1)),
						FileMode.Open,
						FileAccess.Read);
				var ext = Path.GetExtension(u.Avatar);
				return new FileStreamResult(stream, $"image/{ext.Substring(1)}");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[Authorize]
		public ActionResult Datos()
		{
			try
			{
				var u = repositorio.ObtenerPorEmail(User.Identity.Name);
				string buffer = "Nombre;Apellido;Email" + Environment.NewLine +
						$"{u.Nombre};{u.Apellido};{u.Email}";
				var stream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(buffer));
				var res = new FileStreamResult(stream, "text/plain");
				res.FileDownloadName = "Datos.csv";
				return res;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		// GET: Usuarios/Login/
		public ActionResult LoginModal()
		{
			return PartialView("_LoginModal", new LoginView());
		}

		[AllowAnonymous]
		// GET: Usuarios/Login/
		public ActionResult Login(string returnUrl)
		{
			TempData["returnUrl"] = returnUrl;
			return View();
		}

		// POST: Usuarios/Login/
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginView login)
		{
			try
			{
				var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
				if (ModelState.IsValid)
				{
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: login.Clave,
						salt: System.Text.Encoding.ASCII.GetBytes("Super_Secreta_es_la_clave_de_esta_APP_shhh"),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));

					var e = repositorio.ObtenerPorEmail(login.Usuarios);
					if (e == null || e.Clave != hashed)
					{
						ModelState.AddModelError("", "El email o la clave no son correctos");
						TempData["returnUrl"] = returnUrl;
						return View();
					}

					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, e.Email),
						new Claim("FullName", e.Nombre + " " + e.Apellido),
						new Claim(ClaimTypes.Role, e.RolNombre),
					};

					var claimsIdentity = new ClaimsIdentity(
							claims, CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(
							CookieAuthenticationDefaults.AuthenticationScheme,
							new ClaimsPrincipal(claimsIdentity));
					TempData.Remove("returnUrl");
					return Redirect(returnUrl);
				}
				TempData["returnUrl"] = returnUrl;
				return View();
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View();
			}
		}

		// GET: /salir
		[Route("salir", Name = "Logout")]
		public async Task<ActionResult> Logout()
		{
			await HttpContext.SignOutAsync(
					CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}
		[Route("CambiarPass", Name = "CambiarPass")]
		public ActionResult CambiarPass(int id, CambioClaveView cambio)
		{
			Usuarios u = null;
			try
			{
				// recuperar u original
				u = repositorio.ObtenerPorId(id);
				// verificar clave antigüa
				var pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
								password: cambio.ClaveVieja ?? "",
								salt: System.Text.Encoding.ASCII.GetBytes("Super_Secreta_es_la_clave_de_esta_APP_shhh"),
								prf: KeyDerivationPrf.HMACSHA1,
								iterationCount: 1000,
								numBytesRequested: 256 / 8));
				if (u.Clave != pass)
				{
					TempData["Error"] = "Clave incorrecta";
					// se rederige porque no hay vista de cambio de pass, está compartida con Edit
					return RedirectToAction("Edit", new { id = id });
				}
				if (ModelState.IsValid)
				{
					u.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: cambio.ClaveNueva,
							salt: System.Text.Encoding.ASCII.GetBytes("Super_Secreta_es_la_clave_de_esta_APP_shhh"),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));
					repositorio.ModificarClave(u);
					TempData["Mensaje"] = "Contraseña actualizada correctamente";
					return RedirectToAction(nameof(Index));
				}
				else//estado inválido
				{//pasaje de los errores del modelstate a un string en tempData
					foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
					{
						foreach (ModelError error in modelState.Errors)
						{
							TempData["Error"] += error.ErrorMessage + "\n";
						}
					}
					return RedirectToAction("Edit", new { id = id });
				}
			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;
				TempData["StackTrace"] = ex.StackTrace;
				return RedirectToAction("Edit", new { id = id });
			}
		}
		[Route("CambiarAvatar", Name = "CambiarAvatar")]
		public ActionResult CambiarAvatar(int id, CambiarAvatar cambio)
		{
			Usuarios u = repositorio.ObtenerPorId(id);
			
			if (u != null &&  u.Id > 0)
				{ 	u.AvatarFile=cambio.AvatarFile;
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
					string pathCompleto = Path.Combine(path, fileName);
					u.Avatar = Path.Combine("/Uploads", fileName);
					// Esta operación guarda la foto en memoria en la ruta que necesitamos
					using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						u.AvatarFile.CopyTo(stream);
					}
					repositorio.ModificarAvatar(u);
				}
			return RedirectToAction("Index", "Usuarios");
		}

	
	}
	}
