using System.Text;

namespace bluesky_gradient_bot.Models
{
    internal class LinearGradient : IGradient
    {
        public int Angle { get; set; }
        public List<string> Colors { get; set;} = new List<string>();
        
        override
        public string ToString()
        {
            return $"Linear gradient: {Angle}deg. Colors: {String.Join(", ", Colors)}";
        }

        public string ToCss()
        {
            var sb = new StringBuilder();
            sb.Append("linear-gradient");
            sb.Append("(");
            sb.Append($"{Angle}deg");
          
            for (var i = 0; i < Colors.Count; i++)
            {
                sb.Append($", {Colors[i]}");
            }

            sb.Append(")");

            return sb.ToString();
        }
    }
}
