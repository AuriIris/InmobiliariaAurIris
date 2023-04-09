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
    public class ContratoController : Controller
    {
        private readonly ReposotorioInmueble repoInm ;
        private readonly RepositorioContrato repoCon;
        private readonly ReposotorioInquilino repoInq;
        public ContratoController()
        {
            repoInm = new ReposotorioInmueble();
            repoCon = new RepositorioContrato();
            repoInq = new ReposotorioInquilino();
        }
        // GET: Contrato
        [Authorize]
        public ActionResult Index()
        {
            var lista = repoCon.GetContratos();
        
            return View(lista);
        }

        // GET: Contrato/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
             var entidad = repoCon.GetContrato(id);
            return View(entidad);
        }

        // GET: Contrato/Create
        [Authorize]
         public ActionResult Create()
        {
          
				ViewBag.Inquilinos = repoInq.GetInquilinos();
                ViewBag.Inmuebles = repoInm.GetInmuebles();
				return View();
			
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato contrato)
        {
           
					repoCon.Alta(contrato);
					TempData["Id"] = contrato.Id;
					return RedirectToAction(nameof(Index));
				
        }

        // GET: Contrato/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
             var entidad = repoCon.GetContrato(id);
			ViewBag.Inmuebles = repoInm.GetInmuebles();
            ViewBag.Inquilinos = repoInq.GetInquilinos();
			return View(entidad);
        }

        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato contrato)
        {
            
                contrato.Id = id;
				repoCon.Modificar(contrato);
				TempData["Mensaje"] = "Datos guardados correctamente";
				return RedirectToAction(nameof(Index));
        }

        // GET: Contrato/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var entidad = repoCon.GetContrato(id);
            return View(entidad);
        }

        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                repoCon.Eliminar(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}