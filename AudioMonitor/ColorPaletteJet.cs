using System.Windows.Media;
using AudioMonitor.Interfaces;

namespace AudioMonitor
{
    public class ColorPaletteJet:IColorPalette
    {
        private readonly float min;
        private readonly float max;

        private readonly int steps = 1000;
        private readonly Color[] colors;

        

        public ColorPaletteJet(float min, float max)
        {
            this.min = min;
            this.max = max;
            // From plotly Jet palette:
            // https://github.com/plotly/plotly.py/blob/259fe1141e130018c85dd1b24a143a6278bdc634/packages/python/plotly/_plotly_utils/colors/plotlyjs.py
            var colorMap = new[]{
                new {Value = 0.0, Color = Color.FromRgb(0,0,131)},
                new {Value = 0.125, Color = Color.FromRgb(0,60,170)},
                new {Value = 0.375, Color = Color.FromRgb(5,255,255)},
                new {Value = 0.625, Color = Color.FromRgb(255,255,0)},
                new {Value = 0.875, Color = Color.FromRgb(250,0,0)},
                new {Value = 1.0, Color = Color.FromRgb(128,0,0)},
            };

            colors = new Color[steps + 1];

            var colorIdx = 1;
            for (int i = 0; i < colors.Length; i++)
            {
                var value = (double)i / steps;

                if (value > colorMap[colorIdx].Value && colorIdx < colorMap.Length - 1)
                    colorIdx++;

                var from = colorMap[colorIdx - 1];
                var to = colorMap[colorIdx];

                var proportion = (value - from.Value) / (to.Value - from.Value);

                var r = (byte)(from.Color.R * (1 - proportion) + to.Color.R * proportion);
                var g = (byte)(from.Color.G * (1 - proportion) + to.Color.G * proportion);
                var b = (byte)(from.Color.B * (1 - proportion) + to.Color.B * proportion);

                colors[i] = Color.FromRgb(r, g, b);
            }
		}

        public Color GetColor(float value)
        {
            var idx = (int)((value - min) / (max - min) * colors.Length);

            if (idx < 0) idx = 0;
            if (idx >= colors.Length) idx = colors.Length-1;

            return colors[idx];

        }
    }
}