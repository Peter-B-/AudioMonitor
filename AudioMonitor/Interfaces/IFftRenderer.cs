using System.Collections.Generic;

namespace AudioMonitor.Interfaces
{
    public interface IFftRenderer : IRenderer
    {
        void Render(IList<float> fftValues);
    }
}