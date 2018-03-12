using System.ComponentModel.DataAnnotations;

namespace XcConnect.Models.Reports
{
    /// <summary>
    /// Report Empresas
    /// </summary>
    public class RptClientes
    {
        [Display(Name = "Ciudad")]
        public int CiudadID { get; set; }
        public virtual Ciudades Ciudad { get; set; }
    }
}