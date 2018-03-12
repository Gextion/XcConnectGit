using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductoID { get; set; }

        [Required(ErrorMessage = "Código Producto es obligatorio")]
        [MaxLength(15)]
        [MinLength(2)]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Descripción del Producto es obligatorio")]
        [MaxLength(120, ErrorMessage = "Descripción del Producto no puede tener más de 120 caracteres"),
         MinLength(5, ErrorMessage = "Descripción del Producto no puede tener menos de 5 caracteres")]
        [Display(Name = "Descripción")]
        public string NombreProducto { get; set; }

        [DataType(DataType.MultilineText)]
        public string Especificaciones { get; set; }

        [Required(ErrorMessage = "Valor del producto es obligatorio")]
        [Range(0,999999999, ErrorMessage = "El Valor del producto debe estar entre 0 y 999.999.999")]
        public decimal Valor { get; set; }

        [Index("IX_Empresas")]
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

    }
}

