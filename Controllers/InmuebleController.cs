using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Index()
        {
            var lista = Repo.GetInmuebles();
            if (TempData.ContainsKey("Id"))
				ViewBag.Id = TempData["Id"];
			if (TempData.ContainsKey("Mensaje"))
				ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: Inmueble/Details/5
        public ActionResult Details(int id)
        {
             var entidad = Repo.GetInmueble(id);
            return View(entidad);
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
           try
			{
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
        public ActionResult Create(Inmueble inmueble)
        {
           try
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
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View(inmueble);
			}
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            var entidad = Repo.GetInmueble(id);
			ViewBag.Propietarios = repoPropietario.GetPropietarios();
			if (TempData.ContainsKey("Mensaje"))
				ViewBag.Mensaje = TempData["Mensaje"];
			if (TempData.ContainsKey("Error"))
				ViewBag.Error = TempData["Error"];
			return View(entidad);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                inmueble.Id = id;
				Repo.Modificar(inmueble);
				TempData["Mensaje"] = "Datos guardados correctamente";
				return RedirectToAction(nameof(Index));
            }
            catch  (Exception ex)
            {
                ViewBag.Propietarios = repoPropietario.GetPropietarios();
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View(inmueble);
            }
        }

        // GET: Inmueble/Delete/5
        public ActionResult Delete(int id)
        {
            var entidad = Repo.GetInmueble(id);
            return View(entidad);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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