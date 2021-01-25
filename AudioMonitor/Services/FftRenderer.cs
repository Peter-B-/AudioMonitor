using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AudioMonitor.Interfaces;

namespace AudioMonitor.Services
{
    public class Renderer
    {
        protected readonly WriteableBitmap LevelBmp;
        protected int NextLine;
        private readonly Color background = Colors.WhiteSmoke;

        public Renderer()
        {
            LevelBmp = BitmapFactory.New(Width, Height);
            using (LevelBmp.GetBitmapContext())
            {
                LevelBmp.Clear(background);
            }
        }

        public int BlankWidth { get; set; } = 10;
        public int Width { get; } = 800;
        public int Height { get; } = 256;
        public BitmapSource Bitmap => LevelBmp;

        protected void RenderBlank(int start, int blankWidth)
        {
            if (blankWidth < 0)
                throw new ArgumentException("blankWidth must be positive", nameof(blankWidth));
            if (start >= Width)
                throw new ArgumentException("start must inside canvas", nameof(start));

            if (start + blankWidth >= Width)
            {
                RenderBlank(0, blankWidth - (Width - start));
                blankWidth = Width - start;
            }

            LevelBmp.DrawRectangle(start, 0, start + blankWidth, Height, background);

        }
    }

    public class FftRenderer : Renderer, IFftRenderer
    {
        private readonly IColorPalette palette;

        public FftRenderer(IColorPalette palette)
        {
            this.palette = palette;
        }

        public void Render(IList<float> fftValues)
        {
            if (fftValues.IsEmpty()) return;

            using (LevelBmp.GetBitmapContext())
            {
                var x = NextLine;

                for (var y = 0; y < fftValues.Count; y++)
                {
                    var color = palette.GetColor(fftValues[y]);
                    LevelBmp.SetPixel(x, Height - y - 1, color);
                }

                NextLine++;
                if (NextLine >= Width) NextLine = 0;

                RenderBlank(NextLine, BlankWidth);
            }
        }
    }
}