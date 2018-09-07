using StructureMap;
using static System.Console;

namespace ProcessorTimeoutConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Container.For<ConsoleRegistry>();

            var app = container.GetInstance<Application>();

            app.Run();

            ReadKey();
        }
    }
}
