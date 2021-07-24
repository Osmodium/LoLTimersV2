using System.Windows.Media.Imaging;

namespace LoLTimers.DataTypes
{
    public record Spell
    {
        public string Name { get; set; }
        public float Cooldown { get; set; }
        public BitmapImage Image { get; set; }
    }
}
