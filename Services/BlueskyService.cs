using bluesky_gradient_bot.Models;
using FishyFlip;
using FishyFlip.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;

namespace bluesky_gradient_bot.Services
{
    internal class BlueskyService
    {
        private ILogger<BlueskyService> logger;
        private ATProtocol protocol;

        public BlueskyService(ILogger<BlueskyService> logger)
        {
            this.logger = logger;

            this.logger.LogInformation("Building AT Protocol instance");

            var atProtocolBuilder = new ATProtocolBuilder().EnableAutoRenewSession(true);
            this.protocol = atProtocolBuilder.Build();
        }

        public async Task Login(LoginCredential credential)
        {
            this.logger.LogInformation("Logging in");

            Result<Session> result = await this.protocol.Server.CreateSessionAsync(credential.Username, credential.Password, CancellationToken.None);

            result.Switch(
                success =>
                {
                    this.logger.LogInformation($"Logged in as {credential.Username}");
                    this.logger.LogInformation($"Session: {success.Did}");
                },
                error =>
                {
                    this.logger.LogError($"Failed to login: {error.StatusCode} {error.Detail}");
                }
            );
        }

        public async Task PostText(string text)
        {
            var postResult = await this.protocol.Repo.CreatePostAsync(text);

            postResult.Switch(
                success =>
                {
                    this.logger.LogInformation($"Successfully sent text post: {success.Uri} {success.Cid}");
                },
                error =>
                {
                    this.logger.LogError($"Error sending text post: {error.StatusCode} {error.Detail}");
                });
        }

        public async Task PostImage(string text, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"Image not found ${imagePath}");
            }

            var stream = File.OpenRead(imagePath);

            var content = new StreamContent(stream);
            content.Headers.ContentLength = stream.Length;
            content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            var blobResult = await this.protocol.Repo.UploadBlobAsync(content);
            await blobResult.SwitchAsync(
                async success =>
                {
                    this.logger.LogError($"Blob success: {success.Blob.Type}");
                    Image? img = success.Blob.ToImage();

                    var postText = $"{text} IMAGE_HERE";

                    int postStart = postText.IndexOf("IMAGE_HERE", StringComparison.InvariantCulture);
                    int postEnd = postStart + Encoding.Default.GetBytes("IMAGE_HERE").Length;

                    var index = new FacetIndex(postStart, postEnd);

                    var postResult = await this.protocol.Repo.CreatePostAsync(text, null, new ImagesEmbed(img, ""));
                },
                async error =>
                {
                    this.logger.LogError($"Error posting image: {error.StatusCode}: {error.Detail}");
                });
        }
    }
}