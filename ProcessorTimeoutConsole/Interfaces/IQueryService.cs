namespace ProcessorTimeoutConsole.Interfaces
{
    public interface IQueryService
    {
        void RunBrokenQuery(int runCount);
        void RunWorkingQuery(int runCount);
    }
}
