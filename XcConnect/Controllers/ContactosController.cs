using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XcConnect.Models;
using XcConnect.Helpers;

namespace XcConnect.Controllers
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class ContactosController : Controller
    {
        private CRMContext db = new CRMContext();

        // GET: Contactos
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                return View(db.Contactos.Include(c => c.Clientes)
                    .OrderBy(c => c.Nombre)
                    .ToList());
            }
            else
            {
                return View(db.Contactos.Include(c => c.Clientes)
                    .Where(c => c.Clientes.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                    .OrderBy(c => c.Nombre)
                    .ToList());
            }
        }

        // GET: Contactos/Details/5
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contactos contactos = db.Contactos.Find(id);
            if (contactos == null)
            {
                return HttpNotFound();
            }
            return View(contactos);
        }

        // GET: Contactos/Create
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(c => c.Nombre), "ClienteID", "Nombre");
            }
            else
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                 .OrderBy(c => c.Nombre), "ClienteID", "Nombre");
            }
            return View();
        }

        // POST: Contactos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactoID,ClienteID,Nombre,Cargo,Telefono,Celular,Email")] Contactos contactos)
        {
            if (ModelState.IsValid)
            {
                db.Contactos.Add(contactos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nombre", contactos.ClienteID);
            return View(contactos);
        }

        // GET: Contactos/Edit/5
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contactos contactos = db.Contactos.Find(id);
            if (contactos == null)
            {
                return HttpNotFound();
            }

            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(c => c.Nombre), "ClienteID", "Nombre");
            }
            else
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                     .OrderBy(c => c.Nombre), "ClienteID", "Nombre");
            }

            return View(contactos);
        }

        // POST: Contactos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactoID,ClienteID,Nombre,Cargo,Telefono,Celular,Email")] Contactos contactos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                 .OrderBy(c => c.Nombre), "ClienteID", "Nombre");
            return View(contactos);
        }

        // GET: Contactos/Delete/5
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contactos contactos = db.Contactos.Find(id);
            if (contactos == null)
            {
                return HttpNotFound();
            }
            return View(contactos);
        }

        // POST: Contactos/Delete/5
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contactos contactos = db.Contactos.Find(id);
            db.Contactos.Remove(contactos);
            db.SaveChanges();
            return RedirectToAction("Index");
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
