using XcConnect.Models;
using XcConnect.Helpers.Extensions;
using System;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using XcConnect.Helpers;
using System.Data.Entity;

namespace XcConnect.Controllers
{
    /// <summary>
    /// Import Client Data Controller
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class ImportarClientesController : Controller
    {
        // GET: FileUpload    
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult UploadFiles(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //try
                //{
                //Method 1 Get file details from current request    
                //if (Request.Files.Count > 0)
                //{
                //    var Inputfile = Request.Files[0];

                //    if (Inputfile != null && Inputfile.ContentLength > 0)
                //    {
                //        var filename = Path.GetFileName(Inputfile.FileName);
                //        var path = Path.Combine(Server.MapPath("~/App_Data/ClientsToImport"), filename);
                //        Inputfile.SaveAs(path);
                //    }
                //}

                //Method 2
                //string path = Path.Combine(Server.MapPath("~/App_Data/ClientsToImport"), Path.GetFileName(file.FileName));
                //file.SaveAs(path);

                if (file != null)
                {
                    int Counter = 0;
                    int CounterFail = 0;
                    string FileLine = string.Empty;
                    string[] ColumnsData = null;

                    StreamReader txtfile = new StreamReader(file.InputStream);
                    if (txtfile != null)
                    {
                        int lineas = 0;
                        bool grabar;

                        CRMContext db = new CRMContext();

                        while ((FileLine = txtfile.ReadLine()) != null)
                        {
                            if (lineas > 0 && !String.IsNullOrEmpty(FileLine))
                            {

                                FileLine = FileLine.Replace("\"", "");
                                ColumnsData = FileLine.Split(new char[] { ';' });
                                if (ColumnsData != null && ColumnsData.Length == 14)
                                {
                                    #region Columnas
                                    /* Columnas de Tabla
                                        * 
                                        * [1er Nombre]             0
                                        * [2o Nombre]        1
                                        * [Nit]                2
                                        * [RepresentanteLegal] 3
                                        * [EmpresaID]          4
                                        * [SectorEconomicoID]  5
                                        * [CiudadID]           6
                                        * [Direccion]          7
                                        * [Telefono]           8
                                        * [Email]              9
                                        * [SitioWeb]           10
                                        * [AtendidoPor]        11
                                        * 
                                        * */
                                    #endregion

                                    grabar = true;

                                    long NIT = ColumnsData[4].ToLong(long.Parse(ConfigurationManager.AppSettings["DefaultNitValue"]));

                                    if (NIT == long.Parse(ConfigurationManager.AppSettings["DefaultNitValue"]))
                                    {
                                        CounterFail++;
                                    }
                                    else
                                    {
                                        var CurrentClient = db.Clientes.Where(c => c.Nit.Equals(NIT) && c.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                                        .FirstOrDefault();
                                        if (CurrentClient == null)
                                        {
                                            CurrentClient = new Cliente();
                                            CurrentClient.Nit = NIT;

                                            db.Clientes.Add(CurrentClient);
                                        }
                                        else
                                        {
                                            db.Entry(CurrentClient).State = EntityState.Modified;
                                        }

                                        CurrentClient.Nombre = ColumnsData[0] + " " + ColumnsData[1] + " " + ColumnsData[2] + " " + ColumnsData[3];
                                        CurrentClient.Nombre = CurrentClient.Nombre.Trim();
                                        if (ColumnsData[12] != "Natural")
                                        {
                                            CurrentClient.RazonSocial = CurrentClient.Nombre;
                                        }
                                        CurrentClient.EmpresaID = ApplicationContext.CurrentUser.EmpresaID;

                                        if (!string.IsNullOrEmpty(ColumnsData[11]))
                                        {
                                            string NombreSector = ColumnsData[11];
                                            var sector = db.SectoresEconomicos.Where(s => s.SectorEconomico.Equals(NombreSector)).FirstOrDefault();
                                            if (sector == null)
                                                CurrentClient.SectorEconomicoID = int.Parse(ConfigurationManager.AppSettings["DefaultSectorEconomicoIDValue"]);
                                            else
                                                CurrentClient.SectorEconomicoID = sector.SectorEconomicoID;
                                        }
                                        else
                                        {
                                            CurrentClient.SectorEconomicoID = int.Parse(ConfigurationManager.AppSettings["DefaultSectorEconomicoIDValue"]);
                                        }

                                        if (!string.IsNullOrEmpty(ColumnsData[5]))
                                        {
                                            string NombreCiudad = ColumnsData[5];
                                            var ciudad = db.Ciudades.Where(c => c.Ciudad.Equals(NombreCiudad)).FirstOrDefault();
                                            if (ciudad == null)
                                                CurrentClient.CiudadID = int.Parse(ConfigurationManager.AppSettings["DefaultCiudadIDValue"]);
                                            else
                                                CurrentClient.CiudadID = ciudad.CiudadID;
                                        }
                                        else
                                        {
                                            CurrentClient.CiudadID = int.Parse(ConfigurationManager.AppSettings["DefaultCiudadIDValue"]);
                                        }

                                        CurrentClient.Direccion = ColumnsData[6];
                                        CurrentClient.Telefono = ColumnsData[7].ToLong(long.Parse(ConfigurationManager.AppSettings["DefaultTelefonoValue"]));

                                        if (!string.IsNullOrEmpty(ColumnsData[9]))
                                        {
                                            CurrentClient.Email = ColumnsData[9];
                                        }
                                        else
                                        {
                                            CurrentClient.Email = "sincorreo@xyz.org";
                                        }

                                        CurrentClient.SitioWeb = ColumnsData[10];

                                        if (!string.IsNullOrEmpty(ColumnsData[13]))
                                        {
                                            string CodigoVendedor = ColumnsData[13];
                                            var vendedor = db.Vendedores.Where(v => v.Codigo.Equals(CodigoVendedor)).FirstOrDefault();
                                            if (vendedor == null)
                                                CurrentClient.VendedorID = db.Clientes.FirstOrDefault().VendedorID;
                                            else
                                                CurrentClient.VendedorID = vendedor.VendedorID;
                                        }
                                        else
                                        {
                                            CounterFail++;
                                            grabar = false;
                                        }

                                        if (grabar)
                                        {
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            db.Clientes.Remove(CurrentClient);
                                        }
                                    }
                                    Counter++;
                                }
                                else
                                {
                                    CounterFail++;
                                }
                            }
                            lineas++;
                        }

                        ViewBag.FileStatus = $"Archivo procesado. Se importaron {Counter} registro(s) correctamente y {CounterFail} registro(s) con error.";
                    }
                    else
                    {
                        ViewBag.FileStatus = Constants.FileManager.STR_NOT_PROCESS;
                    }
                }
                else
                {
                    ViewBag.FileStatus = Constants.FileManager.STR_NOT_FILE;
                }
                //}
                //catch (Exception eX)
                //{
                //    ViewBag.FileStatus = String.Format(Constants.FileManager.STR_FILEUPDALOAD_EX, eX.Message);
                //}

            }
            return View("Index");
        }
    }
}