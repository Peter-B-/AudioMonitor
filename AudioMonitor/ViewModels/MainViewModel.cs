using System;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Windows.Media.Imaging;
using AudioMonitor.Services;
using TinyLittleMvvm;

namespace AudioMonitor.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private readonly IRenderer renderer;
        public BitmapSource LevelBmp => renderer.LevelBmp;

        public MainViewModel(IRenderer renderer)
        {
            this.renderer = renderer;



            Observable.Interval(TimeSpan.FromSeconds(0.5))
                .ObserveOnDispatcher()
                .Subscribe(_ => renderer.Render());
                ;
        }
    }
}