using XcConnect.Models;
using XcConnect.Models.Reports;
using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using XcConnect.Helpers;

namespace XcConnect.Controllers
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class ReportsController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private CRMContext db = new CRMContext();

        /// <summary>
        /// View Consumos Report
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Actividades()
        {
            RptActividades ViewModel = new RptActividades()
            {
                FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                FechaFinal = DateTime.Now
            };

            return View(ViewModel);
        }

        /// <summary>
        /// View Consumos Report
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Oportunidades()
        {
            SetViewBagListData();

            RptOportunidades ViewModel = new RptOportunidades()
            {
                FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                FechaFinal = DateTime.Now
            };

            return View(ViewModel);
        }

        /// <summary>
        /// View Consumos Report
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Cotizaciones()
        {
            SetViewBagListData();

            RptCotizaciones ViewModel = new RptCotizaciones()
            {
                FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                FechaFinal = DateTime.Now
            };

            return View(ViewModel);
        }

        /// <summary>   
        /// Build Report
        /// </summary>
        /// <returns></returns>
        public ActionResult BuildRptActividades(DateTime? FecIni, DateTime? FecFin)
        {
            try
            {
                StringBuilder RptContent = new StringBuilder();
                RptContent.Append("<thead><tr><th align=\"center\"><b>Fecha</b></th><th align=\"center\" ><b>Descripción</b></th><th align=\"center\" ><b>Tipo Actividad</b></th><th align=\"center\" ><b>Fecha Ejecución</b></th><th align=\"center\"><b>Notas</b></th></tr></thead>");
                RptContent.Append("<tbody>");

                List<Actividades> Actividades = null;

                if (Helpers.ApplicationContext.CurrentUser.IsSeller)
                {
                    Actividades = db.Actividades.Include(a => a.TipoActividad).Where(a => a.UserID == Helpers.ApplicationContext.CurrentUser.Id).OrderBy(s => s.FechaEntrega).ToList();
                }
                else
                {
                    Actividades = db.Actividades.Include(a => a.TipoActividad).OrderBy(s => s.FechaEntrega).ToList();
                }

                if (FecIni.HasValue)
                    Actividades = Actividades.Where(c => c.FechaEntrega >= FecIni.Value).ToList();

                if (FecFin.HasValue)
                    Actividades = Actividades.Where(c => c.FechaEntrega <= FecFin.Value).ToList();

                if (Actividades != null && Actividades.Count() > 0)
                {
                    foreach (var e in Actividades)
                        RptContent.Append($"<tr><td>{e.FechaRegistro.ToShortDateString()}</td><td>{e.Descripcion}</td><td>{e.TipoActividad.NombreTipoActividad}</td><td>{e.FechaEntrega.Value.ToShortDateString()}</td><td>{e.Notas}</td></tr>");
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
        /// Build Report Oportunidades
        /// </summary>
        /// <param name="FecIni"></param>
        /// <param name="FecFin"></param>
        /// <returns></returns>
        public ActionResult BuildRptOportunidades(int? ClienteID, int? VendedorID, EstadosOportunidad Estado, DateTime? FecIni, DateTime? FecFin)
        {
            try
            {
                StringBuilder RptContent = new StringBuilder();
                RptContent.Append("<thead><tr><th align=\"center\"><b>Fecha Solicitud</b></th><th align=\"center\" ><b>Nombre</b></th><th align=\"center\" ><b>Cliente</b></th><th align=\"center\" ><b>Vendedor</b></th><th align=\"center\"><b>Estado</b></th><th align=\"center\"><b>Fecha Entrega</b></th></tr></thead>");
                RptContent.Append("<tbody>");

                List<Oportunidad> Oportunidades = null;

                if (Helpers.ApplicationContext.CurrentUser.IsSeller)
                {
                    Oportunidades = db.Oportunidad.Include(a => a.Vendedor).Include(a => a.Clientes).Where(a => a.UserID == Helpers.ApplicationContext.CurrentUser.Id && a.Estado == Estado).OrderBy(s => s.FechaEntrega).ToList();
                }
                else
                {
                    Oportunidades = db.Oportunidad.Include(a => a.Vendedor).Include(a => a.Clientes).Where(a => a.Estado == Estado).OrderBy(s => s.FechaEntrega).ToList();
                }

                if (ClienteID.HasValue)
                    Oportunidades = Oportunidades.Where(c => c.ClienteID == ClienteID.Value).ToList();

                if (VendedorID.HasValue)
                    Oportunidades = Oportunidades.Where(c => c.VendedorID == VendedorID.Value).ToList();
                
                if (FecIni.HasValue)
                    Oportunidades = Oportunidades.Where(c => c.FechaEntrega >= FecIni.Value).ToList();

                if (FecFin.HasValue)
                    Oportunidades = Oportunidades.Where(c => c.FechaEntrega <= FecFin.Value).ToList();

                if (Oportunidades != null && Oportunidades.Count() > 0)
                {
                    Oportunidades = Oportunidades.OrderBy(o => o.FechaSolicitud).ToList();

                    foreach (var e in Oportunidades)
                        RptContent.Append($"<tr><td>{e.FechaSolicitud}</td><td>{e.NombreOportunidad}</td><td>{e.Clientes.Nombre}</td><td>{e.Vendedor.NombreVendedor}</td><td>{e.Estado}</td><td>{e.FechaEntrega}</td></tr>");
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
        /// Build Cotizaciones Report
        /// </summary>
        /// <param name="ClienteID"></param>
        /// <param name="VendedorID"></param>
        /// <param name="FecIni"></param>
        /// <param name="FecFin"></param>
        /// <returns></returns>
        public ActionResult BuildRptCotizaciones(int? ClienteID, int? VendedorID, DateTime? FecIni, DateTime? FecFin)
        {
            try
            {
                StringBuilder RptContent = new StringBuilder();
                RptContent.Append("<thead><tr><th align=\"center\"><b>Nro. Cotización</b></th><th align=\"center\" ><b>Fecha</b></th><th align=\"center\" ><b>Cliente</b></th><th align=\"center\" ><b>Vendedor</b></th><th align=\"center\"><b>Oportunidad</b></th><th align=\"center\"><b>Valor Total</b></th></tr></thead>");
                RptContent.Append("<tbody>");

                List<Cotizacion> Cotizaciones = null;

                if (ApplicationContext.CurrentUser.IsSuperAdmin)
                {
                    Cotizaciones = db.Cotizacion.Include(a => a.Vendedor).Include(a => a.Cliente).ToList();
                }
                else
                {
                    Cotizaciones = db.Cotizacion.Include(a => a.Vendedor).Include(a => a.Cliente).Where(c => c.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID).ToList();
                }
                
                if (ClienteID.HasValue)
                    Cotizaciones = Cotizaciones.Where(c => c.ClienteID == ClienteID.Value).ToList();

                if (VendedorID.HasValue)
                    Cotizaciones = Cotizaciones.Where(c => c.VendedorID == VendedorID.Value).ToList();

                if (FecIni.HasValue)
                    Cotizaciones = Cotizaciones.Where(c => c.Fecha >= FecIni.Value).ToList();

                if (FecFin.HasValue)
                    Cotizaciones = Cotizaciones.Where(c => c.Fecha <= FecFin.Value).ToList();

                if (Cotizaciones != null && Cotizaciones.Count() > 0)
                {
                    Cotizaciones = Cotizaciones.OrderBy(o => o.Fecha).ToList();
                    string NomOpo = string.Empty;

                    foreach (var e in Cotizaciones)
                    {
                        NomOpo = string.Empty;

                        if (e.Oportunidad != null)
                            NomOpo = e.Oportunidad.NombreOportunidad;

                        RptContent.Append($"<tr><td>{e.NumberID}</td><td>{e.Fecha}</td><td>{e.Cliente.Nombre}</td><td>{e.Vendedor.NombreVendedor}</td><td>{NomOpo}</td><td align=\"right\">{e.Valor.ToString("C2")}</td></tr>");
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
        /// Set ViewBag Data
        /// </summary>
        private void SetViewBagListData()
        {
            if (ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(s => s.Nombre), "ClienteID", "Nombre", null);
                ViewBag.VendedorID = new SelectList(db.Vendedores.OrderBy(s => s.NombreVendedor), "VendedorID", "NombreVendedor", null);
            }
            else
            {
                ViewBag.ClienteID = new SelectList(db.Clientes.Where(c => c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID)).OrderBy(s => s.Nombre), "ClienteID", "Nombre", null);
                ViewBag.VendedorID = new SelectList(db.Vendedores.Where(e => e.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID)).OrderBy(s => s.NombreVendedor), "VendedorID", "NombreVendedor", null);
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