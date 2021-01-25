using System.Windows.Media.Imaging;

namespace AudioMonitor.Interfaces
{
    public interface IRenderer
    {
        BitmapSource Bitmap { get; }
        int Width { get; }
        int Height { get; }
    }
}