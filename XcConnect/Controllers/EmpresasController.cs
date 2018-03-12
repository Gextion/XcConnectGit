using XcConnect.Helpers;
using XcConnect.Models;
using XcConnect.Models.Security;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace XcConnect.Controllers
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class EmpresasController : Controller
    {
        private CRMContext db = new CRMContext();

        // GET: Empresas
        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Index()
        {
            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                return View(db.Empresas.Include(e => e.Ciudad).OrderBy(e => e.Nombre).ToList());
            }
            else
            {
                return View(db.Empresas.Include(e => e.Ciudad)
                                       .Where(p => p.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                       .OrderBy(e => e.Nombre).ToList());
            }
        }

        [Authorize(Roles = "AccessAll, Companies, BusinessEntity, Interventor")]
        public ActionResult ClientesEmpresa()
        {
            var q = db.Empresas
                //.GroupBy(g => new { EmpresaID = g.EmpresaID })
                .Select(c => new ClientesEmpresa
                {
                    EmpresaID = c.EmpresaID,
                    Codigo = c.Codigo,
                    Nombre = c.Nombre,
                    Nit = c.Nit,
                    Ciudad = c.Ciudad.Ciudad,
                    LineaBase = c.LineaBase,
                    Clientes = c.Clientes.Count()
                }).ToList();

            return View(q);
        }

        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Create()
        {
            ViewBag.CiudadID = new SelectList(db.Ciudades, "CiudadID", "Ciudad");
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Create([Bind(Exclude = "EmpresaPhoto", Include = "EmpresaID,Codigo,Nombre,RazonSocial,Nit,CiudadID,Direccion,Telefono,Celular,Email,SitioWeb,LineaBase")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;

                try
                {
                    #region System User Validations

                    var CountCode = db.Empresas.Where(e => e.Codigo.Equals(empresa.Codigo)).Count();
                    if (CountCode > 0)
                        throw new Exception("El Código ingresado ya se encuentra asociado a otra empresa. Debe ser un valor único.");

                    if (empresa.Nit.ToString().Length < 5)
                        throw new Exception("El Nit debe tener al menos 5 caracteres.");

                    var CountNit = db.Empresas.Where(e => e.Nit.Equals(empresa.Nit)).Count();
                    if (CountNit > 0)
                        throw new Exception("El Nit ingresado ya se encuentra asociado a otra empresa. Debe ser un valor único.");
                    #endregion

                    if (string.IsNullOrEmpty(empresa.Email))
                        throw new Exception("El email es un valor requerido.");

                    //var CountEmails = db.Empresas.Where(e => e.Email.ToLower().Trim().Equals(empresa.Email.ToLower().Trim())).Count();
                    //if (CountEmails > 0)
                    //    throw new Exception("El Email ingresado ya se encuentra asociado a otra empresa. Debe ser un valor único.");

                    string LogoUrl = GetEmpresaLogoUrl(empresa.LogoUrl);
                    if (!string.IsNullOrEmpty(LogoUrl))
                        empresa.LogoUrl = LogoUrl;

                    db.Empresas.Add(empresa);
                    db.SaveChanges();

                    var Consecutivo = db.CotizacionConsecutivo.Where(cc => cc.EmpresaID == empresa.EmpresaID).FirstOrDefault();
                    if (Consecutivo == null)
                    {
                        Consecutivo = new CotizacionConsecutivo() { EmpresaID = empresa.EmpresaID, ValorConsecutivo = 0 };
                        db.CotizacionConsecutivo.Add(Consecutivo);
                    }

                    //Create System User
                    var ResultUsr = Models.Security.ApplicationUser.AddUser(empresa.Codigo.ToString(), Startup.InitialFinalUserPwd, empresa.Nombre, empresa.Nit.ToString(), empresa.Email, empresa.EmpresaID);
                    if (!ResultUsr.Succeeded)
                    {
                        ModelState.AddModelError("", String.Join(", ", ResultUsr.Errors.Select(u => u.ToString())));
                        hasErrors = true;
                    }
                }
                catch (Exception eX)
                {
                    ModelState.AddModelError("", eX.Message);
                    hasErrors = true;
                }

                if (!hasErrors)
                    return RedirectToAction("Index");
            }

            ViewBag.CiudadID = new SelectList(db.Ciudades, "CiudadID", "Ciudad", empresa.CiudadID);
            return View(empresa);
        }
        
        // GET: Empresas/Edit/5
        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadID = new SelectList(db.Ciudades, "CiudadID", "Ciudad", empresa.CiudadID);
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "EmpresaPhoto", Include = "EmpresaID,Codigo,Nombre,RazonSocial,Nit,CiudadID,Direccion,Telefono,Celular,Email,SitioWeb,LineaBase")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                string LogoUrl = GetEmpresaLogoUrl(empresa.LogoUrl);
                if (!string.IsNullOrEmpty(LogoUrl))
                    empresa.LogoUrl = LogoUrl;

                db.Entry(empresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CiudadID = new SelectList(db.Ciudades, "CiudadID", "Ciudad", empresa.CiudadID);
            return View(empresa);
        }

        /// <summary>
        /// Delete Object From AjaxJQuery
        /// </summary>
        /// <param name="id">PK Value</param>
        /// <returns></returns>        
        public ActionResult Delete(int id)
        {
            DbContextTransaction Transaction = null;

            try
            {
                Empresa empresa = db.Empresas.Include(e => e.Clientes).Include(e => e.Peticiones).Include(e => e.Productos).Include(e => e.Actividades).Where(e => e.EmpresaID.Equals(id)).FirstOrDefault();
                if (empresa != null)
                {
                    if (empresa.Clientes != null && empresa.Clientes.Count > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "No es posible eliminar una empresa que tiene clientes asociados.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    if (empresa.Peticiones != null && empresa.Peticiones.Count > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "No es posible eliminar una empresa que tiene peticiones asociadas.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    if (empresa.Productos != null && empresa.Productos.Count > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "No es posible eliminar una empresa que tiene productos asociadas.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    if (empresa.Actividades != null && empresa.Actividades.Count > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "No es posible eliminar una empresa que tiene actividades asociadas.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    Transaction = db.Database.BeginTransaction();

                    SecurityDbContext dbSecurity = new SecurityDbContext();

                    var UsersToDelete = dbSecurity.Users.Include(u => u.Groups).Where(u => u.EmpresaID == empresa.EmpresaID).ToList();
                    if (UsersToDelete != null && UsersToDelete.Count > 0)
                    {
                        for (int i = 0; i < UsersToDelete.Count; i++)
                        {
                            var ResultUsr = ApplicationUser.ClearUserGroups(UsersToDelete[i]);
                            if (!ResultUsr.Succeeded)
                            {
                                return new JsonResult
                                {
                                    Data = new { Message = String.Join(", ", ResultUsr.Errors.Select(u => u.ToString())), Success = false },
                                    ContentEncoding = System.Text.Encoding.UTF8,
                                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };
                            }
                            else
                            {
                                dbSecurity.Users.Remove(UsersToDelete[i]);
                            }
                        }

                        dbSecurity.SaveChanges();
                    }

                    db.Empresas.Remove(empresa);
                    db.SaveChanges();

                    Transaction.Commit();
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { Message = "No es posible identificar la empresa. Por favor, intente de nuevo.", Success = false },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
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
                if (Transaction != null)
                    Transaction.Rollback();
                
                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        
        /// <summary>
        /// Get Empresa Logo Url in Request
        /// </summary>
        /// <returns></returns>
        private string GetEmpresaLogoUrl(string CurrentName)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var Inputfile = Request.Files["EmpresaPhoto"];
                if (Inputfile != null && Inputfile.ContentLength > 0)
                {
                    var fileExtension = Path.GetFileName(Inputfile.FileName.Substring(Inputfile.FileName.LastIndexOf('.')));
                    string RelativePath = $"/Uploads/Logos/{Guid.NewGuid().ToString()}{fileExtension}";

                    if (!String.IsNullOrEmpty(CurrentName))
                    {
                        try
                        {
                            if (System.IO.File.Exists(CurrentName))
                                System.IO.File.Delete(CurrentName);
                        }
                        catch { }
                    }

                    var path = Server.MapPath($"~{RelativePath}");
                    Inputfile.SaveAs(path);

                    return $"{Request.Url.GetLeftPart(UriPartial.Authority)}{RelativePath}";
                }
            }

            return string.Empty;
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
