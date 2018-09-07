namespace ProcessorTimeoutConsole.Interfaces
{
    public interface IQueryStringCreationService
    {
        string CreateQueryTextForBrokenQuery();
        string CreateQueryTextForWorkingQueryString();
    }
}
