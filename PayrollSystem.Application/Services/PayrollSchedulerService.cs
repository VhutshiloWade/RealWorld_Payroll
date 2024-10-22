using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using PayrollSystem.Application.Jobs;

namespace PayrollSystem.Application.Services
{
    public class PayrollSchedulerService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private IScheduler _scheduler;

        public PayrollSchedulerService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;

            var job = JobBuilder.Create<PayrollJob>()
                .WithIdentity("payrollJob", "payrollGroup")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("payrollTrigger", "payrollGroup")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 0, 0)) // Run on the 1st day of every month at 00:00
                .Build();

            await _scheduler.ScheduleJob(job, trigger, cancellationToken);

            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_scheduler != null)
            {
                await _scheduler.Shutdown(cancellationToken);
            }
        }
    }
}
