using System;
using AudioMonitor.Interfaces;
using NAudio.Wave;

namespace AudioMonitor.Services
{
    public class Calculator:ICalculator
    {
        public float GetRms(IWaveBuffer waveBuffer)
        {
            if (waveBuffer.ShortBufferCount == 0) return 0.0f;
            
            long squareSum=0;
            
            for (var i = 0; i < waveBuffer.ShortBufferCount; i++)
                squareSum += waveBuffer.ShortBuffer[i] * waveBuffer.ShortBuffer[i];
            
            var rms = (float)Math.Sqrt((double)squareSum / waveBuffer.ShortBufferCount) / short.MaxValue;
            return rms;
        }
    }
}