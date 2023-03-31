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
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario Repo;
         public PropietarioController()
        {
            Repo = new RepositorioPropietario();
        }
        // GET: Propietario
        public ActionResult Index()
        {
            var lista = Repo.GetPropietarios();
            return View(lista);
        }

        // GET: Propietario/Details/5
        public ActionResult Details(int id)
        {
             var entidad = Repo.GetPropietario(id);
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
        public ActionResult Create(Propietario  propietario)
        {
            try
            {
                // TODO: Add insert logic here
                var repo = new RepositorioPropietario();
                repo.Alta(propietario);
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
            var entidad = Repo.GetPropietario(id);
            return View(entidad);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario entidad)
        {
            try
            {
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
              var entidad = Repo.GetPropietario(id);
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