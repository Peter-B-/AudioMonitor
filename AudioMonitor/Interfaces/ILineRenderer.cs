using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace AudioMonitor.Interfaces
{
    public interface IRenderer
    {
        BitmapSource Bitmap { get; }
        int Width { get; }
        int Height { get; }
    }

    public interface ILineRenderer : IRenderer
    {
        void Render(IList<float> rmsValues);
        float Scale { get; }
    }

    public interface IFftRenderer : IRenderer
    {
        void Render(IList<float> fftValues);
    }
}