using ProcessorTimeoutConsole.Interfaces;
using System;

namespace ProcessorTimeoutConsole.Services
{
    public class Writer : IWriter
    {
        public void Linebreak()
        {
            Console.WriteLine();
        }

        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }
    }
}
