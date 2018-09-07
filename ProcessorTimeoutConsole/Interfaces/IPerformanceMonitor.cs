using System;

namespace ProcessorTimeoutConsole.Interfaces
{
    public interface IPerformanceMonitor
    {
        string GetLogMessage();
        string GetSpecificRecordTime(string methodName);
        void Run(Action action, string name);
        TResult Run<TResult>(Func<TResult> action, string name);
        TimeSpan GetTimeTakenToExecute(string recordName);
        long GetLoadTime(string recordName);
    }
}
