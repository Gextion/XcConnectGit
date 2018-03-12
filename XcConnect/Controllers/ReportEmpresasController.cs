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
    public class ReportEmpresasController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private CRMContext db = new CRMContext();

        /// <summary>
        /// View Consumos Report
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Empresas()
        {
            SetViewBagListData();

            RptEmpresas ViewModel = new RptEmpresas() { };

            return View(ViewModel);
        }

        /// <summary>   
        /// Build Report Empresas
        /// </summary>
        /// <returns></returns>        
        public ActionResult BuildRptEmpresas(int? CiudadID)
        {
            try
            {
                StringBuilder RptContent = new StringBuilder();
                RptContent.Append(@"<thead><tr><th align = ""center""><b> Empresa </b></th><th align = ""center""><b> Nit </b></th><th align = ""center""><b> Ciudad </b></th><th align = ""center""><b> Dirección </b></th><th align = ""center""><b> Teléfono </b></th><th align = ""center""><b> Celular </b></th><th align = ""center""><b> Correo </b></th></tr></thead>");
                RptContent.Append("<tbody>");

                List<Empresa> Empresa = null;

                if (CiudadID.HasValue && CiudadID.Value != 999999)
                {
                        Empresa = db.Empresas.Include(e => e.Ciudad).Where(e => e.CiudadID == CiudadID.Value).ToList();
                }
                else
                {
                        Empresa = db.Empresas.Include(e => e.Ciudad).ToList();
                }

                var Empresas = Empresa.OrderBy(e => e.Nombre).ThenBy(e => e.Ciudad.Ciudad);

                if (Empresas != null && Empresas.Count() > 0)
                {
                    foreach (var e in Empresas)
                    {
                        RptContent.Append($"<tr><td>{e.Nombre}</td><td>{e.Nit}</td><td>{e.Ciudad.Ciudad}</td><td>{e.Direccion}</td><td>{e.Telefono}</td><td>{e.Celular}</td><td>{e.Email}</td></tr>");
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
        /// Build Report
        /// </summary>
        /// <returns></returns>        
        //public ActionResult BuildRptConsumos(int? EmpresaID, int? SedeID, int? FuenteID, int? DispositivoID, DateTime? FecIni, DateTime? FecFin)
        //{
        //    try
        //    {
        //        StringBuilder RptContent = new StringBuilder();
        //        RptContent.Append(@"<thead><tr><th align = ""center""><b> Sede </b></th><th align = ""center""><b> Fuente </b></th><th align = ""center""><b> Medidor </b></th><th align = ""center""><b> Fecha Inicial </b></th><th align = ""center""><b> Fecha Final </b></th><th align = ""center""><b> Consumo </b></th><th align = ""center""><b> Línea Base </b></th><th align = ""center""><b> Valor </b></th><th align = ""center""><b> Valor Unitario </b></th></tr></thead>");
        //        RptContent.Append("<tbody>");

        //        List<Empresa> Empresas = null;

        //        if (EmpresaID.HasValue)
        //        {
        //            Empresas = db.Empresas.Include(e => e.Ciudad).Include(e => e.SectorEconomico).Where(e => e.EmpresaID.Equals(EmpresaID.Value)).ToList();
        //        }
        //        else
        //        {
        //            Empresas = db.Empresas.Include(e => e.Sedes).Include(e => e.Ciudad).Include(e => e.SectorEconomico).ToList();
        //        }

        //        if (SedeID.HasValue && SedeID.Value != 999999)
        //        {
        //            Empresas = (from emp in Empresas
        //                        join sed in db.Sedes on emp.EmpresaID equals sed.EmpresaID
        //                        where sed.SedeID.Equals(SedeID.Value)
        //                        select emp).ToList();
        //        }

        //        if (Empresas != null && Empresas.Count > 0)
        //        {
        //            Empresas.ForEach(e =>
        //            {
        //                decimal TotSecConsumo = 0;
        //                decimal TotSecLinBase = 0;
        //                decimal TotSecValor = 0;
        //                decimal TotSecValorUni = 0;

        //                string Ciudad = "";

        //                if (e.Ciudad != null)
        //                    Ciudad = e.Ciudad.Ciudad;

        //                foreach (Sede sede in e.Sedes)
        //                {
        //                    var Dispositivos = db.Dispositivos.Include(d => d.Fuente).Where(d => d.SedeID.Equals(sede.SedeID)).ToList();

        //                    if (DispositivoID.HasValue && DispositivoID.Value != 999999)
        //                        Dispositivos = (Dispositivos.Where(d => d.DispositivoID.Equals(DispositivoID.Value))).ToList();

        //                    if (FuenteID.HasValue)
        //                        Dispositivos = (Dispositivos.Where(d => d.FuenteID.Equals(FuenteID.Value))).ToList();

        //                    if (Dispositivos != null && Dispositivos.Count > 0)
        //                    {
        //                        List<string> FuentesSede = new List<string>();

        //                        foreach (var dispositivo in Dispositivos)
        //                        {
        //                            if (dispositivo.Fuente != null && !String.IsNullOrEmpty(dispositivo.Fuente.Fuente))
        //                            {
        //                                if (!FuentesSede.Contains(dispositivo.Fuente.Fuente))
        //                                    FuentesSede.Add(dispositivo.Fuente.Fuente);
        //                            }
        //                        }

        //                        if (FuentesSede != null && FuentesSede.Count > 0)
        //                        {
        //                            decimal TotSedConsumo = 0;
        //                            decimal TotSedLinBase = 0;
        //                            decimal TotSedValor = 0;

        //                            FuentesSede.Sort();

        //                            RptContent.Append($"<tr><td colspan=\"9\">{sede.NombreSede}</td></tr>");

        //                            foreach (string nombreFuente in FuentesSede)
        //                            {
        //                                var DispositivosFuente = db.Dispositivos.Include(d => d.Fuente).Include(d => d.Sede).Include(d => d.Consumos).Where(d => d.Fuente.Fuente.Equals(nombreFuente) && d.SedeID.Equals(sede.SedeID)).ToList();

        //                                if (DispositivoID.HasValue && DispositivoID.Value != 999999)
        //                                    DispositivosFuente = (DispositivosFuente.Where(d => d.DispositivoID.Equals(DispositivoID.Value))).ToList();

        //                                if (DispositivosFuente != null)
        //                                {
        //                                    DispositivosFuente.ForEach(d =>
        //                                    {
        //                                        decimal TotConsumo = 0;
        //                                        decimal TotLinBase = 0;
        //                                        decimal TotValor = 0;

        //                                        List<Consumo> Consumos = d.Consumos.ToList();

        //                                        if (FecIni.HasValue)
        //                                            Consumos = Consumos.Where(c => c.FechaInicial >= FecIni.Value).ToList();

        //                                        if (FecFin.HasValue)
        //                                            Consumos = Consumos.Where(c => c.FechaInicial <= FecFin.Value).ToList();

        //                                        if (Consumos != null && Consumos.Count > 0)
        //                                        {
        //                                            RptContent.Append($"<tr><td colspan=\"1\"></td><td colspan=\"8\">{nombreFuente}</td></tr>");

        //                                            foreach (Consumo consDis in Consumos)
        //                                            {
        //                                                TotConsumo += consDis.ConsumoPeriodo;
        //                                                TotLinBase += consDis.LineaBase;
        //                                                TotValor += consDis.Valor;

        //                                                RptContent.Append($"<tr><td colspan=\"2\"></td><td>{d.Nombre}</td><td align=\"right\" >{consDis.FechaInicial.ToShortDateString()}</td><td align=\"right\" >{consDis.FechaFinal.ToShortDateString()}</td><td align=\"right\" >{consDis.ConsumoPeriodo.ToString("N2")}</td><td align=\"right\" >{consDis.LineaBase.ToString("N2")}</td><td align=\"right\" >{consDis.Valor.ToString("C2")}</td><td align=\"right\" >{consDis.ValorUnitario.ToString("C2")}</td></tr>");
        //                                            }

        //                                            TotSedConsumo += TotConsumo;
        //                                            TotSedLinBase += TotLinBase;
        //                                            TotSedValor += TotValor;

        //                                            RptContent.Append($"<tr><td colspan=\"2\"></td><td colspan=\"3\" ><b>Total {nombreFuente} - {sede.NombreSede} - {d.Nombre}</b></td><td align=\"right\" ><b>{TotConsumo.ToString("N2")}</b></td><td align=\"right\" ><b>{TotLinBase.ToString("N2")}</b></td><td align=\"right\" ><b>{TotValor.ToString("C2")}</b></td><td align=\"right\"><b></b></td></tr>");
        //                                            RptContent.Append($"<tr><td colspan=\"9\"></td></tr>");
        //                                        }
        //                                    });
        //                                }
        //                            }

        //                            RptContent.Append($"<tr><td colspan=\"5\"><b>Total {sede.NombreSede}</b></td><td align=\"right\" ><b>{TotSedConsumo.ToString("N2")}</b></td><td align=\"right\" ><b>{TotSedLinBase.ToString("N2")}</b></td><td align=\"right\" ><b>{TotSedValor.ToString("C2")}</b></td><td align=\"right\"><b></b></td></tr>");
        //                            RptContent.Append($"<tr><td colspan=\"9\"></td></tr>");

        //                            TotSecConsumo += TotSedConsumo;
        //                            TotSecLinBase += TotSedLinBase;
        //                            TotSecValor += TotSedValor;
        //                        }
        //                    }
        //                }
        //            });
        //        }

        //        RptContent.Append("</tbody>");

        //        var result = new JsonResult
        //        {
        //            Data = new { RptContent = RptContent.ToString(), Success = true },
        //            ContentEncoding = System.Text.Encoding.UTF8,
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //        };

        //        result.MaxJsonLength = int.MaxValue;

        //        return result;
        //    }
        //    catch
        //    {
        //        return new JsonResult
        //        {
        //            Data = new { Message = "Error ejecutando la acción solicitada. Por favor inténtelo de nuevo", Success = false },
        //            ContentEncoding = Encoding.UTF8,
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //        };
        //    }
        //}

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