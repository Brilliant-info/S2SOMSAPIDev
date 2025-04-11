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
                    string CreationDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreationDate"]).Date.ToString("yyyy-MM-dd");
                    //string CreationDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreationDate"]).Date.ToString("yyyy-MM-dd");


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

        public async Task<CancelOrderResp> CancelS2SOrder(CancelOrderReq CancReq)
        {
            var CancelResponce = new CancelOrderResp();
            string Result = "";

            Param = new SqlParameter[]
            {
            new SqlParameter("@ID", CancReq.OrderID),
            new SqlParameter("@UserID", CancReq.UserID)
            };
            Result = await _commonDBFunctionRepo.ReturnScalerValuesAsync("S2S_CancelS2SOrder", Param);
            if (Result == "Success")
            {
                CancelResponce.statuscode = 200;
                CancelResponce.status = "Removed Successfully.";
            }
            else
            {
                CancelResponce.statuscode = 201;
                CancelResponce.status = "Error Occured.";
            }

            return CancelResponce;
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

        public async Task<S2SOrderReportResp> S2SOrderReportRepo(S2SOrderReportReq RepoReq)
        {
            var orderlist = new List<S2SOrderReportlist>();
            var Response = new S2SOrderReportResp();
            try
            {
                DataSet ds = new DataSet();
                ds = await GetS2SorderRepoList(RepoReq);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var S2Sorderrepo = new S2SOrderReportlist();
                        {
                            S2Sorderrepo.OMSOrderNo = row["OMSOrderNo"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.WinCashOrderNumber = row["WinCashOrderNumber"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.Status = row["Status"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.SourceStoreName = row["SourceStoreName"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.DestinationStoreName = row["DestinationStoreName"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.Performedby = row["Performedby"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.Receivedby = row["Receivedby"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.SKU = row["SKU"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.Description = row["Description"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.Serialnumber = row["Serialnumber"]?.ToString() ?? string.Empty;
                            S2Sorderrepo.Quantity = int.Parse(row["Quantity"]?.ToString() ?? "0");
                        }
                        orderlist.Add(S2Sorderrepo);
                    }
                    Response.statuscode = 200;
                    Response.status = "success";
                    Response.S2SOrderReportlist = orderlist;
                }
                else
                {
                    Response.statuscode = 404;
                    Response.status = "Records Not Found.";
                }
            }
            catch (Exception ex) 
            {
                Response.statuscode = 601;
                Response.status = "Something went wrong.";
            }
            return Response;
        }

        public async Task<DataSet> GetS2SorderRepoList(S2SOrderReportReq para)
        {
            Param = new SqlParameter[]
            {
                new SqlParameter("@OrderNo", para.OrderNo),
                new SqlParameter("@fromDate",para.fromDate),
                new SqlParameter("@toDate",para.toDate),
                new SqlParameter("@Status",para.Status)
            };
            return await _commonDBFunctionRepo.ReturnDatasetAsync("S2S_S2SOrderReport", Param);
        }
    }
}