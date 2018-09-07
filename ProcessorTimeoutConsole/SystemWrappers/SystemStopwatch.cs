using System;
using System.Diagnostics;

namespace ProcessorTimeoutConsole.SystemWrappers
{
    public class SystemStopwatch : ISystemStopwatch
    {
        private Stopwatch _stopwatch;

        public void StartNew()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void Stop()
        {
            if (_stopwatch == null) throw new InvalidOperationException();
            _stopwatch.Stop();
        }

        public long ElapsedMilliseconds
        {
            get
            {
                if (_stopwatch == null) throw new InvalidOperationException();
                return _stopwatch.ElapsedMilliseconds;
            }
        }
    }
}
