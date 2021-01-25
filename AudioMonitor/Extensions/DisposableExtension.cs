using System;
using System.Globalization;
using System.Reactive.Disposables;

namespace AudioMonitor.Extensions
{
    public static class DisposableExtension
    {
        public static T AddDisposableTo<T>(this T disposable, CompositeDisposable composite) where T:IDisposable
        {
            composite.Add(disposable);
            return disposable;
        }
    }
}