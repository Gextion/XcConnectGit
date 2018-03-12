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
    /// Importar Producto Data Controller
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class ImportarProductosController : Controller
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

                        CRMContext db = new CRMContext();

                        while ((FileLine = txtfile.ReadLine()) != null)
                        {
                            if (lineas > 0 && !String.IsNullOrEmpty(FileLine))
                            {
                                FileLine = FileLine.Replace("\"", "");
                                ColumnsData = FileLine.Split(new char[] { ';' });

                                if (ColumnsData != null && ColumnsData.Length == 3)
                                {
                                    #region Columnas
                                     //Columnas de Tabla
                                     //   * 
                                     //   * Codigo
                                     //   * Descripcion
                                     //   * 
                                    #endregion

                                    string Codigo = ColumnsData[0].ToString();

                                    var CurrentProducto = db.Productos.Where(p => p.Codigo.Equals(Codigo) && p.EmpresaID.Equals(ApplicationContext.CurrentUser.EmpresaID))
                                                                    .FirstOrDefault();
                                    if (CurrentProducto == null)
                                    {
                                        CurrentProducto = new Producto();
                                        CurrentProducto.Codigo = Codigo;

                                        db.Productos.Add(CurrentProducto);
                                    }
                                    else
                                    {
                                        db.Entry(CurrentProducto).State = EntityState.Modified;
                                    }

                                    CurrentProducto.NombreProducto = ColumnsData[1];
                                    CurrentProducto.Valor = decimal.Parse(ColumnsData[2]);
                                    CurrentProducto.EmpresaID = ApplicationContext.CurrentUser.EmpresaID;

                                    db.SaveChanges();
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
            }
            return View("Index");
        }
    }
}