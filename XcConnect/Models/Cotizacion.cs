using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Cotizacion")]
    public class Cotizacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CotizacionID { get; set; }

        [Display(Name = "Número de Cotización")]
        public string NumberID { get; set; }

        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Fecha Cotización")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Index("IX_Vendedor")]
        [Display(Name = "Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }

        [Index("IX_Cliente")]
        [Display(Name = "Cliente")]
        public int ClienteID { get; set; }
        public virtual Cliente Cliente { get; set; }

        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Valor { get; set; }

        [Index("IX_Actividad")]
        [Display(Name = "Oportunidad")]
        public int? OportunidadID { get; set; }
        public virtual Oportunidad Oportunidad { get; set; }

        [Index("IX_Actividad")]
        [Display(Name = "Actividad")]
        public int? ActividadID { get; set; }
        public virtual Actividades Actividades { get; set; }

        [Display(Name = "Detalle Cotización")]
        public virtual ICollection<CotizacionDetalle> Items { get; set; }

        [NotMapped]
        public bool IsSaved { get; set; }

        #region Item Detail Data
        
        [NotMapped]
        public int ItemProductoID { get; set; }

        [NotMapped]
        public virtual Producto ItemProducto { get; set; }

        [NotMapped]
        public decimal ItemCantidad { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ItemValUnitario { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ItemValTotal { get; set; }

        #endregion
    }

    /// <summary>
    /// CotizacionViewModel
    /// </summary>
    public class CotizacionViewModel
    {
        public int CotID { get; set; }
        public bool IsSaved { get; set; }
        public int EmpID { get; set; }
        public int CliID { get; set; }
        public int VenID { get; set; }
        public int OpoID { get; set; }
        public int ProID { get; set; }
        public decimal Can { get; set; }
        public decimal ValUn { get; set; }
        public decimal ValTo { get; set; }
    }

    /// <summary>
    /// Cotizacion Print
    /// </summary>
    public class CotizacionPrint
    {
        public int CotizacionIDPK { get; set; }
        public string Content { get; set; }
        public string CotizacionID { get; set; }
        public string Fecha { get; set; }
        public string Empresa { get; set; }
        public string Cliente { get; set; }
        public int ClienteID { get; set; }
        public string Vendedor { get; set; }
        public string Total { get; set; }
    }
}