using bluesky_gradient_bot.Models;

namespace bluesky_gradient_bot.Services
{
    internal class CredentialsService
    {
        private string filePath;

        public CredentialsService(string filePath)
        {            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Credentials file path not found: {filePath}");
            }

            this.filePath = filePath;
        }

        public LoginCredential ReadCredsTemp()
        {
            var content = File.ReadAllText(this.filePath);
            string[] split = content.Split(',');

            return new LoginCredential
            {
                Username = split[0],
                Password = split[1]
            };
        }
    }
}
