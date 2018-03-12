using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("Años")]
    public class Años
    {
        [Key]
        [Column("AñoID")]
        public int AnioID { get; set; }

        public int Año { get; set; }
    }
}
