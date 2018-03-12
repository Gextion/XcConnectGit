using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("OportunidadesArchivos")]
    public class OportunidadesArchivos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OportunidadArchivoID { get; set; }

        [Display(Name = "Archivo")]
        public string ArchivoUrl { get; set; }

        [Display(Name = "Archivo")]
        public string LocalUrl { get; set; }

        [Display(Name = "Archivo")]
        [StringLength(250)]
        public string ArchivoNombre { get; set; }

        [Index("IX_Oportunidad")]
        [Display(Name = "Oportunidad")]
        public int OportunidadID { get; set; }
        public virtual Oportunidad Oportunidad { get; set; }
    }
}