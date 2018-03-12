using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Peticiones")]
    public class Peticion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PeticionID { get; set; }

        [Required(ErrorMessage = "Fecha de Registro")]
        [Display(Name = "Fecha Registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "Título es obligatorio")]
        [StringLength(60)]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Descripción es obligatoria")]
        [DataType(DataType.MultilineText)]
        [StringLength(600)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Tipo de Petición")]
        public TipoPeticion TipoPeticion { get; set; }

        [Display(Name = "Fecha Solución")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaSolucion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Solución")]
        public string Solucion { get; set; }

        [Display(Name = "Resuelta Por")]
        public string ResueltaPor { get; set; }

        //[Index("IX_Usuario")]
        [Display(Name = "Usuario")]
        public string UserID { get; set; }

        [Index("IX_Empresas")]
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

    }

    public enum TipoPeticion
    {
        Petición,
        Queja,
        Reclamo,
        Error,
        Felicitación
    }

}