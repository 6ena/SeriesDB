using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

public class MonitoreoApiStatsDto : EntityDto<int>
{
    public float PromedioTiempoRespuesta { get; set; }
    public int CantidadErrores { get; set; }
    public int CantidadMonitoreos { get; set; }
}