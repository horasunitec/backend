namespace VinculacionBackend.Console
{
    using Quartz;
    using Quartz.Impl;
    public class Program
    {
        public static void Main(string[] args)
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();
            var keepAliveJob = JobBuilder.Create<KeepAliveJob>().Build();
            var keepAliveTrigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInMinutes(2).RepeatForever())
                            .Build();

            scheduler.ScheduleJob(keepAliveJob, keepAliveTrigger);
            scheduler.Start();
            var email = new Email();
            email.Send("horasunitec@gmail.com", "setup scheduler", "setup scheduler");
        }
    }
}
