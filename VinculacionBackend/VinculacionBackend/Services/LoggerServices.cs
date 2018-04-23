using System;
using System.Web;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Services
{
    public class LoggerServices : ILoggerServices
    {
        public void LogError(Exception e)
        {
            string logMessage = e.Message;
            string cadena = HttpContext.Current.Request.Url.AbsoluteUri;
            if (e.InnerException != null)
            {
                logMessage += ", Inner Exception: " + e.InnerException;
            }
            if (e.StackTrace != null)
            {
                logMessage += ", StackTrace: " + e.StackTrace;
            }

            var emailService = new Email();
            emailService.Send("errorsunitec@gmail.com", logMessage, "Error On Server " + cadena);
        }

        public void LogMessage(string message)
        {
            string cadena = HttpContext.Current.Request.Url.AbsoluteUri;
            var emailService = new Email();
            emailService.Send("errorsunitec@gmail.com", message, "Message from Server " + cadena);
        }
    }
}