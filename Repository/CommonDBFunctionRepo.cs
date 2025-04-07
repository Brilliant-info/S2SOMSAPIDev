using System.Data;
using Microsoft.Data.SqlClient;

namespace S2SOMSAPI.Repository
{
    public class CommonDBFunctionRepo
    {
        private readonly string _connstr;

        public CommonDBFunctionRepo(string connectionString)
        {
            _connstr = connectionString;
        }

        public async Task<DataSet> ReturnDatasetAsync(string procname, params SqlParameter[] param)
        {
            var ds = new DataSet();

            // Use 'using' to ensure that the connection and adapter are disposed properly
            using (var conn = new SqlConnection(_connstr))
            using (var da = new SqlDataAdapter(procname, conn))
            {
                try
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters in bulk if any exist
                    if (param != null && param.Length > 0)
                    {
                        da.SelectCommand.Parameters.AddRange(param);
                    }

                    // Open the connection if it is not already open
                    if (conn.State == ConnectionState.Closed)
                    {
                        await conn.OpenAsync();
                    }

                    // Fill the DataSet with the result
                    await Task.Run(() => da.Fill(ds));
                }
                catch (Exception ex)
                {
                    // Log the exception for troubleshooting
                    // For example, you could use ILogger here
                    throw new ApplicationException($"Error executing stored procedure: {procname}", ex);
                }
            }

            return ds;
        }

        public async Task<string> ReturnScalerValuesAsync(string procName, params SqlParameter[] param)
        {
            try
            {
                using (var conn = new SqlConnection(_connstr))
                using (var cmd = new SqlCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters in bulk if any exist
                    if (param != null && param.Length > 0)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    if (conn.State == ConnectionState.Closed)
                    {
                        await conn.OpenAsync(); // Opens the connection asynchronously if it is closed
                    }

                    var returnval = await cmd.ExecuteScalarAsync();
                    return returnval?.ToString() ?? "Fail"; // Return the value or "Fail" if null
                }
            }
            catch (Exception ex)
            {
                // Log the exception here (using ILogger or any logging mechanism you prefer)
                // For now, just return the message
                return $"Error: {ex.Message}";  // Optionally return more details if needed for debugging
            }
        }
        
    }
}
