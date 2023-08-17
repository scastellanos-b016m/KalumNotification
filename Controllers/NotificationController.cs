using KalumNotification.DTOs;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace KalumNotification.Controllers
{
    [Route("kalum-notification/v1/notification")]
    [ApiController]
    public class NotificationController: ControllerBase
    {
     [HttpPost]
     public ActionResult<NotificationResponseDTO> SendEmail([FromBody] NotificationRequestDTO notification)
     {
        NotificationResponseDTO response = null;
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Kalum Notification", "sergioivan1191@gmail.com"));
            email.To.Add(new MailboxAddress(notification.Recipient, notification.Email));
            if(string.Equals(notification.Type, "expediente", StringComparison.OrdinalIgnoreCase)) 
            {
                email.Subject = $"Generación de número de expediente {notification.IdentificationId}";
            }
            else if(string.Equals(notification.Type, "carne", StringComparison.OrdinalIgnoreCase)) 
            {
                email.Subject = $"Generación de número de carné {notification.IdentificationId}";
            }
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) 
            {
                Text = $"<h2><b>Su número de {(string.Equals(notification.Type, "expediente", StringComparison.OrdinalIgnoreCase) ? "Expediente" : "Carné")} fue creado con éxito, haz click en el siguiente link para completar el proceso</b></h2><hr><br>" + notification.Body
            };
            using (var smtp = new SmtpClient()) 
            {
                smtp.Connect("smtp.gmail.com", 465, true);
                smtp.Authenticate("sergioivan1191@gmail.com", "pawkvvmrpejppqqv");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            response = new NotificationResponseDTO()
            {
                HttpStatusCode = 201,
                Message = $"Correo electrónico enviado con éxito a {notification.Email}"
            };
            return StatusCode(201, response);
        }
        catch(Exception e) 
        {
            response = new NotificationResponseDTO()
            {
                HttpStatusCode = 503,
                Message = e.Message
            };
            return StatusCode(503, response);
        }
     }
    }
}