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
    public class InquilinoController : Controller
    {
        private readonly ReposotorioInquilino Repo;
        public InquilinoController()
        {
            Repo = new ReposotorioInquilino();
        }
        // GET: Propietario
        public ActionResult Index()
        {
            var lista = Repo.GetInquilinos();
            return View(lista);
        }

        // GET: Propietario/Details/5
        public ActionResult Details(int id)
        {
             var entidad = Repo.GetInquilino(id);
            return View(entidad);
        }

        // GET: Propietario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino  inquilino)
        {
            try
            {
                // TODO: Add insert logic here
                var repo = new ReposotorioInquilino();
                repo.Alta(inquilino);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietario/Edit/5
        public ActionResult Edit(int id)
        {
            var entidad = Repo.GetInquilino(id);
            return View(entidad);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino entidad)
        {
            try
            {
                // TODO: Add update logic here

                 Repo.Modificar(entidad);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietario/Delete/5
        public ActionResult Delete(int id)
        {
              var entidad = Repo.GetInquilino(id);
            return View(entidad);
        }

        // POST: Propietario/Delete/5
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