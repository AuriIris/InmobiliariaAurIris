using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using MVC.Models;

namespace MVC.Controllers
{
    public class InmuebleController : Controller
    {   
        private readonly ReposotorioInmueble Repo;
        private readonly RepositorioPropietario repoPropietario;
        public InmuebleController()
        {
            Repo = new ReposotorioInmueble();
            repoPropietario = new RepositorioPropietario();
        }
        // GET: Inmueble
        [Authorize]
        public ActionResult Index()
        {
            var lista = Repo.GetInmuebles();
        
            return View(lista);
        }
        [Authorize]
        public ActionResult VerInm(int id)
        {
            var lista = Repo.GetInmueblesXProp(id);
        
            return View(lista);
        }
        [Authorize]
        public ActionResult Disponibles(DateTime fecha )
        {
            var hoy=fecha;

            if(fecha==null){
                hoy = DateTime.Today;
            }

            var lista = Repo.GetInmueblesDisponibles(hoy);
            
        
            return View(lista);
        }
 
        // GET: Inmueble/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
             var entidad = Repo.GetInmueble(id);
            return View(entidad);
        }

        // GET: Inmueble/Create
        [Authorize]
        public ActionResult Create()
        {
           try
			{
                ViewBag.Tipos = Inmueble.ObtenerTipo();
                ViewBag.Usos = Inmueble.ObtenerUso();
                 ViewBag.Estados = Inmueble.ObtenerEstado();
				ViewBag.Propietarios = repoPropietario.GetPropietarios();
				return View();
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
				
                if (ModelState.IsValid)
				{
					Repo.Alta(inmueble);
					TempData["Id"] = inmueble.Id;
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ViewBag.Propietarios = repoPropietario.GetPropietarios();
					return View(inmueble);
				}
					
				
			
        }
        // GET: Inmueble/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var entidad = Repo.GetInmueble(id);
            ViewBag.Tipos = Inmueble.ObtenerTipo();
            ViewBag.Usos = Inmueble.ObtenerUso();
            ViewBag.Estados = Inmueble.ObtenerEstado();
			ViewBag.Propietarios = repoPropietario.GetPropietarios();
			//if (TempData.ContainsKey("Mensaje"))
			//	ViewBag.Mensaje = TempData["Mensaje"];
			//if (TempData.ContainsKey("Error"))
			//	ViewBag.Error = TempData["Error"];
			return View(entidad);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            
                inmueble.Id = id;
				Repo.Modificar(inmueble);
				TempData["Mensaje"] = "Datos guardados correctamente";
				return RedirectToAction(nameof(Index));
            
                
            }
        

        // GET: Inmueble/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var entidad = Repo.GetInmueble(id);
            return View(entidad);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
                // TODO: Add delete logic here

                Repo.Eliminar(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}