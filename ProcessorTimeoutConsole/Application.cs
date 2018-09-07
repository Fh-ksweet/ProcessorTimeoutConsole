using ProcessorTimeoutConsole.Interfaces;
using System;
using static System.Console;

namespace ProcessorTimeoutConsole
{
    public class Application
    {
        private readonly ILog _logger;
        private readonly IQueryService _queryService;

        public Application(ILog logger, IQueryService queryService)
        {
            _logger = logger;
            _queryService = queryService;
        }

        public void Run()
        {
            try
            {
                _logger.Info(nameof(Application) + " started.");

                WriteLine();

                _queryService.RunWorkingQuery();

                WriteLine();

                _queryService.RunBrokenQuery();

                WriteLine();

                _logger.Info(nameof(Application) + " finished.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception caught ---> {ex.InnerException}");
            }
        }
    }
}
