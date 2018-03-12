using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("ActividadesArchivos")]
    public class ActividadesArchivos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActividadArchivoID { get; set; }

        [Display(Name = "Archivo")]
        public string ArchivoUrl { get; set; }

        [Display(Name = "Archivo")]
        public string LocalUrl { get; set; }

        [Display(Name = "Archivo")]
        [StringLength(250)]
        public string ArchivoNombre { get; set; }

        [Index("IX_Actividad")]
        [Display(Name = "Actividad")]
        public int ActividadID { get; set; }
        public virtual Actividades Actividad { get; set; }
    }
}