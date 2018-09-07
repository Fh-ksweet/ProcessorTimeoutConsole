using Newtonsoft.Json;
using ProcessorTimeoutConsole.Interfaces;
using ProcessorTimeoutConsole.SystemWrappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessorTimeoutConsole
{
    public class PerformanceMonitor : IPerformanceMonitor
    {
        private IList<PerformanceRecord> Records { get; } = new List<PerformanceRecord>();
        private readonly ISystemStopwatch _stopwatch;

        public PerformanceMonitor(ISystemStopwatch stopwatch)
        {
            _stopwatch = stopwatch;
        }

        public string GetLogMessage()
        {
            long totalTime = Records.Sum(r => r.Milliseconds);
            List<PerformanceRecord> recordsToReturn = new List<PerformanceRecord> { new PerformanceRecord { Name = "TotalTime", Milliseconds = totalTime } };
            recordsToReturn.AddRange(Records);
            return JsonConvert.SerializeObject(recordsToReturn);
        }

        public string GetSpecificRecordTime(string methodName)
        {
            var recordToReturn = Records.SingleOrDefault(r => r.Name == methodName);
            return recordToReturn != null ? $"{recordToReturn.Milliseconds} Milliseconds" : null;
        }

        public TimeSpan GetTimeTakenToExecute(string recordName)
        {
            var recordToReturn = Records.SingleOrDefault(r => r.Name == recordName);
            var timeSpan = TimeSpan.FromMilliseconds(recordToReturn.Milliseconds);
            return timeSpan;
        }

        public long GetLoadTime(string recordName)
        {
            var recordToReturn = Records.SingleOrDefault(r => r.Name == recordName);
            return recordToReturn?.Milliseconds ?? 0;
        }

        public void Run(Action action, string name)
        {
            _stopwatch.StartNew();
            action();
            _stopwatch.Stop();
            Records.Add(new PerformanceRecord { Name = name, Milliseconds = _stopwatch.ElapsedMilliseconds });
        }

        public TResult Run<TResult>(Func<TResult> action, string name)
        {
            _stopwatch.StartNew();
            var result = action();
            _stopwatch.Stop();
            Records.Add(new PerformanceRecord { Name = name, Milliseconds = _stopwatch.ElapsedMilliseconds });
            return result;
        }

        private class PerformanceRecord
        {
            public string Name { get; set; }
            public long Milliseconds { get; set; }
        }
    }
}
