using ProcessorTimeoutConsole.Interfaces;
using System.Data;

namespace ProcessorTimeoutConsole
{
    public class Application
    {
        private readonly ILog _logger;
        private readonly IQueryService _queryService;
        private readonly IWriter _writer;

        public Application(ILog logger,
            IQueryService queryService,
            IWriter writer)
        {
            _logger = logger;
            _queryService = queryService;
            _writer = writer;
        }

        public void Run()
        {
            try
            {
                _logger.Info(nameof(Application) + " started.");

                _writer.Linebreak();

                _queryService.RunWorkingQuery();

                _writer.Linebreak();

                _queryService.RunBrokenQuery();

                _writer.Linebreak();

                _logger.Info(nameof(Application) + " finished.");
            }
            catch (DataException ex)
            {
                _logger.Error($"Exception caught ---> {ex.InnerException}");
            }
        }
    }
}
