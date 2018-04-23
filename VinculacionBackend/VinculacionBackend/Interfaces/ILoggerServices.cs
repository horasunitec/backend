using System;

namespace VinculacionBackend.Interfaces
{
    public interface ILoggerServices
    {
        void LogError(Exception e);
        void LogMessage(string message);
    }
}