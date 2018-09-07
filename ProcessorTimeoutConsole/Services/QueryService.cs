using ProcessorTimeoutConsole.Interfaces;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProcessorTimeoutConsole.Services
{
    public class QueryService : IQueryService
    {
        private readonly IQueryStringCreationService _queryStringCreationService;
        private readonly IPerformanceMonitor _perfMonitor;
        private readonly ILog _logger;
        private readonly IWriter _writer;

        public QueryService(IQueryStringCreationService queryStringCreationService,
            IPerformanceMonitor perfMonitor,
            ILog logger,
            IWriter writer)
        {
            _queryStringCreationService = queryStringCreationService;
            _perfMonitor = perfMonitor;
            _logger = logger;
            _writer = writer;
        }

        public void RunBrokenQuery()
        {
            var brokenQuery = _queryStringCreationService.CreateQueryTextForBrokenQuery();

            _logger.Info("Starting broken query");

            var workingCall = _perfMonitor.Run(
                    () => QueryDTADO(brokenQuery,
                        ConfigurationManager.ConnectionStrings["SQLSERVERADONET35"].ConnectionString),
                    "Run Broken Query");

            var recordCount = workingCall.Rows.Count;
            var loadTime = _perfMonitor.GetTimeTakenToExecute("Run Broken Query");

            _writer.WriteLine($"Load Time Took {loadTime} and loaded {recordCount} records");
        }

        public void RunWorkingQuery()
        {
            var workingQuery = _queryStringCreationService.CreateQueryTextForWorkingQueryString();

            _logger.Info("Starting working query");

            var brokenCall = _perfMonitor.Run(
                () => QueryDTADO(workingQuery,
                    ConfigurationManager.ConnectionStrings["SQLSERVERADONET35"].ConnectionString),
                "Run Working Query");

            var recordCount = brokenCall.Rows.Count;
            var loadTime = _perfMonitor.GetTimeTakenToExecute("Run Working Query");

            _writer.WriteLine($"Load Time Took {loadTime} and loaded {recordCount} records");
        }

        public DataTable QueryDTADO(string QueryString, string connString)
        {
            SqlConnection oConnection = new SqlConnection(connString);
            oConnection.Open();
            SqlCommand oCommand = new SqlCommand(QueryString, oConnection);
            SqlDataAdapter oAdapter = new SqlDataAdapter(oCommand);
            DataTable dt = new DataTable();
            try
            {
                oAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(QueryString + " --> " + ex.ToString());
            }
            oConnection.Close();
            return dt;
        }
    }
}
