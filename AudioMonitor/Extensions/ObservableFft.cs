using System;
using System.Linq;
using System.Reactive.Linq;
using NAudio.Dsp;
using NAudio.Wave;

namespace AudioMonitor.Extensions
{
    public static class ObservableFftExtensions
    {
        public static IObservable<float[]> CalculateFft(this IObservable<IWaveBuffer> source, int fftLength = 1024)
        {
            var fft = new ObservableFft(fftLength, source);
            return Observable.Create<float[]>(fft.Subscribe);
        }
    }

    public class ObservableFft
    {
        private readonly int fftLength;
        private readonly int m;
        private readonly IObservable<IWaveBuffer> source;

        public ObservableFft(int fftLength, IObservable<IWaveBuffer> source)
        {
            if (!IsPowerOfTwo(fftLength))
                throw new ArgumentException("FFT Length must be a power of two", nameof(fftLength));
            this.fftLength = fftLength;
            this.source = source;
            m = (int) Math.Log(fftLength, 2.0);
        }

        private static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public IDisposable Subscribe(IObserver<float[]> obs)
        {
            var fftBuffer = new Complex[fftLength];
            var fftPos = 0;

            return
                source.Subscribe(wave =>
                    {
                        for (var i = 0; i < wave.ShortBufferCount; i++)
                        {
                            var value = wave.ShortBuffer[i];
                            fftBuffer[fftPos].X = (float) (value * FastFourierTransform.HannWindow(fftPos, fftLength));
                            fftBuffer[fftPos].Y = 0;
                            fftPos++;
                            if (fftPos >= fftBuffer.Length)
                            {
                                fftPos = 0;
                                FastFourierTransform.FFT(true, m, fftBuffer);

                                var result =
                                    fftBuffer
                                        .Take(fftLength / 2)
                                        .Select(GetDb)
                                        .ToArray();
                                obs.OnNext(result);
                            }
                        }
                    },
                    obs.OnError,
                    obs.OnCompleted
                );
        }

        private float GetDb(Complex c)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 10 from here http://stackoverflow.com/a/10636698/7532
            return (float) (10 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y)));
        }
    }
}