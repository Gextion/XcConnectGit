using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Vendedores")]
    public class Vendedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendedorID { get; set; }

        [Required(ErrorMessage = "Código Vendedor es obligatorio")]
        [MaxLength(4)]
        [MinLength(2)]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Nombre Vendedor es obligatorio")]
        [MaxLength(60, ErrorMessage = "Nombre Vendedor no puede tener más de 60 caracteres"),
         MinLength(6, ErrorMessage  = "Nombre Vendedor no puede tener menos de 6 caracteres")]
        [Display(Name = "Nombre Vendedor")]
        public string NombreVendedor { get; set; }

        [Required(ErrorMessage = "Celular es obligatorio")]
        public long Celular { get; set; }

        [MaxLength(100, ErrorMessage = "El Email no puede tener más de 100 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Dirección de correo no valida")]
        public string Email { get; set; }

        [Index("IX_Empresas")]
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        public ICollection<Cliente> Clientes { get; set; }
        public ICollection<Oportunidad> Oportunidades { get; set; }
        //public ICollection<Cotizacion> Cotizaciones { get; set; }
    }
}

