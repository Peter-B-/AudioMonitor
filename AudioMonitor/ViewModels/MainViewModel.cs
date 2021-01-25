using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Windows.Media.Imaging;
using AudioMonitor.Extensions;
using AudioMonitor.Interfaces;
using AudioMonitor.Models;
using AudioMonitor.Services;
using TinyLittleMvvm;

namespace AudioMonitor.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private readonly ILineRenderer lineRenderer;
        private readonly IFftRenderer fftRenderer;
        private readonly IAudioSource audioSource;
        private readonly ICalculator calculator;
        private readonly SerialDisposable subscription = new SerialDisposable();

        private RecordDevice selectedDevice;

        public BitmapSource LevelBmp => lineRenderer.Bitmap;
        public BitmapSource FftBmp => fftRenderer.Bitmap;

        public IReadOnlyList<RecordDevice> Devices { get; }

        public RecordDevice SelectedDevice
        {
            get => selectedDevice;
            set
            {
                selectedDevice = value;
                NotifyOfPropertyChange(() => SelectedDevice);
                SetupSubscription(SelectedDevice);
            }
        }

        private void SetupSubscription(RecordDevice device)
        {
            if (device == null) return;

            var disp = new CompositeDisposable();
            subscription.Disposable = disp;

            var audioStream =
                audioSource.GetStream(device)
                    .Publish()
                    .RefCount();

            audioStream
                .Select(calculator.GetRms)
                .ObserveOnDispatcher()
                .Subscribe(rmsValues => lineRenderer.Render(rmsValues))
                .AddDisposableTo(disp)
                ;

            audioStream
                .CalculateFft(512)
                .ObserveOnDispatcher()
                .Subscribe(fftRenderer.Render)
                .AddDisposableTo(disp)
                ;

        }

        public MainViewModel(ILineRenderer lineRenderer, IFftRenderer fftRenderer, IAudioSource audioSource, ICalculator calculator)
        {
            this.lineRenderer = lineRenderer;
            this.fftRenderer = fftRenderer;
            this.audioSource = audioSource;
            this.calculator = calculator;

            Devices = audioSource.EnumerateDevices().ToList().AsReadOnly();
            SelectedDevice = Devices.FirstOrDefault();
        }
    }
}