using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XcConnect.Models
{
    [Table("SectoresEconomicos")]
    public class SectoresEconomicos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectorEconomicoID { get; set; }

        [Display(Name = "Sector Económico")]
        public string SectorEconomico { get; set; }

        public virtual ICollection<Cliente> Empresas { get; set; }
    }
}
