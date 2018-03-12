using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XcConnect.Models
{
    public class DatosActividades
    {
        public string TipoActividad { get; set; }
        public int Ano { get; set; }
        public int MesID { get; set; }
        public string NombreMes { get; set; }
        public string RGBA_Background { get; set; }
        public string RGBA_Border { get; set; }
        public int Cantidad { get; set; }
    }

    public class DatosOportunidades
    {
        public string EstadoOportunidad { get; set; }
        public string RGBA_Background { get; set; }
        public int Cantidad { get; set; }
    }

    //public class IndicadoresFuenteEnergetica
    //{
    //    public string ConsumoMesS { get { return ConsumoMes.ToString("N2"); } }
    //    public string ConsumoAnualS { get { return ConsumoAnual.ToString("N2"); } }
    //    public string LineaBaseMesS { get { return LineaBaseMes.ToString("N2"); } }
    //    public string LineaBaseAnualS { get { return LineaBaseAnual.ToString("N2"); } }
    //    public string ValorConsumoMesS { get { return ValorConsumoMes.ToString("C2"); } }
    //    public string ValorConsumoAnualS { get { return ValorConsumoAnual.ToString("C2"); } }
    //    public string AhorroMesS { get { return AhorroMes.ToString("P2"); } }
    //    public string AhorroAnualS { get { return AhorroAnual.ToString("N2"); } }

    //    public decimal ConsumoMes { get; set; }
    //    public decimal ConsumoAnual { get; set; }
    //    public decimal LineaBaseMes { get; set; }
    //    public decimal LineaBaseAnual { get; set; }
    //    public decimal ValorConsumoMes { get; set; }
    //    public decimal ValorConsumoAnual { get; set; }
    //    public double AhorroMes { get; set; }
    //    public decimal AhorroAnual { get; set; }

    //}
}