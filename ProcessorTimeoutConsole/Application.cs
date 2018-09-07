using ProcessorTimeoutConsole.Interfaces;
using System;
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
                _logger.Alert(nameof(Application) + " started.");

                _logger.Prompt("1 - Run Only Working Query");
                _logger.Prompt("2 - Run Only Broken Query");
                _logger.Prompt("3 - Run both queries");
                var queriesToRun = Console.ReadKey().KeyChar.ToString().ToLower();

                _writer.Linebreak();

                _logger.Prompt("How many times would you like to run the queries?");
                var timesToRun = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

                switch (queriesToRun)
                {
                    case "1":
                        RunOnlyWorkingQuery(timesToRun);
                        break;
                    case "2":
                        RunOnlyBrokenQuery(timesToRun);
                        break;
                    case "3":
                        RunBothQueries(timesToRun);
                        break;
                }
                _logger.Alert(nameof(Application) + " finished.");
            }
            catch (DataException ex)
            {
                _logger.Error($"Exception caught ---> {ex.InnerException}");
            }
        }

        private void RunBothQueries(int timesToRun)
        {
            for (var i = 1; i <= timesToRun; i++)
            {
                _queryService.RunWorkingQuery(i);
                _queryService.RunBrokenQuery(i);

                _logger.Alert($"Total query calls --> {i}");
            }
            _writer.Linebreak();
        }

        private void RunOnlyBrokenQuery(int timesToRun)
        {
            for (var i = 1; i <= timesToRun; i++)
            {
                _queryService.RunBrokenQuery(i);

                _logger.Alert($"Total query calls --> {i}");
            }
            _writer.Linebreak();
        }

        private void RunOnlyWorkingQuery(int timesToRun)
        {
            for (var i = 1; i <= timesToRun; i++)
            {
                _queryService.RunWorkingQuery(i);
                _logger.Alert($"Total query calls --> {i}");
            }
            _writer.Linebreak();
        }
    }
}
