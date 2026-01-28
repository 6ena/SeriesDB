using Microsoft.Extensions.Logging;
using SeriesDB.Application.Contracts.Notificaciones;
using SeriesDB.Domain.Notificaciones;
using SeriesDB.Notificaciones;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SeriesDB.Application.Notificaciones
{
    public class NotificadorEmail : INotificador
    {
        public bool PuedeEnviar(TipoNotificacion tipo)
        {
            return tipo == TipoNotificacion.Email;
        }

        /// Envía una notificación por correo electrónico.
        /// </summary>
        /// <param name="notificacionDto">El DTO de la notificación a enviar.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        /// <exception cref="Exception">Se lanza si hay un error al enviar el correo electrónico.</exception>
        public async Task EnviarNotificacionAsync(NotificacionDto notificacionDto)
        {
            var notificacion = new Notificacion
            {
                IdUsuario = notificacionDto.IdUsuario,
                Titulo = notificacionDto.Titulo,
                Mensaje = notificacionDto.Mensaje,
                Leida = false,
                Tipo = notificacionDto.Tipo,
                FechaCreacion = notificacionDto.FechaCreacion
            };

            
            var fromAddress = new MailAddress("tu-email@example.com", "Tu Nombre");
            var toAddress = new MailAddress("usuario@example.com", "Usuario");
            const string fromPassword = "tu-contraseña";
            string subject = notificacion.Titulo;
            string body = notificacion.Mensaje;

            var smtp = new SmtpClient
            {
                Host = "smtp.example.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
            
        }
    }
}