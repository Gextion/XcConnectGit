using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models
{
    public class ClientesEmpresa
    {
        public int EmpresaID { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public long Nit { get; set; }

        public string Ciudad { get; set; }

        public int LineaBase { get; set; }

        public int Clientes { get; set; }
    }
}

