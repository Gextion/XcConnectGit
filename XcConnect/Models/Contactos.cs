using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Contactos")]
    public class Contactos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactoID { get; set; }

        [Index("IX_Clientes")]
        [Display(Name = "Cliente")]
        public int ClienteID { get; set; }
        public virtual Cliente Clientes { get; set; }

        [Required(ErrorMessage = "Nombre del Contacto es obligatorio")]
        [MaxLength(60, ErrorMessage = "El Nombre del Contacto no puede tener más de 60 caracteres"),
         MinLength(3, ErrorMessage = "El Nombre del Contacto no puede tener menos de 3 caracteres")]
        [Display(Name = "Nombre Contacto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Cargo del Contacto es obligatorio")]
        [MaxLength(50, ErrorMessage = "El Cargo del Contacto no puede tener más de 50 caracteres")]
        public string Cargo { get; set; }

        [Display(Name = "Teléfono")]
        public long Telefono { get; set; }

        public long Celular { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Dirección de correo no valida")]
        public string Email { get; set; }
    }
}

