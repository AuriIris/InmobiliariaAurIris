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
         public ActionResult Create(int IdInmueble)
        {
            ViewBag.Inquilinos = repoInq.GetInquilinos();
            ViewBag.Inmuebles = repoInm.GetInmuebles();
            if (IdInmueble != null)
            {
				ViewData["IdInmueble"]= IdInmueble;
            }
            else
            {
				ViewData["IdInmueble"]=null;

            }
            ViewData["Error"] = "";
            return View();
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato contrato)
        {


                if (contrato.FecDesde >= contrato.FecHasta)
                {
                    ViewBag.Inquilinos = repoInq.GetInquilinos();
                    ViewBag.Inmuebles = repoInm.GetInmuebles();
                    ViewData["Error"] = "La fecha de inicio debe ser menor que la fecha de fin.";
                    return View();
                }
                else
                {
                    ViewData["Error"] ="";
                    repoCon.Alta(contrato);
                    TempData["Id"] = contrato.Id;
                    return RedirectToAction(nameof(Index));
                    return View();
                }
            

            
            // Si hay errores en el modelo, volver al formulario para mostrar los errores.
           

           

        }

        // GET: Contrato/Edit/5
        [Authorize]
        public ActionResult Edit(int id, String a)
        {
            var entidad = repoCon.GetContrato(id);
           
            if(a!="Null"){
                ViewData["Titulo"]="Cancelar Contrato";
                 entidad.FecHasta = DateTime.Today;
            }
            else {
                ViewData["Titulo"]="Editar Contrato";
            }

            ViewData["Error"]=a;
            
			ViewBag.Inmuebles = repoInm.GetInmuebles();
            ViewBag.Inquilinos = repoInq.GetInquilinos();
			return View(entidad);
        }
        [Authorize]
        public ActionResult Renovar(int idInq, int idInm)
        {
            ViewData["Titulo"]="Renovar Contrato";
            ViewData["Error"]="";
            
			ViewBag.Inmuebles = repoInm.GetInmueble(idInm);
            ViewBag.Inquilinos = repoInq.GetInquilino(idInq);
			return View();
        }

        [Authorize]
        public ActionResult Cancelar(int id)
        {
            var contrato = repoCon.GetContrato(id);
            var inmueble = repoInm.GetInmueble(contrato.IdInmueble);

            var inicioContrato = contrato.FecDesde;
            var hoy = DateTime.Today;
            var diasTranscurridos = (int)(hoy - inicioContrato).TotalDays;

            var duracionContrato = (int)(contrato.FecHasta - inicioContrato).TotalDays;

            var mitadDuracion = duracionContrato / 2;
            var mesesAdicionales = diasTranscurridos < mitadDuracion ? 2 : 1;
            var a="";
            if (mesesAdicionales == 1)
            {
                a = "La multa a pagar es de " + 1 + " mes(es), un total de: $" + (inmueble.Precio * 1);
            }
            else
            {
                 a = "La multa a pagar es de " + 2 + " mes(es), un total de: $" + (inmueble.Precio * 2);
            }

            ViewBag.Inmuebles = repoInm.GetInmuebles();
            ViewBag.Inquilinos = repoInq.GetInquilinos();

            return RedirectToAction("Edit", new { id = contrato.Id , a });
        }
        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato contrato)
        {
            
                if (contrato.FecDesde >= contrato.FecHasta)
                {
                    ViewBag.Inquilinos = repoInq.GetInquilinos();
                    ViewBag.Inmuebles = repoInm.GetInmuebles();
                    TempData["Error"] = "La fecha de inicio debe ser menor que la fecha de fin.";
                    return View(contrato);
                }
                else
                {
                    TempData["Error"] ="";
                    contrato.Id = id;
                    repoCon.Modificar(contrato);
                    TempData["Mensaje"] = "Datos guardados correctamente";
                    return RedirectToAction(nameof(Index));
                }
                
            
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