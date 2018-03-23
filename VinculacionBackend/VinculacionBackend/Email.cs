using System;
using RestSharp;
using RestSharp.Authenticators;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend
{
    public class Email:IEmail
    {    
        public  IRestResponse Send(string emailAdress, string message, string subject)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new System.Uri("https://api.mailgun.net/v3");
            client.Authenticator =
            new HttpBasicAuthenticator("api",
                                      "key-90c09be8abf5f2eca6588d944727a22b");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandbox792e3910e58e449d870d909b6e71e032.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            //request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox792e3910e58e449d870d909b6e71e032.mailgun.org>");
            request.AddParameter("from", "Sistema Horas Vinculacion UNITEC <horasunitec@gmail.com>");
            request.AddParameter("to", emailAdress);
            request.AddParameter("subject", subject);
            request.AddParameter("text", message);
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}