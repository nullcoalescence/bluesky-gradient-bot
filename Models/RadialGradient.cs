using System.Text;

namespace bluesky_gradient_bot.Models
{
    internal class RadialGradient
    {
        public string Shape { get; set; }
        public string Size { get; set; }
        public List<string> Colors { get; set; }

        override
        public string ToString()
        {
            return $"Radial gradient: Shape: {Shape}, Size: {Size}, Colors: {String.Join(", ", Colors)}";
        }

        public string ToCss()
        {
            var sb = new StringBuilder();
            sb.Append("radial-gradient");
            sb.Append("(");
            sb.Append(Shape);

            for (var i = 0; i < Colors.Count; i++)
            {
                sb.Append($", {Colors[i]}");
            }
            sb.Append(")");

            return sb.ToString();
        }
    }
}
