using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace S2SOMSAPI.Repository
{
    public class S2SOrderViewRepo : IS2SOrderView
    {
        private readonly CommonDBFunctionRepo _commonDBFunctionRepo;
        SqlParameter[] Param = new SqlParameter[0];
        public S2SOrderViewRepo(CommonDBFunctionRepo commonDBFunctionRepo)
        {
            _commonDBFunctionRepo = commonDBFunctionRepo;
        }
        
        public async Task<S2SOrderViewResp> OrderView(S2SOrderViewReq req)
        {
            var response = new S2SOrderViewResp();
            DataSet ds = new DataSet();
            var ProductList = new List<Skulist>();
            try
            {
                ds = await fetchOrder(req);
                if (ds != null & ds.Tables[0].Rows.Count > 0)
                {
                    string WincashOrderNumber = ds.Tables[0].Rows[0]["WincashOrderNo"]?.ToString() ?? string.Empty;
                    string S2SOrderNo = ds.Tables[0].Rows[0]["S2SOrderNo"]?.ToString() ?? string.Empty;
                    string Status = ds.Tables[0].Rows[0]["Status"]?.ToString() ?? string.Empty;
                    string Sourcestore = ds.Tables[0].Rows[0]["Sourcestore"]?.ToString() ?? string.Empty;
                    string DestinationStore = ds.Tables[0].Rows[0]["DestinationStore"]?.ToString() ?? string.Empty;
                    string Performedby = ds.Tables[0].Rows[0]["Performedby"]?.ToString() ?? string.Empty;
                    string Receivedby = ds.Tables[0].Rows[0]["Receivedby"]?.ToString() ?? string.Empty;
                    DateTime CreationDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreationDate"]);

                    // Assign fetched values to the response object
                    response.WincashOrderNo = WincashOrderNumber;
                    response.S2SOrderNo = S2SOrderNo;
                    response.Status = Status;
                    response.Sourcestore = Sourcestore;
                    response.DestinationStore = DestinationStore;
                    response.Performedby = Performedby;
                    response.Receivedby = Receivedby;
                    response.CreationDate = CreationDate;

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        string SKU = row["SKU"]?.ToString() ?? string.Empty;                       
                        string serialnumber = row["serialnumber"]?.ToString() ?? string.Empty;
                        Decimal Quantity = Convert.ToDecimal(row["quantity"]);

                        var ListSku = new Skulist
                        {
                            SKU = SKU,                            
                            serialnumber = serialnumber,
                            Quantity = Quantity
                        };
                        ProductList.Add(ListSku);
                    }
                    response.statuscode = 200;
                    response.desc = "success";
                    response.Skulist = ProductList;
                }
                else
                {
                    response.statuscode = 404;
                    response.desc = "Data Not Found.";
                }
            }
            catch (Exception ex)
            {
                response.statuscode = 601;
                response.desc = "Something went wrong.";
            }
            finally
            {
                ds.Dispose();
            }
            return response;
        }

        public async Task<DataSet> fetchOrder(S2SOrderViewReq req)
        {
            var ds = new DataSet();
            Param = new SqlParameter[]
            {
                new SqlParameter("OmsorderNo",req.OmsorderNo),
            };
            return await _commonDBFunctionRepo.ReturnDatasetAsync("GetOrderView", Param);
        }
    }
}