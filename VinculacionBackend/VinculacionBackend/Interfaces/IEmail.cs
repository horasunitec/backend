using RestSharp;

namespace VinculacionBackend.Interfaces
{
    public interface IEmail
    {
        bool Send(string emailAdress, string message, string subject);
    }
}
