namespace VinculacionBackend.Console
{
    using System.Net;
    using Quartz;

    public class KeepAliveJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadString("http://backend-4.apphb.com");
            }
            var email = new Email();
            email.Send("horasunitec@gmail.com", "keep alive request", "keep alive request");
        }
    }
}
