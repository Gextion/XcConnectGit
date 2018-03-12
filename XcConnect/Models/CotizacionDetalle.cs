using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("CotizacionDetalle")]
    public class CotizacionDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Index("IX_Producto")]
        [Display(Name = "Producto")]
        public int ProductoID { get; set; }
        public virtual Producto Producto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El Valor unitario es obligatorio")]
        [Display(Name = "Valor Unitario")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ValUnitario { get; set; }

        [Display(Name = "Valor Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ValTotal { get; set; }

        [Index("IX_CotizacionID")]
        public int CotizacionID { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }
    }
}