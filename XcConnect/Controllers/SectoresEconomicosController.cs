using XcConnect.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace XcConnect.Controllers
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class SectoresEconomicosController : Controller
    {
        private CRMContext db = new CRMContext();

        // GET: SectoresEconomicos
        [Authorize(Roles = "AccessAll, Companies")]
        public ActionResult Index()
        {
            return View(db.SectoresEconomicos.OrderBy(s => s.SectorEconomico).ToList());
        }

        [Authorize(Roles = "AccessAll, Companies")]
        // GET: SectoresEconomicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectoresEconomicos sectoresEconomicos = db.SectoresEconomicos.Find(id);
            if (sectoresEconomicos == null)
            {
                return HttpNotFound();
            }
            return View(sectoresEconomicos);
        }

        [Authorize(Roles = "AccessAll, Companies")]
        // GET: SectoresEconomicos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SectoresEconomicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AccessAll, Companies")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectorEconomicoID,SectorEconomico")] SectoresEconomicos sectoresEconomicos)
        {
            if (ModelState.IsValid)
            {
                db.SectoresEconomicos.Add(sectoresEconomicos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sectoresEconomicos);
        }

        // GET: SectoresEconomicos/Edit/5
        [Authorize(Roles = "AccessAll, Companies")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectoresEconomicos sectoresEconomicos = db.SectoresEconomicos.Find(id);
            if (sectoresEconomicos == null)
            {
                return HttpNotFound();
            }
            return View(sectoresEconomicos);
        }

        // POST: SectoresEconomicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AccessAll, Companies")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectorEconomicoID,SectorEconomico")] SectoresEconomicos sectoresEconomicos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sectoresEconomicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sectoresEconomicos);
        }

        [Authorize(Roles = "AccessAll, Companies")]
        // GET: SectoresEconomicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectoresEconomicos sectoresEconomicos = db.SectoresEconomicos.Find(id);
            if (sectoresEconomicos == null)
            {
                return HttpNotFound();
            }
            return View(sectoresEconomicos);
        }

        // POST: SectoresEconomicos/Delete/5
        [Authorize(Roles = "AccessAll, Companies")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SectoresEconomicos sectoresEconomicos = db.SectoresEconomicos.Find(id);
            db.SectoresEconomicos.Remove(sectoresEconomicos);
            db.SaveChanges();
            return RedirectToAction("Index");
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
