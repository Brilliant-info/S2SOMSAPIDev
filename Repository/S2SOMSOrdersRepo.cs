using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace S2SOMSAPI.Repository
{
    public class S2SOMSOrdersRepo : IS2SOMSOrders
    {
        private readonly IConfiguration _configuration;
        private readonly string _connstr;
        public S2SOrderListReq para;
        SqlParameter[] Param;
        public S2SOMSOrdersRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connstr = _configuration.GetConnectionString("GWC_ConnectionString");
        }

        DataSet ds = new DataSet();
        string S2SOrderNo = "", WincashOrderNo = "", Ordertype = "", Sourcestore = "", DestinationStore = "", Performedby = "", Receivedby = "";
        public async Task<S2SOrderListRespn> GetS2SOrders(S2SOrderListReq para)
        {
            var S2SOrdersList = new List<S2SOrders>();
            var Response = new S2SOrderListRespn();
            
            try
            {
                ds = await GetOrders(para);
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var S2SOrderNo = row["S2SOrderNo"].ToString();
                        var WincashOrderNo = row["WincashOrderNo"].ToString();
                        var Ordertype = row["Ordertype"].ToString();
                        var Sourcestore = row["Sourcestore"].ToString();
                        var DestinationStore = row["DestinationStore"].ToString();
                        var Performedby = row["Performedby"].ToString();
                        var Receivedby = row["Receivedby"].ToString();

                        var S2SOrders = new S2SOrders
                        {
                            S2SOrderNo = S2SOrderNo,
                            WincashOrderNo = WincashOrderNo,
                            Ordertype = Ordertype,
                            Sourcestore = Sourcestore,
                            DestinationStore = DestinationStore,
                            Performedby = Performedby,
                            Receivedby = Receivedby
                        };

                        S2SOrdersList.Add(S2SOrders);
                    }
                    int statuscode = 200;
                    string status = "success";
                    Response.statuscode = statuscode;
                    Response.status = status;
                    Response.S2SOrders = S2SOrdersList;
                }
            }
            catch (Exception ex)
            {
                
            }
            finally 
            { 
            }

            return Response;
        }

        public async Task<DataSet> GetOrders(S2SOrderListReq para)
        {
            Param = new SqlParameter[]
            {
                new SqlParameter("@CurrentPage", para .CurrentPage),
                new SqlParameter("@RecordLimit",para.RecordLimit),
                new SqlParameter("@UserId",para.UserId),
                new SqlParameter("@Search",para.Search),
                new SqlParameter("@Filter",para.Filter)
            };
            return Return_dataset("GetS2SOrderList", Param);

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
