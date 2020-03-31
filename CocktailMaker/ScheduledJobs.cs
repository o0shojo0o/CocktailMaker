using FluentScheduler;
using Serilog;

namespace CocktailMaker
{
    public class ScheduledJobs : Registry
    {
        public ScheduledJobs()
        {
            // Update Check
            Schedule(() => null).WithName("Container Update Check").ToRunNow().AndEvery(1).Hours();

            // Database Sync Check
            Schedule(() => Database.CheckUpdate()).WithName("Database Update Check").ToRunNow().AndEvery(1).Hours();

            // Read System Stats 
            Schedule(() => null).WithName("Read CPU Temp").ToRunNow().AndEvery(10).Seconds();
        }
    }
}
