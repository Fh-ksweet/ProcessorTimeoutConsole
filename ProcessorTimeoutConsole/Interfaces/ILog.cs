﻿namespace ProcessorTimeoutConsole.Interfaces
{
    public interface ILog
    {
        void Error(string message);
        void Info(string message);
    }
}
