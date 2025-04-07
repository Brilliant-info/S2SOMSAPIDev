using S2SOMSAPI.Model;
using System.Data;
using S2SOMSAPI.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace S2SOMSAPI.Repository
{
    public class S2SOrderHistoryRepo : IS2SOrderHistory
    {
        private readonly CommonDBFunctionRepo _commonDBFunctionRepo;
        SqlParameter[] Param = new SqlParameter[0];
        public S2SOrderHistoryRepo(CommonDBFunctionRepo commonDBFunctionRepo)
        {
            _commonDBFunctionRepo = commonDBFunctionRepo;
        }

        public async Task<S2SOrderHistoryResp> S2SOrderHistory(S2SOrderHistoryReq reqpara)
        {
            var Response = new S2SOrderHistoryResp();
            var OrderhistoryList = new List<S2SOrderHistories>();
            DataSet ds = new DataSet();
            try
            {                
                ds = await FetchHistory(reqpara);
                if (ds != null && ds.Tables[0].Rows.Count > 0) 
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var S2SOrderNo = row["S2SOrderNo"]?.ToString() ?? string.Empty;
                        var Status = row["status"]?.ToString() ?? string.Empty;
                        DateTime Date = Convert.ToDateTime(row["Date"]);
                        var Updatedby = row["UserId"]?.ToString() ?? string.Empty;

                        var S2SOrderHistories = new S2SOrderHistories
                        {
                            S2SOrderNo = S2SOrderNo,
                            Status = Status,
                            Date = Date,
                            Updatedby = Updatedby
                        };
                        OrderhistoryList.Add(S2SOrderHistories);
                    }
                    Response.statuscode = 200;
                    Response.status = "success";
                    Response.S2SOrderHistories = OrderhistoryList;
                }
                else
                {
                    Response.statuscode = 404;
                    Response.status = "Data Not Found.";
                }
            }
            catch 
            {
                Response.statuscode = 601;
                Response.status = "Someting went wrong.";
            }
            finally
            {
                ds.Dispose();
            }
            return Response;
        }

        public async Task<DataSet> FetchHistory(S2SOrderHistoryReq reqpara) 
        {
            Param = new SqlParameter[]
            {
                new SqlParameter("S2SOrderNO",reqpara.S2SOrderNO)
            };
            return await _commonDBFunctionRepo.ReturnDatasetAsync("S2SOrderHistory", Param);
        }
    }
    
}
