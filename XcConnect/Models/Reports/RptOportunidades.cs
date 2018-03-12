using System.ComponentModel.DataAnnotations;

namespace XcConnect.Models.Reports
{
    public class RptOportunidades
    {
        [Display(Name = "Cliente")]
        public int ClienteID { get; set; }
        public virtual Cliente Cliente { get; set; }

        [Display(Name = "Estado")]
        public EstadosOportunidad Estado { get; set; }

        [Display(Name = "Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }

        [Display(Name = "Fecha Inicial")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaInicial { get; set; }

        [Display(Name = "Fecha Final")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaFinal { get; set; }
    }
}