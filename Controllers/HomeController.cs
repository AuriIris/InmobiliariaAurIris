using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [Authorize]
		public ActionResult Seguro()
		{
			var identity = (ClaimsIdentity)User.Identity;
			IEnumerable<Claim> claims = identity.Claims;
			return View(claims);
		}

		[Authorize(Policy = "Administrador")]
		public ActionResult Admin()
		{
			return View();
		}
        
		public ActionResult Restringido()
		{
			return View();
		}

		[Authorize]
		public async Task<ActionResult> CambiarClaim()
		{
			var identity = (ClaimsIdentity)User.Identity;
			identity.RemoveClaim(identity.FindFirst("FullName"));
			identity.AddClaim(new Claim("FullName", "Cosme Fulanito"));
			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(identity));
			return Redirect(nameof(Seguro));
		}




    public IActionResult Privacy()
    {
        return View();
    }
     public IActionResult Ver()

    {
        Persona persona=new Persona
        {
            Nombre="Juan"
        };

        return View(persona);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
