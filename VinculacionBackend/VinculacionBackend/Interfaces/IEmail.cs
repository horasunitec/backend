using RestSharp;

namespace VinculacionBackend.Interfaces
{
    public interface IEmail
    {
        void Send(string emailAdress, string message, string subject);
    }
}
