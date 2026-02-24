using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UsuariosCRUD.Models;
using System.Data.Entity.Infrastructure;

namespace UsuariosCRUD.Controllers
{
    public class UsuariosController : Controller
    {
        private dbModels db = new dbModels();

        // GET: Usuarios
        public async Task<ActionResult> Index()
        {
            return View(await db.Usuarios.ToListAsync());
        }

        public async Task<ActionResult> Buscar(string cedula)
        {
            var resultado = await db.Usuarios
                                    .Where(u => u.Cedula == cedula)
                                    .ToListAsync();

            return View("Index", resultado);
        }

        // GET: Usuarios/Details
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Cedula,Nombre,Apellidos,Profesion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                bool existe = await db.Usuarios.AnyAsync(u => u.Cedula == usuario.Cedula);

                if (existe)
                {
                    ModelState.AddModelError("Cedula", "Ya existe un usuario con esa cédula.");
                    return View(usuario);
                }

                try
                {
                    db.Usuarios.Add(usuario);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar el usuario.");
                }
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/id
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Cedula,Nombre,Apellidos,Profesion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(usuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "No se pudo actualizar el usuario.");
                }
            }

            return View(usuario);
        }

        // GET: Usuarios/Delete/id
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/id

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                Usuario usuario = await db.Usuarios.FindAsync(id);

                if (usuario == null)
                    return HttpNotFound();

                db.Usuarios.Remove(usuario);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "No se pudo eliminar el usuario.");
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
