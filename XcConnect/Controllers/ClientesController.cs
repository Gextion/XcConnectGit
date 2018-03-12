using XcConnect.Helpers;
using XcConnect.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace XcConnect.Controllers
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class ClientesController : Controller
    {
        private CRMContext db = new CRMContext();

        // GET: Empresas
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                return View( db.Clientes.Include(e => e.Ciudad)
                                        .Include(e => e.SectorEconomico)
                                        .Include(e => e.Empresa)
                                        .Include(e => e.Vendedor)
                             .OrderBy(c => c.Nombre)
                             .ToList());
            }
            else
            {
                return View (db.Clientes.Include(e => e.Ciudad)
                                        .Include(e => e.SectorEconomico)
                                        .Include(e => e.Empresa)
                                        .Include(e => e.Vendedor)
                    .Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                    .OrderBy(c => c.Nombre)
                    .ToList());
            }
        }

        // GET: Empresas/Details/5
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Empresas/Create
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            ViewBag.CiudadID = new SelectList(db.Ciudades.OrderBy(c => c.Ciudad) , "CiudadID", "Ciudad");
            ViewBag.SectorEconomicoID = new SelectList(db.SectoresEconomicos.OrderBy(s => s.SectorEconomico), "SectorEconomicoID", "SectorEconomico");
            ViewBag.VendedorID = new SelectList(db.Vendedores.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                             .OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor");
            return View();
        }

        // POST: clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClienteID,Nombre,RazonSocial,Nit,RepresentanteLegal,SectorEconomicoID,CiudadID,Direccion,Telefono,Celular,Email,SitioWeb,EmpresaID,VendedorID")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.EmpresaID = ApplicationContext.CurrentUser.EmpresaID;
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CiudadID = new SelectList(db.Ciudades.OrderBy(c => c.Ciudad), "CiudadID", "Ciudad", cliente.CiudadID);
            ViewBag.SectorEconomicoID = new SelectList(db.SectoresEconomicos.OrderBy(s => s.SectorEconomico), "SectorEconomicoID", "SectorEconomico", cliente.SectorEconomicoID);
            ViewBag.VendedorID = new SelectList(db.Vendedores.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                             .OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor");
            return View(cliente);
        }

        // GET: Empresas/Edit/5
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadID = new SelectList(db.Ciudades.OrderBy(c => c.Ciudad), "CiudadID", "Ciudad", cliente.CiudadID);
            ViewBag.SectorEconomicoID = new SelectList(db.SectoresEconomicos.OrderBy(s => s.SectorEconomico), "SectorEconomicoID", "SectorEconomico", cliente.SectorEconomicoID);
            ViewBag.VendedorID = new SelectList(db.Vendedores.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                             .OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor", cliente.VendedorID);
            return View(cliente);
        }

        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClienteID,Nombre,RazonSocial,Nit,RepresentanteLegal,SectorEconomicoID,CiudadID,Direccion,Telefono,Celular,Email,SitioWeb,EmpresaID,VendedorID")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.EmpresaID = ApplicationContext.CurrentUser.EmpresaID;
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CiudadID = new SelectList(db.Ciudades.OrderBy(c => c.Ciudad), "CiudadID", "Ciudad", cliente.CiudadID);
            ViewBag.SectorEconomicoID = new SelectList(db.SectoresEconomicos.OrderBy(s => s.SectorEconomico), "SectorEconomicoID", "SectorEconomico", cliente.SectorEconomicoID);
            ViewBag.VendedorID = new SelectList(db.Vendedores.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                             .OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor", cliente.VendedorID);
            return View(cliente);
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        [Authorize(Roles = "BusinessEntity")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            try
            {
                db.Clientes.Remove(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("","Se ha presentado un error eliminando el cliente, elimine oportunidades y contactos antes de eliminarlo");
                return View(cliente);
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
