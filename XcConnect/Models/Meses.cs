using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Meses")]
    public class Meses
    {
        [Key]
        public int MesID { get; set; }

        public string Mes { get; set; }

        [Column("RGBA_Background")]
        public string rbgaBackground { get; set; }

        [Column("RGBA_Border")]
        public string rbgaBorder { get; set; }

    }
}