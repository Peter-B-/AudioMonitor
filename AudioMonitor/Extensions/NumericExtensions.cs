namespace AudioMonitor.Extensions
{
    public static class NumericExtensions
    {
        public static int Clip(this int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}