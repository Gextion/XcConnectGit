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
    public class OportunidadesController : Controller
    {
        private CRMContext db = new CRMContext();

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                return View(db.Oportunidad.Include(e => e.Archivos).Include(v => v.Vendedor)
                             .OrderBy(c => c.NombreOportunidad)
                             .ToList());
            }
            else
            {
                return View(db.Oportunidad.Include(e => e.Archivos).Include(v => v.Vendedor)
                             .OrderBy(c => c.NombreOportunidad)
                             .Where(c => c.Clientes.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
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
            Oportunidad oportunidad = db.Oportunidad.Find(id);
            if (oportunidad == null)
            {
                return HttpNotFound();
            }
            return View(oportunidad);
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(c => c.Nombre), "ClienteID", "Nombre");
                ViewBag.VendedorID = new SelectList(db.Vendedores.OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor");
            }
            else
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                              .OrderBy(c => c.Nombre), "ClienteID", "Nombre");
                ViewBag.VendedorID = new SelectList(db.Vendedores.Where(v => v.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                              .OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor");
            }
            var o = new Oportunidad();
            o.FechaSolicitud = Helpers.DateHelper.GetColombiaDateTime();
            return View(o);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create([Bind(Exclude = "ArchivosUploader", Include = "OportunidadID,NombreOportunidad,Descripcion,Estado,ClienteID,SolicitadaPor,VendedorID,FechaSolicitud,FechaEntrega,FechaCierre,Valor")] Oportunidad oportunidad)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    if (oportunidad.Archivos == null)
                        oportunidad.Archivos = new List<OportunidadesArchivos>();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileWrapper file = Request.Files[i] as HttpPostedFileWrapper;
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileExtension = Path.GetFileName(file.FileName.Substring(file.FileName.LastIndexOf('.')));
                            string RelativePath = $"/Uploads/Docs/Opor/{Guid.NewGuid().ToString()}{fileExtension}";

                            var path = Server.MapPath($"~{RelativePath}");

                            file.SaveAs(path);

                            oportunidad.Archivos.Add(new OportunidadesArchivos() {
                                Oportunidad = oportunidad,
                                ArchivoNombre = file.FileName,
                                ArchivoUrl = $"{Request.Url.GetLeftPart(UriPartial.Authority)}{RelativePath}",
                                LocalUrl = path
                            });
                        }
                    }
                }

                oportunidad.UserID = ApplicationContext.CurrentUser.Id;
                db.Oportunidad.Add(oportunidad);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(c => c.Nombre), "ClienteID", "Nombre", oportunidad.ClienteID);
            ViewBag.VendedorID = new SelectList(db.Vendedores.OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor", oportunidad.VendedorID);
            return View(oportunidad);
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oportunidad oportunidad = db.Oportunidad.Find(id);
            if (oportunidad == null)
            {
                return HttpNotFound();
            }

            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.Include(v => v.Vendedor)
                                                              .OrderBy(c => c.Nombre), "ClienteID", "Nombre", oportunidad.ClienteID);
                ViewBag.VendedorID = new SelectList(db.Vendedores.OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor", oportunidad.VendedorID);
            }
            else
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                              .OrderBy(c => c.Nombre), "ClienteID", "Nombre", oportunidad.ClienteID);
                ViewBag.VendedorID = new SelectList(db.Vendedores.Where(v => v.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                              .OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor", oportunidad.VendedorID);
            }

            return View(oportunidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit([Bind(Exclude = "ArchivosUploader", Include = "OportunidadID,NombreOportunidad,Descripcion,ClienteID,Estado,SolicitadaPor,VendedorID,FechaSolicitud,FechaEntrega,FechaCierre,Valor,UserID")] Oportunidad oportunidad)
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

                            db.OportunidadArchivos.Add(new OportunidadesArchivos() {
                                Oportunidad = oportunidad,
                                OportunidadID = oportunidad.OportunidadID,
                                ArchivoNombre = file.FileName,
                                ArchivoUrl = $"{Request.Url.GetLeftPart(UriPartial.Authority)}{RelativePath}",
                                LocalUrl = path
                            });
                        }
                    }
                }

                db.Entry(oportunidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                          .OrderBy(c => c.Nombre), "ClienteID", "Nombre", oportunidad.ClienteID);
            ViewBag.VendedorID = new SelectList(db.Vendedores.Where(v => v.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                             .OrderBy(v => v.NombreVendedor), "VendedorID", "NombreVendedor", oportunidad.VendedorID);
            return View(oportunidad);
        }
        
        
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult DeleteFile(int? id)
        {
            var oportunidadFile = db.OportunidadArchivos.Find(id);

            if (oportunidadFile != null)
            {
                if (!string.IsNullOrEmpty(oportunidadFile.LocalUrl))
                {
                    if (System.IO.File.Exists(oportunidadFile.LocalUrl))
                    {
                        try
                        {
                            System.IO.File.Delete(oportunidadFile.LocalUrl);
                        }
                        catch { }
                    }
                }

                db.OportunidadArchivos.Remove(oportunidadFile);
                db.SaveChanges();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
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
                Oportunidad oportunidad = db.Oportunidad.Find(id);
                if (oportunidad != null)
                {
                    var CountCotizacion = db.Cotizacion.Where(c => c.OportunidadID == oportunidad.OportunidadID).Count();
                    if (CountCotizacion > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "No es posible eliminar una oportunidad que tiene cotizaciones asociadas.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    if (oportunidad.Archivos != null && oportunidad.Archivos.Count > 0)
                    {
                        foreach (var archivo in oportunidad.Archivos)
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

                        db.OportunidadArchivos.RemoveRange(oportunidad.Archivos);
                    }

                    db.Oportunidad.Remove(oportunidad);
                    db.SaveChanges();
                }

                return new JsonResult
                {
                    Data = new { Message = string.Empty, Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
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
