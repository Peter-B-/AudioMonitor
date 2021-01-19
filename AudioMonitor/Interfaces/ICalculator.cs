using NAudio.Wave;

namespace AudioMonitor.Interfaces
{
    public interface ICalculator
    {
        float GetRms(IWaveBuffer waveBuffer);
    }
}