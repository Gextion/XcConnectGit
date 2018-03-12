using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Empresas")]
    public class Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpresaID { get; set; }

        [Required(ErrorMessage = "Código de la Empresa es obligatoria")]
        [Index(IsUnique = true)]
        [RegularExpression("[0-9]{3}", ErrorMessage = "El Código de la Empresa debe tener tres números")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Nombre de la Empresa es obligatoria")]
        [MaxLength(80, ErrorMessage = "El Nombre de la Empresa no puede tener más de 80 caracteres"),
         MinLength(3, ErrorMessage = "El Nombre de la Empresa no puede tener menos de 3 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Razon Social es obligatoria")]
        [MaxLength(80, ErrorMessage = "La Razon Social no puede tener más de 80 caracteres"),
         MinLength(3, ErrorMessage = "La Razon Social no puede tener menos de 3 caracteres")]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "Nit es obligatorio")]
        [Index(IsUnique = true)]
        [Range(999999, 9999999999)]
        public long Nit { get; set; }

        [Index("IX_Ciudades")]
        [Display(Name = "Ciudad")]
        public int CiudadID { get; set; }
        public virtual Ciudades Ciudad { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Dirección es obligatoria")]
        [MaxLength(120, ErrorMessage = "La Dirección no puede tener más de 120 caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Teléfono es obligatorio")]
        [Display(Name = "Teléfono")]
        public long Telefono { get; set; }

        [Required(ErrorMessage = "Celular es obligatorio")]
        public long Celular { get; set; }

        [MaxLength(100, ErrorMessage = "El Email no puede tener más de 100 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Dirección de correo no valida")]
        public string Email { get; set; }

        [MaxLength(300, ErrorMessage = "El Sitio Web no puede tener más de 300 caracteres")]
        [Display(Name = "Sitio Web")]
        public string SitioWeb { get; set; }

        [Display(Name = "Logo")]
        public string LogoUrl { get; set; }

        [Display(Name = "Linea Base")]
        [Required(ErrorMessage = "Debe especificar la Línea Base")]
        [Range(0, 9999, ErrorMessage = "La Linea Base debe estar entre 0 y 9999")]
        public int LineaBase { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<Actividades> Actividades { get; set; }
        public virtual ICollection<Peticion> Peticiones { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
      //public virtual ICollection<ApplicationUser> Usuarios { get; set; }
    }
}

