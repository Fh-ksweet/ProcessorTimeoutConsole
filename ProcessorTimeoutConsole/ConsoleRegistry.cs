using ProcessorTimeoutConsole.Interfaces;
using ProcessorTimeoutConsole.Services;
using StructureMap;

namespace ProcessorTimeoutConsole
{
    public class ConsoleRegistry : Registry
    {
        public ConsoleRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
            // requires explicit registration; doesn't follow convention
            For<ILog>().Use<LogService>();
        }
    }
}
