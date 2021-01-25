using System.Collections;
using System.Collections.Generic;

namespace AudioMonitor.Interfaces
{
    public interface ILineRenderer : IRenderer
    {
        void Render(IList<float> rmsValues);
        float Scale { get; }
    }
}