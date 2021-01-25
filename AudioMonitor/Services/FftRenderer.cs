using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AudioMonitor.Extensions;
using AudioMonitor.Interfaces;

namespace AudioMonitor.Services
{
    public class FftRenderer : IFftRenderer
    {
        private readonly IColorPalette palette;
        private readonly WriteableBitmap levelBmp;

        private int nextLine = 0;

        public FftRenderer(IColorPalette palette)
        {
            this.palette = palette;
            levelBmp = BitmapFactory.New(Width, Height);
            using (levelBmp.GetBitmapContext())
            {
                levelBmp.Clear(Colors.WhiteSmoke);
            }
        }

        public void Render(IList<float> fftValues)
        {
            if (fftValues.IsEmpty()) return;

            using (levelBmp.GetBitmapContext())
            {
                var x = nextLine;

                for (var y = 0; y < fftValues.Count; y++)
                {
                    var color = palette.GetColor(fftValues[y]);
                    levelBmp.SetPixel(x, Height - y - 1, color);
                }

                nextLine++;
                if (nextLine >= Width) nextLine = 0;

                RenderBlank(nextLine, BlankWidth);
                if (nextLine + BlankWidth >= Width)
                    RenderBlank(0, BlankWidth - (Width - nextLine));

            }
        }

        private void RenderBlank(int start, int width)
        {
            if (width < 0)
                throw new ArgumentException("width must be positive", nameof(width));
            if (start >= Width)
                throw new ArgumentException("start must inside canvas", nameof(start));

            if (start + width >= Width)
                width = Width - start;

            levelBmp.DrawRectangle(start, 0, start + width, Height, Colors.WhiteSmoke);
        }

        public int BlankWidth { get; set; } = 10;
        public float Scale { get; } = 4;
        public int Width { get; } = 800;
        public int Height { get; } = 256;

        public BitmapSource Bitmap => levelBmp;
    }
}