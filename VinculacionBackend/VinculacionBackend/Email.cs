using System;
using System.Net;
using System.Net.Mail;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend
{
    public class Email:IEmail
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Send(string emailAdress, string msg, string subject)
        {
            try
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

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = msg
                }) ;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
            finally
            {
                logger.Info("Finally");
            }
        }
    }
}