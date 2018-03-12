using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using XcConnect.Models;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;
using XcConnect.Models.Reports;

namespace XcConnect.Controllers
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class ReportClientesController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private CRMContext db = new CRMContext();

        /// <summary>
        /// View Consumos Report
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Clientes()
        {
            SetViewBagListData();

            RptClientes ViewModel = new RptClientes() { };

            return View(ViewModel);
        }

        /// <summary>   
        /// Build Report Empresas
        /// </summary>
        /// <returns></returns>        
        public ActionResult BuildRptClientes(int? CiudadID)
        {
            try
            {
                StringBuilder RptContent = new StringBuilder();
                RptContent.Append(@"<thead><tr><th align = ""center""><b> Cliente </b></th><th align = ""center""><b> Nit </b></th>><th align = ""center""><b> Vendedor </b></th><th align = ""center""><b> Ciudad </b></th><th align = ""center""><b> Dirección </b></th><th align = ""center""><b> Teléfono </b></th><th align = ""center""><b> Celular </b></th><th align = ""center""><b> Correo </b></th></tr></thead>");
                RptContent.Append("<tbody>");

                List<Cliente> Clientes = null;

                if (CiudadID.HasValue && CiudadID.Value != 999999)
                {
                    Clientes = db.Clientes.Include(e => e.Ciudad).Where(e => e.CiudadID == CiudadID.Value).OrderBy(c => c.Nombre).ToList();
                }
                else
                {
                    if (XcConnect.Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                    {
                        Clientes = db.Clientes.Include(e => e.Ciudad).OrderBy(c => c.Nombre).ToList();
                    }
                    else
                    {
                        Clientes = db.Clientes.Include(e => e.Ciudad)
                                     .Where(e => e.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID))
                                     .OrderBy(c => c.Nombre).ToList();
                    }
                }

                //var Clientes = Empresa.OrderBy(e => e.Nombre).ThenBy(e => e.Ciudad.Ciudad);

                if (Clientes != null && Clientes.Count() > 0)
                {
                    foreach (var c in Clientes)
                    {
                        RptContent.Append($"<tr><td>{c.Nombre}</td><td>{c.Nit}</td><td>{c.Vendedor.NombreVendedor}</td><td>{c.Ciudad.Ciudad}</td><td>{c.Direccion}</td><td>{c.Telefono}</td><td>{c.Celular}</td><td>{c.Email}</td></tr>");
                    }
                }

                RptContent.Append("</tbody>");

                var result = new JsonResult
                {
                    Data = new { RptContent = RptContent.ToString(), Success = true },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                result.MaxJsonLength = int.MaxValue;

                return result;
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción solicitada. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Set ViewBag List Data
        /// </summary>
        private void SetViewBagListData()
        {
            ViewBag.CiudadID = new SelectList(db.Ciudades.OrderBy(d => d.Ciudad), "CiudadID", "Ciudad", null);

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID);
            }
            else
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas
                                                     .Where(s => s.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID))
                                                     .OrderBy(e => e.Nombre), "EmpresaID", "Nombre", Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID);

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