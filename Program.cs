using bluesky_gradient_bot.Services;
using FishyFlip;
using FishyFlip.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace bluesky_gradient_bot
{
    internal class Program
    {
       static void Main(string[] args)
        {
            // Build DI container
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<SessionCreationService>()
                .BuildServiceProvider();

            // Do work
            var sessionService = serviceProvider.GetService<SessionCreationService>();
            sessionService.Test();
        }

    }
}
 