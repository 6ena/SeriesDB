using SeriesDB.Notificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace SeriesDB.Domain.Notificaciones
{
    public class Notificacion : FullAuditedEntity<int>
    {
        public Guid IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public bool Leida { get; set; }
        public TipoNotificacion Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }

    }

}