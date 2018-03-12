using XcConnect.Models.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace XcConnect.Models
{
    public class Dashboard
    {
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Tipo Actividad")]
        public int TipoActividadID { get; set; }
        public virtual TipoActividad TipoActividad { get; set; }

        //[Display(Name = "Actividad")]
        //public int ActividadID { get; set; }
        //public virtual Actividades Actividades { get; set; }

        [Display(Name = "Usuario")]
        public int UserID { get; set; }
        public virtual UserManager<ApplicationUser> Usuario { get; set; }

        [Display(Name = "Año")]
        [Column("AñoID")]
        public int AnioID { get; set; }
        public virtual Años Años { get; set; }

        [Display(Name = "Mes")]
        public int MesID { get; set; }
        public virtual Meses Meses { get; set; }

        public DatosActividades DatosActividades { get; set; }
        public DatosOportunidades DatosOportunidades { get; set; }
       
        public ICollection<Actividades> Actividades { get; set; }
    }
}