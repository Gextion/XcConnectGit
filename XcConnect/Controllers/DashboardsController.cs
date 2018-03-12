using XcConnect.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System;
using XcConnect.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace XcConnect.Controllers
{
    /// <summary>
    /// CSP Config
    /// https://docs.nwebsec.com/en/stable/nwebsec/Configuring-csp.html
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class DashboardsController : Controller
    {
        private CRMContext db = new CRMContext();

        Dashboard dashboard = new Dashboard();

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Dashboard()
        {
            SetViewBagListData();
            DateTime fechaFinal = DateTime.Today.AddDays(7);

            Dashboard dashboard = new Dashboard()
            {
                Actividades = db.Actividades.Where(
                                        a => a.FechaEntrega >= DateTime.Today && a.FechaEntrega <= fechaFinal
                                        && a.UserID == Helpers.ApplicationContext.CurrentUser.Id
                                        ).OrderBy(a => a.FechaEntrega).ToList()
            };

            return View(dashboard);
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult DatosActividades(int? empresa, int? tipoactividad, int? anio, int? mes, string user)
        {
            SqlParameter pEmpresa = new SqlParameter("@EmpresaID", empresa);
            SqlParameter pTipoActividad = new SqlParameter("@TipoActividadID", tipoactividad);
            SqlParameter pAño = new SqlParameter("@Año", anio);
            SqlParameter pMes = new SqlParameter("@Mes", mes);
            SqlParameter pUser = new SqlParameter("@UsuarioID", user);

            var Actividades = db.Database.SqlQuery<DatosActividades>(
                                @"spDashboard_Actividades @EmpresaID, @TipoActividadID, @Año, @Mes, @UsuarioID",
                                pEmpresa, pTipoActividad, pAño, pMes, pUser).ToList();

            var actividadesDistinct = Actividades.GroupBy(a => a.TipoActividad)
                                                 .Select(a => a.First().TipoActividad)
                                                 .ToList();

            var dataActividades = @"{ ""labels"":" + $"[\"{ string.Join("\",\"", actividadesDistinct) }\"]" + "," + @"""datasets"": [" ;
            string mesx = "";

            foreach (var actividad in Actividades)
            {
                if (mesx != actividad.NombreMes)
                {
                    if (mesx.Length > 0)
                    {
                        dataActividades = dataActividades.Remove(dataActividades.Length - 1, 1) + @"], ""backgroundColor"": """ + actividad.RGBA_Background + @""" }, ";
                    }
                    mesx = actividad.NombreMes;
                    dataActividades = dataActividades + @"{ ""label"": """ + mesx + @""", ""data"": [";
                }
                dataActividades = dataActividades + actividad.Cantidad + ",";
            }

            var ultimaActividad  = Actividades.Select(a => a.RGBA_Background).LastOrDefault();
            if (ultimaActividad !=null)
            {
                dataActividades = dataActividades.Remove(dataActividades.Length - 1, 1) + @"], ""backgroundColor"": """ + ultimaActividad + @""" }]}";
            }

            return new JsonResult { Data = new { datasource = dataActividades, success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            //        {
            //            ""label"": ""Consumo de Energía"",
            //            ""backgroundColor"": ""rgba(26,179,148,0.5)"",
            //            ""borderColor"": ""rgba(26,179,148,0.7)"",
            //            ""pointBackgroundColor"": ""rgba(26,179,148,1)"",
            //            ""pointBorderColor"": ""#fff"",
            //            ""data"": " + $"[{ string.Join(",", Actividades.Select(c => c.Cantidad)) }]" + @"
            //        }
            //    ]
            //}";

        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult DatosOportunidades(int? empresa, int? anio, int? mes, string user)
        {
            SqlParameter pEmpresa = new SqlParameter("@EmpresaID", empresa);
            SqlParameter pAño = new SqlParameter("@Año", anio);
            SqlParameter pMes = new SqlParameter("@Mes", mes);
            SqlParameter pUser = new SqlParameter("@UsuarioID", user);

            var Oportunidades = db.Database.SqlQuery<DatosOportunidades>(
                                            @"spDashboard_Oportunidades @EmpresaID, @Año, @Mes, @UsuarioID",
                                            pEmpresa, pAño, pMes, pUser).ToList();

            var dataOportunidades = @"{ ""labels"":" + $"[\"{ string.Join("\",\"", Oportunidades.Select(o => o.EstadoOportunidad)) }\"]" + ", "  ;

            var data = @"""datasets"": [{""data"" : [" ;
            var bgColor = @"""backgroundColor"": [";

            foreach (var oportunidad in Oportunidades)
            {
                data = data + oportunidad.Cantidad + ", ";
                bgColor = bgColor + @"""" + oportunidad.RGBA_Background + @""", ";
            }

            dataOportunidades = dataOportunidades + data.Remove(data.Length - 2, 2) + "], ";
            dataOportunidades = dataOportunidades + bgColor.Remove(bgColor.Length - 2, 2) + "] } ] }";

            return new JsonResult { Data = new { datasource = dataOportunidades, success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private void SetViewBagListData()
        {
            UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SecurityDbContext()));

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", Helpers.ApplicationContext.CurrentUser.EmpresaID);
                ViewBag.TipoActividadID = new SelectList(db.TipoActividad.OrderBy(t => t.NombreTipoActividad), "TipoActividadID", "NombreTipoActividad");
                ViewBag.UserID = new SelectList(_userManager.Users.OrderBy(u => u.UserName), "ID", "UserName");
            }
            else
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas
                                                     .Where(s => s.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID)).OrderBy(e => e.Nombre), "EmpresaID", "Nombre");
                ViewBag.TipoActividadID = new SelectList(db.TipoActividad.OrderBy(t => t.NombreTipoActividad), "TipoActividadID", "NombreTipoActividad");
                ViewBag.UserID = new SelectList(_userManager.Users.Where(u => u.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID))
                                                   .OrderBy(u => u.UserName),  "ID", "UserName");
            }

            ViewBag.AnioID = new SelectList(db.Años, "AnioID", "Año");
            ViewBag.MesID = new SelectList(db.Meses, "MesID", "Mes");

            //ViewBag.Actividades = db.Actividades.Where(a => a.FechaEntrega >= DateTime.Today && a.FechaEntrega <= fechaFinal).ToList();

        }

    }
}