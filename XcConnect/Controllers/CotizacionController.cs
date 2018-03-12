using XcConnect.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace XcConnect.Controllers
{
    /// <summary>
    /// ConsumoesController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class CotizacionController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private CRMContext db = new CRMContext();

        // GET: Cotizacion
        public ActionResult Index()
        {
            if (Helpers.ApplicationContext.CurrentUser == null || Helpers.ApplicationContext.CurrentUser.Empresa == null)
                return View("../Authentication/Login");

            ViewBag.Caption = "Cotizaciones";

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", Helpers.ApplicationContext.CurrentUser.EmpresaID);

            var cotizaciones = (from c in db.Cotizacion.Include(c => c.Empresa)
                           where c.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                           select c);

            return View("Index", cotizaciones.ToList());
        }

        /// <summary>
        /// Print View By Cotizacion ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Print(int id)
        {
            Cotizacion cotizacion = db.Cotizacion
                .Include(c => c.Items)
                .Include(c => c.Empresa)
                .Include(c => c.Cliente)
                .Include(c => c.Vendedor)
                .Include(c => c.Oportunidad)
                .Include(c => c.Actividades)
                .Where(c => c.CotizacionID == id).FirstOrDefault();

            try
            {
                if (cotizacion != null)
                {
                    StringBuilder RptContent = GetCotizacionContentToPrint(cotizacion);

                    return View("Print", new CotizacionPrint()
                    {
                        Content = RptContent.ToString(),
                        CotizacionIDPK = cotizacion.CotizacionID,
                        CotizacionID = cotizacion.NumberID,
                        Fecha = cotizacion.Fecha.ToShortDateString(),
                        Empresa = cotizacion.Empresa.Nombre,
                        Cliente = cotizacion.Cliente.Nombre,
                        ClienteID = cotizacion.ClienteID,
                        Vendedor = cotizacion.Vendedor.NombreVendedor,
                        Total = cotizacion.Valor.ToString("C2")
                    });
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { Message = "No es posible recuperar la cotización con el ID recibido. Por favor inténtelo de nuevo", Success = false },
                        ContentEncoding = Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
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
        /// Send From Email
        /// </summary>
        /// <param name="CliID">Client ID</param>
        /// <returns></returns>        
        public ActionResult SendByEmail(int CliID, int CotID)
        {
            try
            {

                Cliente cliente = db.Clientes.Find(CliID);
                if (cliente != null)
                {
                    if (String.IsNullOrEmpty(cliente.Email))
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "El cliente seleccionado no cuenta con un correo electrónico válido. Por favor, seleccione un cliente válido e inténtelo de nuevo.", Success = false },
                            ContentEncoding = Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        Cotizacion cotizacion = db.Cotizacion.Include(c => c.Items).Include(c => c.Empresa).Include(c => c.Cliente)
                            .Include(c => c.Vendedor).Include(c => c.Oportunidad).Include(c => c.Actividades)
                            .Where(c => c.CotizacionID == CotID).FirstOrDefault();

                        if (cotizacion != null)
                        {
                            string FileName = $"Cotizacion_{Guid.NewGuid().ToString()}.pdf";
                            string PDFPath = Server.MapPath($"~/Uploads/PDF/{FileName}");

                            StringBuilder RptContent = GetCotizacionContentToPrint(cotizacion);

                            string AllRpt = $"<div id=\"customprintarea\"><center><h1>Cotización Nro. {cotizacion.NumberID} - {cotizacion.Fecha.ToShortDateString()}</h1></center><br /><div><div class=\"row\"><div class=\"col-md-4\"><b>Empresa: </b>{cotizacion.Empresa.Nombre}<br /></div><div class=\"col-md-4\"><b>Cliente: </b>{cotizacion.Cliente.Nombre}<br /></div><div class=\"col-md-4\"><b>Vendedor: </b>{cotizacion.Vendedor.NombreVendedor}<br /></div></div><div class=\"row\"><div class=\"col-md-4\"><b>Valor Total: </b>{cotizacion.Valor.ToString("C2")}<br /></div></div></div><br /><br /><h2>Productos</h2><br /><div id=\"contentRaw\" class=\"table-responsive\">{RptContent.ToString()}</div></div>";
                            
                            var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(AllRpt, PdfSharp.PageSize.A4);
                            pdf.Save(PDFPath);

                            if (cotizacion.Oportunidad != null && cotizacion.OportunidadID.HasValue)
                            {
                                OportunidadesArchivos OpoFile = new OportunidadesArchivos();
                                OpoFile.Oportunidad = cotizacion.Oportunidad;
                                OpoFile.OportunidadID = cotizacion.OportunidadID.Value;
                                OpoFile.ArchivoNombre = $"Cotización ({cotizacion.Cliente.Nombre}) {Helpers.DateHelper.GetColombiaDateTime().ToShortDateString()}";
                                OpoFile.ArchivoUrl = $"{Request.Url.GetLeftPart(UriPartial.Authority)}{PDFPath}";
                                OpoFile.LocalUrl = PDFPath;

                                cotizacion.Oportunidad.Archivos.Add(OpoFile);

                                db.OportunidadArchivos.Add(OpoFile);
                                db.SaveChanges();
                            }

                            if (SendCotizacionByEmail(PDFPath, cotizacion.Cliente.Nombre, cotizacion.NumberID, cotizacion.Empresa.Nombre, cotizacion.Vendedor.NombreVendedor, cotizacion.Vendedor.Email, cotizacion.Vendedor.Celular, cliente.Email))
                            {
                                return new JsonResult
                                {
                                    Data = new { Message = "Cotización enviada.", Success = true },
                                    ContentEncoding = Encoding.UTF8,
                                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };
                            }
                            else
                            {
                                return new JsonResult
                                {
                                    Data = new { Message = "No fue posible enviar la cotización automáticamente. Por favor inténtelo de nuevo.", Success = false },
                                    ContentEncoding = Encoding.UTF8,
                                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };
                            }

                            // Docs: 
                            // http://htmlrenderer.codeplex.com
                            //

                            //
                            // Sample To Download AS a File
                            // 
                            //public static Byte[] PdfSharpConvert(String html)
                            //{
                            //    Byte[] res = null;
                            //    using (MemoryStream ms = new MemoryStream())
                            //    {
                            //        var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                            //        pdf.Save(ms);
                            //        res = ms.ToArray();
                            //    }
                            //    return res;
                            //}
                        }
                        else
                        {
                            return new JsonResult
                            {
                                Data = new { Message = "No es posible recuperar la cotización con el ID recibido. Por favor inténtelo de nuevo", Success = false },
                                ContentEncoding = Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }
                    }
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { Message = "El cliente es requerido. Por favor, seleccione un cliente e inténtelo de nuevo.", Success = false },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Send Cotizacion By Email
        /// </summary>
        /// <param name="pDFPath"></param>
        /// <param name="NombreCliente"></param>
        /// <param name="CotNumber"></param>
        /// <param name="NombreEmpresa"></param>
        /// <param name="NombreVendedor"></param>
        /// <param name="EmailVendedor"></param>
        /// <param name="NumeroCelular"></param>
        /// <returns></returns>
        private bool SendCotizacionByEmail(string pDFPath, string NombreCliente, string CotNumber, string NombreEmpresa, string NombreVendedor, string EmailVendedor, long NumeroCelular, string ClienteEmail)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    MailMessage Mail = new MailMessage();
                    Mail.From = new MailAddress(ConfigurationManager.AppSettings["SmtpUsrCredentials"], ConfigurationManager.AppSettings["SmtpUsrDisplayName"]);
                    Mail.To.Add(new MailAddress(ClienteEmail));

                    Mail.Subject = $"Cotización";
                    Mail.IsBodyHtml = true;

                    var att = new Attachment(pDFPath);
                    att.ContentType = new System.Net.Mime.ContentType("application/pdf");

                    Mail.Attachments.Add(att);

                    string BodyHTML = Properties.EmailTemplates.CotizacionPDF;
                    BodyHTML = BodyHTML.Replace("[NOM_CLI]", NombreCliente);
                    BodyHTML = BodyHTML.Replace("[NOM_EMP]", NombreEmpresa);
                    BodyHTML = BodyHTML.Replace("[COT_NUM]", CotNumber);
                    BodyHTML = BodyHTML.Replace("[NOM_VEN]", NombreVendedor);
                    BodyHTML = BodyHTML.Replace("[EMAIL_VEN]", EmailVendedor);
                    BodyHTML = BodyHTML.Replace("[CELL_VEN]", NumeroCelular.ToString());
                    
                    Mail.Body = BodyHTML;

                    if (bool.Parse(ConfigurationManager.AppSettings["SmtpUseSecurityData"]))
                    {
                        smtp.UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["SmtpUseDefaultCredentials"]);
                        smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUsrCredentials"], ConfigurationManager.AppSettings["SmtpPwdCredentials"]);
                        smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
                    }

                    smtp.Send(Mail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get CotizacionContentToPrint
        /// </summary>
        /// <param name="cotizacion"></param>
        /// <returns></returns>
        private static StringBuilder GetCotizacionContentToPrint(Cotizacion cotizacion)
        {
            StringBuilder RptContent = new StringBuilder();
            RptContent.Append("<table id='rptTable' class='table table-hover'> <thead> <tr> <th>Código</th><th>Nombre</th><th>Especificaciones</th><th align='center'>Cant.</th><th align='center'>Val. Uni.</th><th align='center'>Val. Tot.</th></tr></thead>");
            RptContent.Append("<tbody>");

            foreach (var item in cotizacion.Items)
            {
                RptContent.Append($"<tr><td>{item.Producto.Codigo}</td><td>{item.Producto.NombreProducto}</td><td>{item.Producto.Especificaciones}</td><td align='right'>{item.Cantidad.ToString("N2")}</td><td align='right'>{item.ValUnitario.ToString("N2")}</td><td align='right'>{item.ValTotal.ToString("N2")}</td></tr>");
            }

            RptContent.Append($"<tr><td colspan='6'></td></tr>");
            RptContent.Append($"<tr><td colspan='5'><b>Total</b></td><td align='right' ><b>{cotizacion.Valor.ToString("C2")}</b></td></tr>");

            RptContent.Append($"<tr><td colspan='6'></td></tr>");
            RptContent.Append($"<tr><td colspan='6'></td></tr>");
            RptContent.Append($"<tr><td td align='right' colspan='6'>Fecha y Hora de Impresión: {Helpers.DateHelper.GetColombiaDateTime()}</td></tr>");

            RptContent.Append("</tbody>");
            return RptContent;
        }

        /// <summary>
        /// View in List Mode
        /// <example> GET: Dispositivos </example>
        /// <param name="idSede"></param>
        /// </summary>    
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult FilterByEmpresaID(int? id)
        {
            if (id != null && id.HasValue)
            {
                var Cotizaciones = (from c in db.Cotizacion where c.EmpresaID == id.Value select c).ToList();
                
                var Empresa = db.Empresas.Where(e => e.EmpresaID.Equals(id.Value)).FirstOrDefault();
                if (Empresa != null)
                {
                    ViewBag.Caption = $"Cotizaciones de la empresa: {Empresa.Nombre}";
                }
                else
                {
                    ViewBag.Caption = "Cotizaciones";
                }

                if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                    ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", id.Value);

                return View("Index", Cotizaciones);
            }
            else
            {
                return Index();
            }
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: Consumoes/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Cotizacion cotizacion = db.Cotizacion.Find(id);

            if (cotizacion == null)
                return HttpNotFound();

            return View(cotizacion);
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
                Transaction = db.Database.BeginTransaction();

                Cotizacion cotizacion = db.Cotizacion.Include(c => c.Items).Where(c => c.CotizacionID == id).FirstOrDefault();
                if (cotizacion != null)
                {
                    if (cotizacion.Items != null && cotizacion.Items.Count > 0)
                    {
                        List<CotizacionDetalle> ItemsToDelete = cotizacion.Items.ToList();

                        foreach (var CotizacionItem in ItemsToDelete)
                            db.CotizacionDetalle.Remove(CotizacionItem);
                    }

                    if (cotizacion.ActividadID.HasValue)
                    {
                        var Act = db.Actividades.Where(a => a.ActividadID == cotizacion.ActividadID.Value).FirstOrDefault();
                        if (Act != null)
                        {
                            db.Actividades.Remove(Act);
                            db.SaveChanges();
                        }
                    }

                    db.Cotizacion.Remove(cotizacion);
                    db.SaveChanges();
                }

                Transaction.Commit();

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

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        
        /// <summary>
        /// Action Create New Object
        /// <example> GET: Consumoes/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Manage(int? id)
        {
            if (id.HasValue)
            {
                var Cotizacion = db.Cotizacion.Include(c => c.Items).Where(c => c.CotizacionID == id).FirstOrDefault();
                if (Cotizacion != null)
                {
                    Cotizacion.Fecha = Helpers.DateHelper.GetColombiaDateTime();
                    Cotizacion.IsSaved = true;

                    SetViewBagData(Cotizacion.VendedorID, Cotizacion.ClienteID, Cotizacion.OportunidadID, Cotizacion.EmpresaID);
                }

                return View(Cotizacion);
            }
            else
            {
                SetViewBagData();

                Cotizacion Cot = new Cotizacion() {
                    Fecha = Helpers.DateHelper.GetColombiaDateTime(),
                    IsSaved = false };

                return View(Cot);
            }
        }

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action Create New Object
        /// <example> POST: Consumoes/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "CotizacionID,IsSaved,EmpresaID,ClienteID,VendedorID,OportunidadID,ActividadID,Valor,NumberID,Fecha")] Cotizacion cotizacion)
        {
            if (ModelState.IsValid)
            {
                DbContextTransaction Transaction = null;

                try
                {
                    if (!cotizacion.IsSaved)
                    {
                        Transaction = db.Database.BeginTransaction();

                        var Consecutivo = db.CotizacionConsecutivo.Where(cc => cc.EmpresaID == cotizacion.EmpresaID).FirstOrDefault();
                        if (Consecutivo == null)
                        {
                            Consecutivo = new CotizacionConsecutivo() { EmpresaID = cotizacion.EmpresaID, ValorConsecutivo = 0 };
                            db.CotizacionConsecutivo.Add(Consecutivo);
                            db.SaveChanges();
                        }

                        cotizacion.NumberID = (Consecutivo.ValorConsecutivo + 1).ToString("0000");
                        cotizacion.Valor = 0;

                        Consecutivo.ValorConsecutivo = Consecutivo.ValorConsecutivo + 1;
                        db.Cotizacion.Add(cotizacion);

                        db.SaveChanges();

                        AddActivityTo(cotizacion);

                        Transaction.Commit();
                    }
                    else
                    {
                        db.Entry(cotizacion).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    decimal totalC = 0;
                    cotizacion = db.Cotizacion.Include(c => c.Items).Where(c => c.CotizacionID == cotizacion.CotizacionID).FirstOrDefault();
                    if (cotizacion != null)
                    {
                        foreach (var item in cotizacion.Items)
                            totalC += item.ValTotal;

                        cotizacion.Valor = totalC;
                        db.Entry(cotizacion).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception eX)
                {
                    if (Transaction != null)
                        Transaction.Rollback();

                    ModelState.AddModelError("", eX.Message);
                }
            }

            SetViewBagData(cotizacion.VendedorID, cotizacion.ClienteID, cotizacion.OportunidadID);
            return View("Manage", cotizacion);
        }

        /// <summary>
        /// Add Activity to Model
        /// </summary>
        /// <param name="cotizacion"></param>
        private void AddActivityTo(Cotizacion cotizacion)
        {
            string NombreCliente = string.Empty;

            if (cotizacion.Cliente != null)
            {
                NombreCliente = cotizacion.Cliente.Nombre;
            }
            else
            {
                Cliente Cl = db.Clientes.Where(cl => cl.ClienteID == cotizacion.ClienteID).FirstOrDefault();
                if (Cl != null)
                    NombreCliente = Cl.Nombre;
            }

            Actividades newAct = new Actividades()
            {
                Descripcion = $"Seguimiento Cotización Nro {cotizacion.NumberID} - {NombreCliente}",
                EmpresaID = cotizacion.EmpresaID,
                TipoActividadID = int.Parse(ConfigurationManager.AppSettings["TipoActividadSeguimientoID"]),
                FechaEntrega = Helpers.DateHelper.GetColombiaDateTime().AddDays(7),
                FechaRegistro = Helpers.DateHelper.GetColombiaDateTime(),
            };

            db.Actividades.Add(newAct);
            db.SaveChanges();

            cotizacion.ActividadID = newAct.ActividadID;
            db.Entry(cotizacion).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Set ViewBag Dispositivos
        /// </summary>
        private void SetViewBagData(int? DefaultVendedorID = null, int? DefaultClienteID = null, int? DefaultOportunidadID = null, int? DefaultEmpresaID = null)
        {
            object DefVendedorID = null;
            object DefClienteID = null;
            object DefOportunidadID = null;
            object DefEmpID = Helpers.ApplicationContext.CurrentUser.EmpresaID;

            if (DefaultVendedorID.HasValue)
                DefVendedorID = DefaultVendedorID.Value;

            if (DefaultClienteID.HasValue)
                DefClienteID = DefaultClienteID.Value;

            if (DefaultOportunidadID.HasValue)
                DefOportunidadID = DefaultOportunidadID.Value;

            if (DefaultEmpresaID.HasValue)
                DefEmpID = DefaultEmpresaID.Value;

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", DefEmpID);
                ViewBag.VendedorID = new SelectList(db.Vendedores.OrderBy(d => d.NombreVendedor), "VendedorID", "NombreVendedor", DefVendedorID);
                ViewBag.ClienteID = new SelectList(db.Clientes.OrderBy(s => s.Nombre), "ClienteID", "Nombre", DefClienteID);
                ViewBag.OportunidadID = new SelectList(db.Oportunidad.OrderBy(s => s.NombreOportunidad), "OportunidadID", "NombreOportunidad", DefOportunidadID);
                ViewBag.ItemProductoID = new SelectList(db.Productos.OrderBy(s => s.NombreProducto), "ProductoID", "NombreProducto", null);
            }
            else
            {   
                ViewBag.EmpresaID = new SelectList(db.Empresas.Where(e => e.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID).OrderBy(e => e.Nombre), "EmpresaID", "Nombre", DefEmpID);
                ViewBag.VendedorID = new SelectList(db.Vendedores.Where(e => e.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID).OrderBy(d => d.NombreVendedor), "VendedorID", "NombreVendedor", DefVendedorID);
                ViewBag.ClienteID = new SelectList(db.Clientes.Where(e => e.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID).OrderBy(s => s.Nombre), "ClienteID", "Nombre", DefClienteID);
                ViewBag.ItemProductoID = new SelectList(db.Productos.Where(e => e.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID).OrderBy(s => s.NombreProducto), "ProductoID", "NombreProducto", null);

                var oportunidades = new SelectList((from opo in db.Oportunidad
                                                   join cli in db.Clientes on opo.ClienteID equals cli.ClienteID
                                                   where cli.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                                                   orderby opo.NombreOportunidad
                                                    select opo).ToList(), "OportunidadID", "NombreOportunidad", DefOportunidadID);

                ViewBag.OportunidadID = oportunidades;
            }
        }

        [HttpPost]
        public ActionResult GetProductsByEmpresaIDForEdit(int empresaID, int itemID)
        {
            try
            {
                var ItemDetail = db.CotizacionDetalle.Where(cd => cd.ItemID == itemID).FirstOrDefault();
                if (ItemDetail != null)
                {
                    return new JsonResult
                    {
                        Data = new {
                            Can = ItemDetail.Cantidad,
                            ValUni = ItemDetail.ValUnitario,
                            ValTot = ItemDetail.ValTotal,
                            List = new SelectList(db.Productos.OrderBy(d => d.NombreProducto).Where(m => m.EmpresaID.Equals(empresaID)).ToList(), "ProductoID", "NombreProducto", 0),
                            Success = true
                        },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

                return new JsonResult
                {
                    Data = new { Message = "El item no se reconoce. Por favor inténtelo de nuevo.", Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult GetProductsByEmpresaID(int empresaID)
        {
            try
            {
                return new JsonResult
                {
                    Data = new { List = new SelectList(db.Productos.OrderBy(d => d.NombreProducto).Where(m => m.EmpresaID.Equals(empresaID)).ToList(), "ProductoID", "NombreProducto", 0), Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult GetProductUnitValue(int prodID)
        {
            try
            {
                var Prod = db.Productos.Where(p => p.ProductoID == prodID).FirstOrDefault();
                if (Prod != null)
                {
                    return new JsonResult { Data = new { Value = Prod.Valor, Success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult { Data = new { Value = 0, Success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult GetClientByEmpresaID(int empresaID)
        {
            try
            {
                return new JsonResult
                {
                    Data = new { List = new SelectList(db.Clientes.OrderBy(d => d.Nombre).Where(m => m.EmpresaID.Equals(empresaID)).ToList(), "ClienteID", "Nombre", 0), Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult GetVendorsByClientID(int clientID)
        {
            try
            {
                var Vendedores = (from Cl in db.Vendedores
                                  where Cl.Clientes.Any(cli => cli.ClienteID == clientID)
                                  select Cl).ToList();

                int DefaultID = -1;

                if (Vendedores != null && Vendedores.Count > 0)
                    DefaultID = Vendedores.First().VendedorID;

                return new JsonResult
                {
                    Data = new { List = new SelectList(Vendedores.OrderBy(d => d.NombreVendedor), "VendedorID", "NombreVendedor", 0), Success = true, DefID = DefaultID },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Delete Object From AjaxJQuery
        /// </summary>
        /// <param name="id">PK Value</param>
        /// <returns></returns>        
        public ActionResult DeleteItem(int id)
        {
            try
            {
                CotizacionDetalle ItemDetail = db.CotizacionDetalle.Find(id);
                if (ItemDetail != null)
                {
                    var Cotizacion = db.Cotizacion.Include(c => c.Items).Where(c => c.CotizacionID == ItemDetail.CotizacionID).FirstOrDefault();
                    if (Cotizacion != null)
                    {   
                        Cotizacion.Valor = Cotizacion.Valor - ItemDetail.ValTotal;
                        db.Entry(Cotizacion).State = EntityState.Modified;

                        db.CotizacionDetalle.Remove(ItemDetail);
                        db.SaveChanges();

                        return new JsonResult
                        {
                            Data = new { Success = true },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }
                
                return new JsonResult
                {
                    Data = new { Message = "Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception eX)
            {
                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult UpdateItemDetail(int itemID, decimal can, decimal valUni)
        {
            try
            {
                var ProductItem = db.CotizacionDetalle.Include(cd => cd.Producto).Where(cd => cd.ItemID == itemID).FirstOrDefault();
                if (ProductItem != null)
                {
                    decimal totalC = 0;

                    ProductItem.Cantidad = can;
                    ProductItem.ValUnitario = valUni;
                    ProductItem.ValTotal = (can * valUni);

                    db.Entry(ProductItem).State = EntityState.Modified;
                    db.SaveChanges();

                    var Cotizacion = db.Cotizacion.Where(c => c.CotizacionID == ProductItem.CotizacionID).FirstOrDefault();
                    if (Cotizacion != null)
                    {
                        foreach (var item in Cotizacion.Items)
                            totalC += item.ValTotal;

                        Cotizacion.Valor = totalC;
                        db.Entry(Cotizacion).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                    string Actions = $"<td><a class=\"fa fa-pencil-square-o\" href=\"#\" title=\"Editar\" onclick=\"EditRecord({ProductItem.ItemID}); return false;\" ></a> | <a class=\"fa fa-trash-o\" href=\"#\" data-backdrop=\"static\" data-href=\"Petición Id=@item.ItemID\" data-pkid=\"@item.ItemID\" data-toggle=\"modal\" data-target=\"#confirm-delete\" title=\"Eliminar\"></a></td>";
                    string MessageVal = $"<tr id=\"(del{ProductItem.ItemID})\"><td>{ProductItem.Producto.ProductoID}</td><td>{ProductItem.Producto.Codigo}</td><td>{ProductItem.Producto.NombreProducto}</td><td>{ProductItem.Producto.Especificaciones}</td><td align=\"right\">{ProductItem.Cantidad.ToString("C2")}</td><td align=\"right\">{ProductItem.ValUnitario.ToString("C2")}</td><td align=\"right\">{ProductItem.ValTotal.ToString("C2")}</td>{Actions}</tr>";

                    return new JsonResult
                    {
                        Data = new { newTotal = totalC, Message = MessageVal, Success = true },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { Message = "Los datos no se reconocen correctamente. Por favor inténtelo de nuevo.", Success = false },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            catch (Exception eX)
            {
                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Save Model
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        public ActionResult SaveProduct(CotizacionViewModel Model)
        {
            DbContextTransaction Transaction = null;

            try
            {
                if (Model != null)
                {
                    if (!Model.IsSaved)
                    {
                        Transaction = db.Database.BeginTransaction();

                        Cotizacion NewModel = new Cotizacion();
                        NewModel.Fecha = Helpers.DateHelper.GetColombiaDateTime();
                        NewModel.ClienteID = Model.CliID;
                        NewModel.EmpresaID = Model.EmpID;
                        NewModel.VendedorID = Model.VenID;

                        if (Model.OpoID > 0)
                            NewModel.OportunidadID = Model.OpoID;

                        var Consecutivo = db.CotizacionConsecutivo.Where(cc => cc.EmpresaID == Model.EmpID).FirstOrDefault();
                        if (Consecutivo == null)
                        {
                            Consecutivo = new CotizacionConsecutivo() { EmpresaID = Model.EmpID, ValorConsecutivo = 0 };
                            db.CotizacionConsecutivo.Add(Consecutivo);
                            db.SaveChanges();
                        }

                        NewModel.NumberID = (Consecutivo.ValorConsecutivo + 1).ToString("0000");
                        NewModel.Valor = (Model.Can * Model.ValUn);
                        NewModel.IsSaved = true;

                        Consecutivo.ValorConsecutivo = Consecutivo.ValorConsecutivo + 1;
                        db.SaveChanges();

                        db.Cotizacion.Add(NewModel);
                        db.SaveChanges();

                        CotizacionDetalle NewItemModel = new CotizacionDetalle()
                        {
                            CotizacionID = NewModel.CotizacionID,
                            Cotizacion = NewModel,
                            ProductoID = Model.ProID,
                            Cantidad = Model.Can,
                            ValUnitario = Model.ValUn,
                            ValTotal = Model.Can * Model.ValUn,
                        };

                        db.CotizacionDetalle.Add(NewItemModel);
                        db.SaveChanges();

                        AddActivityTo(NewModel);

                        Transaction.Commit();

                        return new JsonResult{
                            Data = new { CotID = NewModel.CotizacionID, Success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {

                        var ProductExist = db.CotizacionDetalle.Where(cd => cd.ProductoID == Model.ProID).Any();
                        if (ProductExist)
                        {
                            return new JsonResult
                            {
                                Data = new { Message = "Este producto ya fue agregado al detalle de la cotización. Seleccione otro producto.", Success = false },
                                ContentEncoding = System.Text.Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }

                        var CotizacionSaved = db.Cotizacion.Where(c => c.CotizacionID == Model.CotID).FirstOrDefault();
                        var Producto = db.Productos.Where(p => p.ProductoID == Model.ProID).FirstOrDefault();

                        if (CotizacionSaved != null && Producto != null)
                        {
                            CotizacionDetalle NewItemModel = new CotizacionDetalle()
                            {
                                CotizacionID = Model.CotID,
                                Cotizacion = CotizacionSaved,
                                ProductoID = Model.ProID,
                                Cantidad = Model.Can,
                                ValUnitario = Model.ValUn,
                                ValTotal = Model.Can * Model.ValUn,
                            };

                            CotizacionSaved.Valor = CotizacionSaved.Valor + NewItemModel.ValTotal;
                            db.Entry(CotizacionSaved).State = EntityState.Modified;

                            db.CotizacionDetalle.Add(NewItemModel);
                            db.SaveChanges();

                            string Actions = $"<td><a class=\"fa fa-pencil-square-o\" href=\"#\" title=\"Editar\" onclick=\"EditRecord({NewItemModel.ItemID}); return false;\" ></a> | <a class=\"fa fa-trash-o\" href=\"#\" data-backdrop=\"static\" data-href=\"Petición Id=@item.ItemID\" data-pkid=\"@item.ItemID\" data-toggle=\"modal\" data-target=\"#confirm-delete\" title=\"Eliminar\"></a></td>";
                            string MessageVal = $"<tr id=\"(del{NewItemModel.ItemID})\"><td>{Producto.ProductoID}</td><td>{Producto.Codigo}</td><td>{Producto.NombreProducto}</td><td>{Producto.Especificaciones}</td><td align=\"right\">{Model.Can.ToString("C2")}</td><td align=\"right\">{Model.ValUn.ToString("C2")}</td><td align=\"right\">{NewItemModel.ValTotal.ToString("C2")}</td>{Actions}</tr>";

                            return new JsonResult {
                                Data = new { Message = MessageVal , Success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }
                    }
                }

                return new JsonResult
                {
                    Data = new { Message = "Los datos no se reconocen correctamente. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception eX)
            {
                if (Transaction != null)
                    Transaction.Rollback();

                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}