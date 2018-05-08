using System.Net;
using System.Net.Mail;

namespace VinculacionBackend.Console
{
    public class Email 
    {
        public void Send(string emailAdress, string msg, string subject)
        {
            var fromAddress = new MailAddress("horasunitec@gmail.com");
            var fromPassword = "goldeneye007";
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
