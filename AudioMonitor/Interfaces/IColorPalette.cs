using System.Windows.Media;

namespace AudioMonitor.Interfaces
{
    public interface IColorPalette
    {
        Color GetColor(float value);
    }
}