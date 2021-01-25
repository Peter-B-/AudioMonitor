using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AudioMonitor.Extensions;
using AudioMonitor.Interfaces;

namespace AudioMonitor.Services
{
    public class LineRenderer : Renderer, ILineRenderer
    {
        private int lastY = 0;

        public void Render(float rmsValue)
        {
            using (LevelBmp.GetBitmapContext())
            {
                var level = (int)(rmsValue * Scale * Height);
                var y = Height - 1 - level;
                y = y.Clip(0, Height - 1);

                var x = NextLine;

                if (x > 0)
                    LevelBmp.DrawLine(x -1 , lastY, x, y, Colors.CornflowerBlue);
                
                lastY = y;

                NextLine++;
                if (NextLine >= Width) NextLine = 0;

                RenderBlank(NextLine, BlankWidth);
            }
        }


        public float Scale { get; } = 4;
    }
}