using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace AudioMonitor.Interfaces
{
    public interface IRenderer
    {
        void Render(IList<float> rmsValues);
        BitmapSource LevelBmp { get; }
        float Scale { get; }
        int Width { get; }
        int Height { get; }
    }
}