using ProcessorTimeoutConsole.Interfaces;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProcessorTimeoutConsole.Services
{
    public class QueryService : IQueryService
    {
        private readonly ILog _logger;
        private readonly IPerformanceMonitor _perfMonitor;
        private readonly IQueryStringCreationService _queryStringCreationService;
        private readonly IWriter _writer;

        public QueryService(ILog logger,
            IPerformanceMonitor perfMonitor,
            IQueryStringCreationService queryStringCreationService,
            IWriter writer)
        {
            _logger = logger;
            _perfMonitor = perfMonitor;
            _queryStringCreationService = queryStringCreationService;
            _writer = writer;
        }

        public void RunBrokenQuery()
        {
            var brokenQuery = _queryStringCreationService.CreateQueryTextForBrokenQuery();

            _logger.Info("Starting broken query");

            var brokenCall = _perfMonitor.Run(() => QueryDTADO(brokenQuery), "Run Broken Query");
            var recordCount = brokenCall.Rows.Count;
            var loadTime = _perfMonitor.GetTimeTakenToExecute("Run Broken Query");

            _writer.WriteLine($"Load Time Took {loadTime} and loaded {recordCount} records");
        }

        public void RunWorkingQuery()
        {
            var workingQuery = _queryStringCreationService.CreateQueryTextForWorkingQueryString();

            _logger.Info("Starting working query");

            var workingCall = _perfMonitor.Run(() => QueryDTADO(workingQuery), "Run Working Query");
            var recordCount = workingCall.Rows.Count;
            var loadTime = _perfMonitor.GetTimeTakenToExecute("Run Working Query");

            _writer.WriteLine($"Load Time Took {loadTime} and loaded {recordCount} records");
        }

        private static DataTable QueryDTADO(string QueryString)
        {
            var connString = ConfigurationManager.ConnectionStrings["SQLSERVERADONET35"].ConnectionString;

            var oConnection = new SqlConnection(connString);
            oConnection.Open();
            var oCommand = new SqlCommand(QueryString, oConnection);
            var oAdapter = new SqlDataAdapter(oCommand);
            var dt = new DataTable();
            try
            {
                oAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new DataException(QueryString + " --> " + ex);
            }
            oConnection.Close();
            return dt;
        }
    }
}
