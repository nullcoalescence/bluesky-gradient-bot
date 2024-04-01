using bluesky_gradient_bot.Services;
using FishyFlip;
using FishyFlip.Models;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

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

            using IHost host = builder.Build();

            // Authenticate
            var credentialService = host.Services.GetService<CredentialsService>();
            var creds = credentialService.ReadCredsTemp();

            var blueskyService = host.Services.GetService<BlueskyService>();
            await blueskyService.Login(creds);

            // Generate a gradient
            var gradientService = host.Services.GetService<GradientService>();
            var linearGradient = gradientService.GenerateLinearGradient();

            // Post
            //await blueskyService.PostImage("Test image", @"C:\Users\btov1\OneDrive\Pictures\alex_grey_1.JPG");
            
            await host.RunAsync();

            // Post it
        }

    }
}
 