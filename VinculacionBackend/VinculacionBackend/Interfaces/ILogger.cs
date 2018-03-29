using System;

namespace VinculacionBackend.Interfaces
{
    public interface ILogger
    {
        void LogError(Exception e);
    }
}
