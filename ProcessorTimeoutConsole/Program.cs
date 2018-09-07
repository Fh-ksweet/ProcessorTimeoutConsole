using StructureMap;
using System;
using static System.Console;

namespace ProcessorTimeoutConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Container.For<ConsoleRegistry>();

            var app = container.GetInstance<Application>();

            bool shouldRun = true;

            while (shouldRun)
            {
                app.Run();

                WriteLine();

                var color = ForegroundColor;
                ForegroundColor = ConsoleColor.Cyan;
                WriteLine("Run Again? Y/N y/n");
                ForegroundColor = color;

                var userRunAgainInput = ReadKey().KeyChar.ToString().ToLower();

                if (userRunAgainInput != "y") { shouldRun = false; }

                Clear();
            }
        }
    }
}
