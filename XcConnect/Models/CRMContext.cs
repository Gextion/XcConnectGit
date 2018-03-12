using System.Data.Entity;

namespace XcConnect.Models
{
    /// <summary>
    /// DataBase Context
    /// </summary>
    public class CRMContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public CRMContext() : base("name=CRMContext")
        {
        }

        public DbSet<Ciudades> Ciudades { get; set; }

        public DbSet<SectoresEconomicos> SectoresEconomicos { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Contactos> Contactos { get; set; }

        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<Vendedor> Vendedores { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Actividades> Actividades { get; set; }

        public DbSet<ActividadesArchivos> ActividadesArchivos { get; set; }

        public DbSet<Oportunidad> Oportunidad { get; set; }

        public DbSet<OportunidadesArchivos> OportunidadArchivos { get; set; }

        public DbSet<Peticion> Peticions { get; set; }

        public DbSet<TipoActividad> TipoActividad { get; set; }

        public DbSet<Años> Años { get; set; }

        public DbSet<Meses> Meses { get; set; }


        public DbSet<Cotizacion> Cotizacion { get; set; }

        public DbSet<CotizacionDetalle> CotizacionDetalle { get; set; }

        public DbSet<CotizacionConsecutivo> CotizacionConsecutivo { get; set; }
    }
}
