using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using System.Text;

namespace bluesky_gradient_bot.Services
{
    /*
     *  Spin up a headless browser to screen-capture gradients
     */

    internal class GradientCaptureService
    {
        private ILogger<GradientCaptureService> logger;

        public GradientCaptureService(ILogger<GradientCaptureService> logger)
        {
            this.logger = logger;
        }

        public async Task<string> BuildHeadlessBrowserAndCaptureGradientScreenshot(string screenshotPath, string gradientCss)
        {
            var html = BuildHtmlString(gradientCss);
            this.logger.LogInformation($"Generated gradient html: {html}");

            this.logger.LogInformation("Building headless browser to take screenshot");
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);

            await page.ScreenshotAsync(screenshotPath);

            this.logger.LogInformation($"Took screenshot, saved to {screenshotPath}");
            return screenshotPath;
        }

        private string BuildHtmlString(string gradientCss)
        {
            var sb = new StringBuilder();
            sb.Append("<!doctype html>");
            sb.Append("<html lang='en'>");
            sb.Append("<head>");
            sb.Append("<style type='text/css'>");
            sb.Append("html { height: 100%; }");
            sb.Append("body { background-image: ");
            sb.Append(gradientCss);
            sb.Append("; }");
            sb.Append("</style>");
            sb.Append("</head>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}
