using bluesky_gradient_bot.Models;
using Microsoft.Extensions.Logging;

namespace bluesky_gradient_bot.Services
{
    internal class GradientService
    {
        private ILogger<GradientService> logger;
        private Random random;

        public GradientService(ILogger<GradientService> logger)
        {
            this.logger = logger;
            this.random = new Random();
        }

        public LinearGradient GenerateLinearGradient()
        {
            this.logger.LogInformation("Creating linear gradient");
            var gradient = new LinearGradient();

            gradient.Angle = GetRandomNumInRange(0, 359);

            var numColors = GetRandomNumInRange(2, 5);
            gradient.Colors = new List<string>();
            for (int i = 0; i < numColors; i++)
            {
                gradient.Colors.Add(GetRandomColorHex());
            }

            logger.LogInformation($"Created linear gradient: {gradient.ToString()}");
            logger.LogInformation($"CSS: {gradient.ToCss()}");
            return gradient;
        }

        public RadialGradient GenerateRadialGradient()
        {
            this.logger.LogInformation("Creating radial gradient");
            var gradient = new RadialGradient();

            gradient.Shape = GetRandomNumInRange(0, 2) == 0 ? "circle" : "eclipse";

            int randSize = GetRandomNumInRange(0, 4);
            switch (randSize)
            {
                case 0:
                    gradient.Size = "closest-side";
                    break;
                case 1:
                    gradient.Size = "farthest-side";
                    break;
                case 2:
                    gradient.Size = "closest-corner";
                    break;
                case 3:
                    gradient.Size = "farthest-corner";
                    break;
                default:
                    throw new Exception("Catostrophic failure");
            }

            var numColors = GetRandomNumInRange(2, 5);
            for (int i = 0; i < numColors; i++)
            {
                gradient.Colors.Add(GetRandomColorHex());
            }

            logger.LogInformation($"Created radial gradient: {gradient.ToString()}");
            logger.LogInformation($"CSS: {gradient.ToCss()}");
            return gradient;
        }

        /*
        * Helper funcs
        */

        private int GetRandomNumInRange(int min, int max)
        {
            return this.random.Next(min, max);
        }

        private string GetRandomColorHex()
        {
            return String.Format("#{0:X6}", this.random.Next(0x1000000));
        }
    }
}