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
    /// <summary>
    /// VendedoresController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class VendedoresController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private CRMContext db = new CRMContext();

        /// <summary>
        /// View in List Mode
        /// <example> GET: Vendedores </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                return View(db.Vendedores.Include(v => v.Empresa));
            }
            else
            {
                return View(db.Vendedores.Include(v => v.Empresa)
                                         .Where(e => e.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                         .OrderBy(v => v.NombreVendedor)
                                         .ToList());
            }
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: Vendedores/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedores.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> GET: Vendedores/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            ViewBag.EmpresaID = new SelectList(db.Empresas, "EmpresaID", "Nombre");
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action Create New Object
        /// <example> POST: Vendedores/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendedorID,Codigo,NombreVendedor,Celular,Email,EmpresaID")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();
                    vendedor.EmpresaID = ApplicationContext.CurrentUser.EmpresaID;
                    db.Vendedores.Add(vendedor);
                    db.SaveChanges();

                    Transaction.Commit();
                }
                catch (Exception eX)
                {
                    if (Transaction != null)
                        Transaction.Rollback();

                    ModelState.AddModelError("", eX.Message);
                    hasErrors = true;
                }
                if (!hasErrors)
                    return RedirectToAction("Index");
            }

            ViewBag.EmpresaID = new SelectList(db.Empresas, "EmpresaID", "Nombre", vendedor.EmpresaID);
            return View(vendedor);
        }

        /// <summary>
        /// Edit Object
        /// <example> GET: Vendedores/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedores.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaID = new SelectList(db.Empresas, "EmpresaID", "Nombre", vendedor.EmpresaID);
            return View(vendedor);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit Object
        /// <example> POST: Vendedores/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VendedorID,Codigo,NombreVendedor,Celular,Email,EmpresaID")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    vendedor.EmpresaID = ApplicationContext.CurrentUser.EmpresaID;
                    db.Entry(vendedor).State = EntityState.Modified;
                    db.SaveChanges();
                    Transaction.Commit();
                }
                catch (Exception eX)
                {
                    if (Transaction != null)
                        Transaction.Rollback();

                    ModelState.AddModelError("", eX.Message);
                    hasErrors = true;
                }

                if (!hasErrors)
                    return RedirectToAction("Index");
            }
            ViewBag.EmpresaID = new SelectList(db.Empresas, "EmpresaID", "Nombre", vendedor.EmpresaID);
            return View(vendedor);
        }

        /// <summary>
        /// Delete Object From AjaxJQuery
        /// </summary>
        /// <param name="id">PK Value</param>
        /// <returns></returns>        
        public ActionResult Delete(int id)
        {
            try
            {
                Vendedor vendedor = db.Vendedores.Find(id);
                if (vendedor != null)
                {
                    var CountVendedor = db.Cotizacion.Where(c => c.VendedorID == vendedor.VendedorID).Count();
                    if (CountVendedor > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "No es posible eliminar un vendedor que tiene cotizaciones asociadas.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    db.Vendedores.Remove(vendedor);
                    db.SaveChanges();
                }

                return new JsonResult {
                    Data = new { Message = string.Empty, Success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception eX)
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        /// <summary>
        /// Dispose Resources
        /// </summary>
        /// <param name="disposing"></param>
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
