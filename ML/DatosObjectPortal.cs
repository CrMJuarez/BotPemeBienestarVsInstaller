using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
   public class DatosObjectPortal
    {
        public string IdFolioDeServicio { get; set; }
        public string Prioridad { get; set; }
        public string TipoServicio { get; set; }
        public string SucursalConsignatario { get; set; }
        public string FechaCaptura { get; set; }
        public string FechaRealizarServicio { get; set; }
        public string OrdenServicio { get; set; }
        public decimal? Importe { get; set; }
        public string Divisa { get; set; }
        public string Te { get; set; }
        public string HoraEnvio { get; set; }
        public string Actualización { get; set; }
        public string Estatus { get; set; }
    }
}
