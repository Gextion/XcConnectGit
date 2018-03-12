using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("TiposActividades")]
    public class TipoActividad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipoActividadID { get; set; }

        [Display(Name = "Tipo de Actividad")]
        public string NombreTipoActividad { get; set; }

        public string Icono { get; set; }

        public ICollection<Actividades> Actividades { get; set; }

    }
}