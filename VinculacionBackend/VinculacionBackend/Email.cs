using System.Net;
using System.Net.Mail;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend
{
    public class Email : IEmail
    {
        public void Send(string emailAdress, string msg, string subject)
        {
            var fromAddress = new MailAddress("horasunitec@gmail.com", "Sistema Horas Vinculacion UNITEC SPS");
            var encrypter = new Data.Encryption();
            var fromPassword = encrypter.Decrypt("XwHmlRyXybsIXb8pOcTlMg==");
            var toAddress = new MailAddress(emailAdress);

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = msg
            };
            smtp.Send(message);
        }
    }
}