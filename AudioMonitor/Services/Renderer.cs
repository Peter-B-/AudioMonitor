using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AudioMonitor.Services
{
    public interface IRenderer
    {
        void Render();
        BitmapSource LevelBmp { get; }
    }

    public class Renderer : IRenderer
    {
        private readonly WriteableBitmap levelBmp;

        public Renderer()
        {
            levelBmp = BitmapFactory.New(Width, Height);
        }

        public void Render()
        {
            var rand = new Random();

            var color = Color.FromRgb((byte) rand.Next(0, 256), (byte) rand.Next(0, 256), (byte) rand.Next(0, 256));

            using (levelBmp.GetBitmapContext())
            {
                levelBmp.Clear(color);
            }
        }


        public int Width { get; } = 800;
        public int Height { get; } = 512;

        public BitmapSource LevelBmp => levelBmp;
    }
}