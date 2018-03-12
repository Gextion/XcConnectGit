using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Oportunidades")]
    public class Oportunidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OportunidadID { get; set; }

        [Required(ErrorMessage = "Oportunidad es obligatoria")]
        [StringLength(80)]
        //[MaxLength(80, ErrorMessage = "La Oportunidad no puede tener más de 80 caracteres")]
        [Display(Name = "Nombre Oportunidad")]
        public string NombreOportunidad { get; set; }

        [Required(ErrorMessage = "Descripción es obligatoria")]
        [StringLength(400)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public EstadosOportunidad Estado { get; set; }

        [Index("IX_Clientes")]
        [Display(Name = "Cliente")]
        public int ClienteID { get; set; }
        public virtual Cliente Clientes { get; set; }

        [Required(ErrorMessage = "Solicitado Por es obligatorio")]
        [StringLength(60)]
        //[MaxLength(60, ErrorMessage = "Solicitado Por no puede tener más de 60 caracteres")]
        [Display(Name = "Solicitada Por")]
        public string SolicitadaPor { get; set; }

        [Index("IX_Vendedor")]
        [Display(Name = "Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }

        [Required(ErrorMessage = "Fecha de Solicitud es obligatoria")]
        [Display(Name = "Fecha Solicitud")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaSolicitud { get; set; }

        [Display(Name = "Fecha Entrega")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaEntrega { get; set; }

        [Display(Name = "Fecha Cierre")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaCierre { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Range(0,999999999, ErrorMessage = "El valor debe estar entre 0 y 999.999.999")]
        public int Valor { get; set; }

        [Display(Name = "Usuario")]
        public string UserID { get; set; }

        [Display(Name = "Archivos Asociados")]
        public virtual ICollection<OportunidadesArchivos> Archivos { get; set; }
    }

    public enum EstadosOportunidad
    {
        Calificada,
        Preparación,
        Presentada,
        Negociación,
        Ganada,
        Perdida,
        Suspendida,
        Cancelada
    }

}

