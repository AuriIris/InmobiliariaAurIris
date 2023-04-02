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
    public class PagoController : Controller
    {    private readonly RepositorioContrato repoCon;
        private readonly RepositorioPago repoPago;
        public PagoController()
        {
            repoCon= new RepositorioContrato();
            repoPago = new RepositorioPago();
        }
        // GET: Pago
        public ActionResult Index()
        {
            var lista = repoPago.GetPagos(); 
        
            return View(lista);
        }

        // GET: Pago/Details/5
        public ActionResult Details(int id)
        {
            var entidad = repoPago.GetPago(id);
            return View(entidad);
        }

        // GET: Pago/Create
        public ActionResult Create()
        {
             try
			{
				ViewBag.Contratos = repoCon.GetContratos();
				return View();
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            
                if (ModelState.IsValid)
				{
					repoPago.Alta(pago);
					TempData["Id"] = pago.Id;
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ViewBag.Contratos = repoCon.GetContratos();
					return View(pago);
				}
        }

        // GET: Pago/Edit/5
        public ActionResult Edit(int id)
        {
             var entidad = repoPago.GetPago(id);
			ViewBag.Contratos = repoCon.GetContratos();
			if (TempData.ContainsKey("Mensaje"))
				ViewBag.Mensaje = TempData["Mensaje"];
			if (TempData.ContainsKey("Error"))
				ViewBag.Error = TempData["Error"];
			return View(entidad);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago pago)
        {
                pago.Id = id;
				repoPago.Modificar(pago);
				TempData["Mensaje"] = "Datos guardados correctamente";
				return RedirectToAction(nameof(Index));
        }

        // GET: Pago/Delete/5
        public ActionResult Delete(int id)
        {
            var entidad = repoPago.GetPago(id);
            return View(entidad);
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                repoPago.Eliminar(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}