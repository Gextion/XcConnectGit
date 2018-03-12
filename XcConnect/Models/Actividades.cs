using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Actividades")]
    public class Actividades
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActividadID { get; set; }

        [Required(ErrorMessage = "Fecha de Registro")]
        [Display(Name = "Fecha Registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "Descripción es obligatoria")]
        [StringLength(80)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Tipo de Actividad")]
        public int TipoActividadID { get; set; }
        public virtual TipoActividad TipoActividad { get; set; }


        [Index("IX_Empresas")]
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        [Required(ErrorMessage = "La Fecha de Ejecución es obligatoria")]
        [Display(Name = "Fecha Ejecución")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaEntrega { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notas { get; set; }

        [Display(Name = "Usuario")]
        public string UserID { get; set; }

        [Display(Name = "Archivos Asociados")]
        public virtual ICollection<ActividadesArchivos> Archivos { get; set; }

    }

}