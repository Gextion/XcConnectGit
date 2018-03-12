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
    public class PeticionesController : Controller
    {
        private CRMContext db = new CRMContext();

        // GET: Peticiones
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                return View(db.Peticions.OrderBy(p => p.FechaRegistro).ToList());
            }
            else
            {
                return View(db.Peticions.Where(p => p.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                        .OrderBy(p => p.FechaRegistro)
                                        .ToList());
            }
        }

        // GET: Peticiones/Details/5
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peticion peticion = db.Peticions.Find(id);
            if (peticion == null)
            {
                return HttpNotFound();
            }
            return View(peticion);
        }

        // GET: Peticiones/Create
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            var p = new Peticion();
            p.FechaRegistro = Helpers.DateHelper.GetColombiaDateTime();
            return View(p);
        }

        // POST: Peticiones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PeticionID,FechaRegistro,Titulo,Descripcion,TipoPeticion,EmpresaID,UserID")] Peticion peticion)
        {
            if (ModelState.IsValid)
            {
                peticion.UserID = ApplicationContext.CurrentUser.Id;
                peticion.EmpresaID = ApplicationContext.CurrentUser.EmpresaID;
                db.Peticions.Add(peticion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(peticion);
        }

        // GET: Peticiones/Edit/5
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peticion peticion = db.Peticions.Find(id);
            if (peticion == null)
            {
                return HttpNotFound();
            }
            return View(peticion);
        }

        // POST: Peticiones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PeticionID,FechaRegistro,Titulo,Descripcion,TipoPeticion,FechaSolucion,Solucion,ResueltaPor,EmpresaID,UserID")] Peticion peticion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(peticion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(peticion);
        }

        // GET: Peticiones/Delete/5
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peticion peticion = db.Peticions.Find(id);
            if (peticion == null)
            {
                return HttpNotFound();
            }
            return View(peticion);
        }

        // POST: Peticiones/Delete/5
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Peticion peticion = db.Peticions.Find(id);
            db.Peticions.Remove(peticion);
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
