using Serilog;
using System;
using System.Net;
using System.Net.Mail;

namespace VinculacionBackend.Services
{
    public class Logger : Interfaces.ILogger
    {
        public void LogError(Exception e)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("log.txt",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
            .CreateLogger();
            var msg = e.Message;
            if (e.InnerException != null)
            {
                msg += '\n';
                msg += e.InnerException;
            }
            Log.Information(msg);
            Log.CloseAndFlush();

            SendEmail(e);
        }

        void SendEmail(Exception e)
        {
            var fromAddress = new MailAddress("horasunitec@gmail.com", "Sistema Horas Vinculacion UNITEC SPS");
            var encrypter = new Data.Encryption();
            var fromPassword = encrypter.Decrypt("XwHmlRyXybsIXb8pOcTlMg==");
            var toAddress = new MailAddress("horasunitec@gmail.com");

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var msg = e.Message;
            if (e.InnerException != null)
            {
                msg += '\n';
                msg += e.InnerException;
            }
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "ERROR" + DateTime.Today.ToString(),
                Body = msg
            };
            smtp.Send(message);
        }
    }
}