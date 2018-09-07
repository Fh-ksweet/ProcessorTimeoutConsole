namespace ProcessorTimeoutConsole.SystemWrappers
{
    public interface ISystemStopwatch
    {
        void StartNew();
        void Stop();
        long ElapsedMilliseconds { get; }
    }
}
