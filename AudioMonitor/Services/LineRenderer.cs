using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AudioMonitor.Extensions;
using AudioMonitor.Interfaces;

namespace AudioMonitor.Services
{
    public class LineRenderer : ILineRenderer
    {
        private readonly WriteableBitmap levelBmp;

        public LineRenderer()
        {
            levelBmp = BitmapFactory.New(Width, Height);
        }

        public void Render(IList<float> rmsValues)
        {
            using (levelBmp.GetBitmapContext())
            {
                levelBmp.Clear(Colors.WhiteSmoke);

                if (rmsValues.IsEmpty()) return;

                var points =
                rmsValues
                    .Select(rms => (int)(rms * Scale * Height))
                    .Select(rms => rms.Clip(0, Height - 1))
                    .Select(v => Height - 1 - v)
                    .Take(Width - 1);

                var lastValue = points.First();
                var x = 0;
                foreach (var value in points)
                {
                    levelBmp.DrawLine(x, lastValue, x + 1, value, Colors.CornflowerBlue);
                    x++;
                    lastValue = value;
                }



            }
        }


        public float Scale { get; } = 4;
        public int Width { get; } = 800;
        public int Height { get; } = 512;

        public BitmapSource Bitmap => levelBmp;
    }
}