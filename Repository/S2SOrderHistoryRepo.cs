using S2SOMSAPI.Model;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using S2SOMSAPI.Repository.Interface;

namespace S2SOMSAPI.Repository
{
    public class S2SOrderHistoryRepo : IS2SOrderHistory
    {
        private readonly IConfiguration _configuration;
        private readonly string _connstr;
        public S2SOrderHistoryReq reqpara;
        DataSet ds = new DataSet();

        SqlParameter[] Param;
        
        public S2SOrderHistoryRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connstr = configuration.GetConnectionString("GWC_ConnectionString");
        }

        public async Task<S2SOrderHistoryResp> S2SOrderHistory(S2SOrderHistoryReq reqpara)
        {
            var Response = new S2SOrderHistoryResp();
            var OrderhistoryList = new List<S2SOrderHistories>();
            try
            {
                
                ds = await FetchHistory(reqpara);
                if (ds != null && ds.Tables[0].Rows.Count > 0) 
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var S2SOrderNo = row["S2SOrderNo"].ToString();
                        var Status = row["status"].ToString();
                        DateTime Date = Convert.ToDateTime(row["Date"]);
                        var Updatedby = row["UserId"].ToString();

                        var S2SOrderHistories = new S2SOrderHistories
                        {
                            S2SOrderNo = S2SOrderNo,
                            Status = Status,
                            Date = Date,
                            Updatedby = Updatedby
                        };

                        OrderhistoryList.Add(S2SOrderHistories);
                    }

                    int statuscode = 200;
                    string status = "success";
                    Response.statuscode = statuscode;
                    Response.status = status;
                    Response.S2SOrderHistories = OrderhistoryList;
                }
            }
            catch 
            { 

            }
            finally
            {

            }
            return Response;
        }

        public async Task<DataSet> FetchHistory(S2SOrderHistoryReq reqpara) 
        {
            Param = new SqlParameter[]
            {
                new SqlParameter("S2SOrderNO",reqpara.S2SOrderNO)
            };
            return Return_dataset("S2SOrderHistory", Param);
        }

        public DataSet Return_dataset(string procname, params SqlParameter[] param)
        {
            
            using SqlConnection conn = new SqlConnection(_connstr);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(procname, conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (param != null)
                {
                    foreach (SqlParameter p in param)
                    {
                        da.SelectCommand.Parameters.Add(p);
                    }
                }
                //conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return ds;
        }
    }
    
}
