using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using AudioMonitor.Interfaces;
using AudioMonitor.Models;
using NAudio.Wave;

namespace AudioMonitor.Services
{
    public class AudioSource : IAudioSource
    {
        public IEnumerable<RecordDevice> EnumerateDevices()
        {
            return
            Enumerable.Range(0, WaveIn.DeviceCount)
                .Select(n =>
                {
                    var capabilities = WaveIn.GetCapabilities(n);
                    return new RecordDevice(n, capabilities.ProductName);
                });
        }

        public IObservable<IWaveBuffer> GetStream(RecordDevice device)
        {
            return
                Observable.Create<IWaveBuffer>(obs =>
                {
                    var disposables = new CompositeDisposable();

                    var samples = 512;
                    var timeMs = 20;
                    var rate = samples * 1000 / timeMs;

                    var waveIn = new WaveInEvent()
                    {
                        DeviceNumber = device.DeviceNumber,
                        WaveFormat = new WaveFormat(rate, 16, 1),
                        BufferMilliseconds = timeMs

                    };

                    var subscription =
                        Observable.FromEventPattern<WaveInEventArgs>(
                                h => waveIn.DataAvailable += h,
                                h => waveIn.DataAvailable -= h)
                            .Where(e => 
                                // Remove events where Buffer is not filled completely.
                                // This will happen, when recording is stopped.
                                e.EventArgs.BytesRecorded == e.EventArgs.Buffer.Length)
                            .Select(e =>
                            {
                                var buffer = new WaveBuffer(e.EventArgs.Buffer)
                                {
                                    ByteBufferCount = e.EventArgs.BytesRecorded
                                };

                                return buffer;
                            })
                            .Subscribe(obs);

                    disposables.Add(Disposable.Create(() => waveIn.StopRecording()));
                    disposables.Add(subscription);
                    disposables.Add(waveIn);
                    
                    waveIn.StartRecording();
                    
                    return disposables;
                });
        }
    }
}