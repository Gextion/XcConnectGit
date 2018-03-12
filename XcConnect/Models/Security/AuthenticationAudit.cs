using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models.Security
{
    [Table("AuthenticationAudit")]
    public class AuthenticationAudit
    {
        /// <summary>
        /// Audit Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuditID { get; set; }

        [Display(Name = "Usuario")]
        [StringLength(128)]
        public string UserId { get; set; }
        
        [Display(Name = "Dirección IP")]
        [StringLength(50)]
        public string LoginIP { get; set; }

        [Display(Name = "Navegador")]
        [StringLength(150)]
        public string LoginBrowser { get; set; }

        [Display(Name = "Versión de Navegador")]
        [StringLength(150)]
        public string LoginBrowserVersion { get; set; }

        [Display(Name = "Plataforma")]
        [StringLength(150)]
        public string LoginPlatform { get; set; }

        [Display(Name = "Fecha")]
        public DateTime LoginDate { get; set; }

        /// <summary>
        /// Application User Instance
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }
}