using System;
using System.Net;
using System.Net.Mail;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend
{
    public class Email:IEmail
    {
        public void Send(string emailAdress, string msg, string subject)
        {
            var fromAddress = new MailAddress("horasunitec@gmail.com");
            var fromPassword = "goldeneye007";
            var toAddress = new MailAddress(emailAdress);

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
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