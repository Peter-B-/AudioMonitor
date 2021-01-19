using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Windows.Media.Imaging;
using AudioMonitor.Interfaces;
using AudioMonitor.Services;
using TinyLittleMvvm;

namespace AudioMonitor.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private readonly IRenderer renderer;
        private readonly IAudioSource audioSource;
        public BitmapSource LevelBmp => renderer.LevelBmp;

        public MainViewModel(IRenderer renderer, IAudioSource audioSource, ICalculator calculator)
        {
            this.renderer = renderer;
            this.audioSource = audioSource;

            var device = audioSource.EnumerateDevices().FirstOrDefault();

            if (device == null) return;

            audioSource.GetStream(device)
                .Select(calculator.GetRms)
                .Buffer(renderer.Width, 1)
                .ObserveOnDispatcher()
                .Subscribe(renderer.Render);
        }
    }
}