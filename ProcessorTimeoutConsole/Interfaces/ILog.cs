namespace ProcessorTimeoutConsole.Interfaces
{
    public interface ILog
    {
        void Alert(string message);
        void Error(string message);
        void Info(string message);
        void Prompt(string message);
    }
}
