using ProcessorTimeoutConsole.Interfaces;
using System;

namespace ProcessorTimeoutConsole.Services
{
    public class Writer : IWriter
    {
        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }
    }
}
