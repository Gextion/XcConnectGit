using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    [Table("CotizacionConsecutivo")]
    public class CotizacionConsecutivo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsecutivoID { get; set; }

        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        public int ValorConsecutivo { get; internal set; }
    }
}