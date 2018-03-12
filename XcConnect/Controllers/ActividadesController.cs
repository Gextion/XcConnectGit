using XcConnect.Helpers;
using XcConnect.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace XcConnect.Controllers
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class ActividadesController : Controller
    {
        private CRMContext db = new CRMContext();

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                return View(db.Actividades.Include(e => e.Archivos)
                             .OrderBy(a => a.FechaRegistro)
                             .ToList());
            }
            else
            {
                return View(db.Actividades.Include(e => e.Archivos)
                             .OrderBy(a => a.Descripcion)
                             .Where(a => a.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                             .ToList());
            }
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return HttpNotFound();
            }
            return View(actividades);
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            var a = new Actividades();
            a.FechaRegistro = Helpers.DateHelper.GetColombiaDateTime();
            ViewBag.TipoActividadID = new SelectList(db.TipoActividad.OrderBy(t => t.NombreTipoActividad), "TipoActividadID", "NombreTipoActividad");
            return View(a);
        }

        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ArchivosUploader", Include = "ActividadID,FechaRegistro,Descripcion,TipoActividadID,FechaEntrega,Notas,Adjunto,UserID")] Actividades actividades)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    if (actividades.Archivos == null)
                        actividades.Archivos = new List<ActividadesArchivos>();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileWrapper file = Request.Files[i] as HttpPostedFileWrapper;
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileExtension = Path.GetFileName(file.FileName.Substring(file.FileName.LastIndexOf('.')));
                            string RelativePath = $"/Uploads/Docs/Act/{Guid.NewGuid().ToString()}{fileExtension}";

                            var path = Server.MapPath($"~{RelativePath}");

                            file.SaveAs(path);

                            actividades.Archivos.Add(new ActividadesArchivos()
                            {
                                Actividad = actividades,
                                ArchivoNombre = file.FileName,
                                ArchivoUrl = $"{Request.Url.GetLeftPart(UriPartial.Authority)}{RelativePath}",
                                LocalUrl = path
                            });
                        }
                    }
                }

                actividades.UserID = ApplicationContext.CurrentUser.Id;
                actividades.EmpresaID = ApplicationContext.CurrentUser.EmpresaID;
                db.Actividades.Add(actividades);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.TipoActividadID = new SelectList(db.TipoActividad.OrderBy(t => t.NombreTipoActividad), "TipoActividadID", "NombreTipoActividad", actividades.TipoActividadID);

            return View(actividades);
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return HttpNotFound();
            }

            ViewBag.TipoActividadID = new SelectList(db.TipoActividad.OrderBy(t => t.NombreTipoActividad), "TipoActividadID", "NombreTipoActividad", actividades.TipoActividadID);

            return View(actividades);
        }

        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "ArchivosUploader", Include = "ActividadID,FechaRegistro,Descripcion,TipoActividadID,FechaEntrega,Notas,EmpresaID,UserID")] Actividades actividades)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileWrapper file = Request.Files[i] as HttpPostedFileWrapper;
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileExtension = Path.GetFileName(file.FileName.Substring(file.FileName.LastIndexOf('.')));
                            string RelativePath = $"/Uploads/Docs/Opor/{Guid.NewGuid().ToString()}{fileExtension}";

                            var path = Server.MapPath($"~{RelativePath}");

                            file.SaveAs(path);

                            db.ActividadesArchivos.Add(new ActividadesArchivos()
                            {
                                Actividad = actividades,
                                ActividadArchivoID = actividades.ActividadID,
                                ArchivoNombre = file.FileName,
                                ArchivoUrl = $"{Request.Url.GetLeftPart(UriPartial.Authority)}{RelativePath}",
                                LocalUrl = path
                            });
                        }
                    }
                }

                db.Entry(actividades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoActividadID = new SelectList(db.TipoActividad.OrderBy(t => t.NombreTipoActividad), "TipoActividadID", "NombreTipoActividad", actividades.TipoActividadID);
            return View(actividades);
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return HttpNotFound();
            }
            return View(actividades);
        }

        [Authorize(Roles = "BusinessEntity")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actividades actividades = db.Actividades.Find(id);
            if (actividades != null)
            {

                if (actividades.Archivos != null && actividades.Archivos.Count > 0)
                {
                    foreach (var archivo in actividades.Archivos)
                    {
                        if (System.IO.File.Exists(archivo.LocalUrl))
                        {
                            try
                            {
                                System.IO.File.Delete(archivo.LocalUrl);
                            }
                            catch { }
                        }
                    }

                    db.ActividadesArchivos.RemoveRange(actividades.Archivos);
                }

                db.Actividades.Remove(actividades);
                db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult DeleteFile(int? id)
        {
            var actividadFile = db.ActividadesArchivos.Find(id);
            if (actividadFile != null)
            {
                if (!string.IsNullOrEmpty(actividadFile.LocalUrl))
                {
                    if (System.IO.File.Exists(actividadFile.LocalUrl))
                    {
                        try
                        {
                            System.IO.File.Delete(actividadFile.LocalUrl);
                        }
                        catch { }
                    }
                }

                db.ActividadesArchivos.Remove(actividadFile);
                db.SaveChanges();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
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
