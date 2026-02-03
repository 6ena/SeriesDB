using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace SeriesDB.Series
{
    public class MonitoreoApi : Entity<int>
    {
        public DateTime HoraAcceso { get; set; }
        public DateTime HoraFin { get; set; }
        public float TiempoRespuesta { get; set; }
        public List<string> Errores { get; set; } = new List<string>();
    }
}