using System;
using System.Collections.Generic;
using AudioMonitor.Models;
using NAudio.Wave;

namespace AudioMonitor.Interfaces
{
    public interface IAudioSource
    {
        IEnumerable<RecordDevice> EnumerateDevices();
        public IObservable<IWaveBuffer> GetStream(RecordDevice device);
    }
}