using System;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Services
{
    public class LoggerServices : ILoggerServices
    {
        public void LogError(Exception e)
        {
            string logMessage = e.Message;
            if (e.InnerException != null)
            {
                logMessage += ", Inner Exception: " + e.InnerException;
            }
            if (e.StackTrace != null)
            {
                logMessage += ", StackTrace: " + e.StackTrace;
            }

            var emailService = new Email();
            emailService.Send("errorsunitec@gmail.com", logMessage, "Error On Server");
        }

        public void LogMessage(string message)
        {
            var emailService = new Email();
            emailService.Send("errorsunitec@gmail.com", message, "Message from Server");
        }
    }
}