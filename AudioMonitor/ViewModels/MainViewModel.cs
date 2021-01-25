using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Windows.Media.Imaging;
using AudioMonitor.Extensions;
using AudioMonitor.Interfaces;
using AudioMonitor.Services;
using TinyLittleMvvm;

namespace AudioMonitor.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private readonly ILineRenderer lineRenderer;
        private readonly IFftRenderer fftRenderer;

        public BitmapSource LevelBmp => lineRenderer.Bitmap;
        public BitmapSource FftBmp => fftRenderer.Bitmap;

        public MainViewModel(ILineRenderer lineRenderer, IFftRenderer fftRenderer, IAudioSource audioSource, ICalculator calculator)
        {
            this.lineRenderer = lineRenderer;
            this.fftRenderer = fftRenderer;

            var device = audioSource.EnumerateDevices().FirstOrDefault();

            if (device == null) return;

            var audioStream = 
                audioSource.GetStream(device)
                    .Publish()
                    .RefCount();

            audioStream
                .Select(calculator.GetRms)
                .Buffer(lineRenderer.Width, 1)
                .ObserveOnDispatcher()
                .Subscribe(lineRenderer.Render);

            audioStream
                .CalculateFft(512)
                .ObserveOnDispatcher()
                .Subscribe(fftRenderer.Render);
        }
    }
}