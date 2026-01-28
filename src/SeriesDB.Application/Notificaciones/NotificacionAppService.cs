using SeriesDB.Application.Contracts.Notificaciones;
using SeriesDB.Domain.Notificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SeriesDB.Notificaciones
{
    public class NotificacionAppService : INotificacionAppService, ITransientDependency
    {
        
        private readonly INotificacionRepository _notificacionRepository;
        private readonly IEnumerable<INotificador> _notificadores;


        public List<NotificacionDto> MostrarNotificacionesPantalla(Guid idUsuario)
        {
            var notificaciones = _notificacionRepository.GetNotificacionesNoLeidasAsync(idUsuario).Result;

            var notificacionesDto = notificaciones.Select(n => new NotificacionDto
            {
                IdUsuario = n.IdUsuario,
                Titulo = n.Titulo,
                Mensaje = n.Mensaje,
                Leida = n.Leida,
                Tipo = n.Tipo
            }).ToList();

            return notificacionesDto;
        }
    

        public NotificacionAppService(
            INotificacionRepository notificacionRepository,
            IEnumerable<INotificador> notificadores)
        {
            _notificacionRepository = notificacionRepository;
            _notificadores = notificadores;
        }

        
        public async Task CrearYEnviarNotificacionAsync(Guid idUsuario, string titulo, string mensaje, TipoNotificacion tipo)
        {
            // Crear un DTO para la notificación
            var notificacionDto = new NotificacionDto
            {
                IdUsuario = idUsuario,
                Titulo = titulo,
                Mensaje = mensaje,
                Leida = false,
                Tipo = tipo
            };

            // Insertar la notificación en el repositorio
            var notificacion = new Notificacion
            {
                IdUsuario = idUsuario,
                Titulo = titulo,
                Mensaje = mensaje,
                Leida = false,
                Tipo = tipo
            };

            await _notificacionRepository.InsertAsync(notificacion);

            // Enviar la notificación utilizando los notificador(es) registrados
            var notificadoresFiltrados = _notificadores.Where(n => n.PuedeEnviar(tipo));
            foreach (var notificador in notificadoresFiltrados)
            {
                await notificador.EnviarNotificacionAsync(notificacionDto); // Usar el DTO aquí
            }
        }
    }
}