using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XcConnect.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "Nombre del Cliente es obligatorio")]
        [MaxLength(80, ErrorMessage = "El Nombre del Cliente no puede tener más de 80 caracteres"),
         MinLength(3, ErrorMessage = "El Nombre del Cliente no puede tener menos de 3 caracteres")]
        public string Nombre { get; set; }

        //[Range(999, 9999999999)]
        [Display(Name = "Identificación")]
        public long Nit { get; set; }

        //[Required(ErrorMessage = "Razon Social es obligatorio")]
        [MaxLength(80, ErrorMessage = "La Razon Social no puede tener más de 80 caracteres")]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        //[Required(ErrorMessage = "Representante Legal es obligatorio")]
        [MaxLength(80, ErrorMessage = "El Representante Legal no puede tener más de 80 caracteres")]
        [Display(Name = "Representante Legal")]
        public string RepresentanteLegal { get; set; }

        [Index("IX_SectoresEconomicos")]
        [Display(Name = "Sector Económico")]
        public int SectorEconomicoID { get; set; }
        public virtual SectoresEconomicos SectorEconomico { get; set; }

        [Index("IX_Ciudades")]
        [Display(Name = "Ciudad")]
        public int CiudadID { get; set; }
        public virtual Ciudades Ciudad { get; set; }

        [Display(Name = "Dirección")]
        //[Required(ErrorMessage = "Dirección es obligatoria")]
        [MaxLength(120, ErrorMessage = "La Dirección no puede tener más de 120 caracteres")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        public long? Telefono { get; set; }

        public long? Celular { get; set; }

        [MaxLength(100, ErrorMessage = "El Email no puede tener más de 100 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Dirección de correo no valida")]
        public string Email { get; set; }

        [MaxLength(300, ErrorMessage = "El Sitio Web no puede tener más de 300 caracteres")]
        [Display(Name = "Sitio Web")]
        //[DataType(DataType.Url)]
        public string SitioWeb { get; set; }

        [Index("IX_Vendedor")]
        [Display(Name = "Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }

        [Index("IX_Empresas")]
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        public ICollection<Contactos> Contactos { get; set; }
        public ICollection<Oportunidad> Oportunidades { get; set; }
    }
}

