using bluesky_gradient_bot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace bluesky_gradient_bot
{
    internal class Program
    {
       static async Task Main(string[] args)
       {
            // Build DI container
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<BlueskyService>();
            builder.Services.AddScoped<CredentialsService>(s => new CredentialsService(@"D:\Keystore\gradient-bot\creds.txt"));
            builder.Services.AddScoped<GradientService>();
            builder.Services.AddScoped<GradientCaptureService>();

            using IHost host = builder.Build();

            // Authenticate
            var credentialService = host.Services.GetService<CredentialsService>();
            var creds = credentialService.ReadCredsTemp();

            var blueskyService = host.Services.GetService<BlueskyService>();
            await blueskyService.Login(creds);

            // Generate a gradient
            var gradientService = host.Services.GetService<GradientService>();
            var linearGradient = gradientService.GenerateLinearGradient();

            // Save image
            var gradientCaptureService = host.Services.GetService<GradientCaptureService>();
            var screenshot = await gradientCaptureService.BuildHeadlessBrowserAndCaptureGradientScreenshot(
                @"D:\Projects\Dotnet\bluesky-gradient-bot\gradient.png",
                linearGradient.ToCss());

            // Post
            await blueskyService.PostImage(linearGradient.ToString(), screenshot);
            
            //await host.RunAsync();
        }

    }
}
 