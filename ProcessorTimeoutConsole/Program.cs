using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using static System.Console;

namespace ProcessorTimeoutConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                QueryService querySvc = new QueryService();

                var workingQuery = querySvc.CreateQueryTextForWorkingQueryString();
                WriteLine("Starting first, working call");
                var timerWorkingQuery = Stopwatch.StartNew();
                var workingCall = QueryDTADO(workingQuery, ConfigurationManager.ConnectionStrings["SQLSERVERADONET35"].ConnectionString);
                timerWorkingQuery.Stop();
                WriteLine($"{workingCall.Rows.Count} Records returned from working call and took {timerWorkingQuery.Elapsed}");

                WriteLine();

                var brokenQuery = querySvc.CreateQueryTextForBrokenQuery();
                WriteLine("Starting second, broken call");
                var timerBrokenQuery = Stopwatch.StartNew();
                var brokenCall = QueryDTADO(brokenQuery, ConfigurationManager.ConnectionStrings["SQLSERVERADONET35"].ConnectionString);
                timerBrokenQuery.Stop();
                WriteLine($"{brokenCall.Rows.Count} Records returned from working call and took {timerBrokenQuery.Elapsed}");

            }
            catch (Exception ex)
            {
                WriteLine($"Exception caught ---> {ex.InnerException}");
            }

            ReadKey();
        }

        public static DataTable QueryDTADO(string QueryString, string connString)
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
