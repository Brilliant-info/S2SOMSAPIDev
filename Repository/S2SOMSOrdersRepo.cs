using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;
using System.Data;
using Microsoft.Data.SqlClient;


namespace S2SOMSAPI.Repository
{
    public class S2SOMSOrdersRepo : IS2SOMSOrders
    {
        // Old Code
        /*private readonly IConfiguration _configuration;
        private readonly string _connstr;

        public S2SOrderListReq para;
        SqlParameter[] Param;

        public S2SOMSOrdersRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connstr = _configuration.GetConnectionString("GWC_ConnectionString")!;
        }*/

        // New Code
        private readonly CommonDBFunctionRepo _commonDBFunctionRepo;

        SqlParameter[] Param = new SqlParameter[0];
        public S2SOMSOrdersRepo(CommonDBFunctionRepo commonDBFunctionRepo)
        {
            _commonDBFunctionRepo = commonDBFunctionRepo;
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
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    Response.TotalRecords = int.Parse(ds.Tables[0].Rows[0]["TotalRecord"]?.ToString()!);
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var Id = row["Id"]?.ToString() ?? "";
                        var S2SOrderNo = row["S2SOrderNo"]?.ToString() ?? "";
                        var WincashOrderNo = row["WincashOrderNo"]?.ToString() ?? "";
                        var Ordertype = row["Ordertype"]?.ToString() ?? "";
                        var Sourcestore = row["Sourcestore"]?.ToString() ?? "";
                        var DestinationStore = row["DestinationStore"]?.ToString() ?? "";
                        var Performedby = row["Performedby"]?.ToString() ?? "";
                        var Receivedby = row["Receivedby"]?.ToString() ?? "";
                        var Status = row["Status"]?.ToString() ?? "";

                        var S2SOrders = new S2SOrders
                        {
                            Id = Id,
                            S2SOrderNo = S2SOrderNo,
                            WincashOrderNo = WincashOrderNo,
                            Ordertype = Ordertype,
                            Sourcestore = Sourcestore,
                            DestinationStore = DestinationStore,
                            Performedby = Performedby,
                            Receivedby = Receivedby,
                            Status = Status
                        };

                        S2SOrdersList.Add(S2SOrders);
                    }
                    Response.statuscode = 200;
                    Response.status = "success";
                    Response.S2SOrders = S2SOrdersList;
                }
                else
                {
                    Response.statuscode = 404;
                    Response.status = "Order Not Found";
                }
            }
            catch (Exception ex)
            {
                Response.statuscode = 505;
                Response.status = "Someting Went Wrong";
            }
            finally 
            { 
                ds.Dispose();
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
            return await _commonDBFunctionRepo.ReturnDatasetAsync("GetS2SOrderList", Param);

        }

        public async Task<S2SOrderDocumentListRespn> S2Sdocumentlist(S2SdocumentlistReq para)
        {
            var Getdocumentlist = new List<documentlist>();
            var Response = new S2SOrderDocumentListRespn();
            try
            {
                ds = await Getdocument(para);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var Id = row["Id"]?.ToString() ?? "";
                        var DocumentName = row["DocumentName"]?.ToString() ?? "";
                        var Description = row["Description"]?.ToString() ?? "";
                        var DocumentType = row["DocumentType"]?.ToString() ?? "";
                        var FileType = row["FileType"]?.ToString() ?? "";
                        var AttachedFile = row["AttachedFile"]?.ToString() ?? "";
                        var ReferenceID = row["ReferenceID"]?.ToString() ?? "";
                        //var Receivedby = row["Receivedby"]?.ToString() ?? "";

                        var documentlist = new documentlist
                        {
                            Id = Id,
                            DocumentName = DocumentName,
                            Description = Description,
                            Ordertype = Ordertype,
                            DocumentType = DocumentType,
                            FileType = FileType,
                            AttachedFile = AttachedFile,
                            ReferenceID = ReferenceID
                        };

                        Getdocumentlist.Add(documentlist);
                    }
                    Response.statuscode = 200;
                    Response.status = "success";
                    Response.documentlist = Getdocumentlist;
                }
                else
                {
                    Response.statuscode = 404;
                    Response.status = "Order Not Found";
                }
            }
            catch (Exception ex)
            {
                Response.statuscode = 505;
                Response.status = "Someting Went Wrong";
            }
            finally
            {
                ds.Dispose();
            }
            return Response;
        }

        public async Task<DataSet> Getdocument(S2SdocumentlistReq para)
        {
            Param = new SqlParameter[]
            {
                new SqlParameter("@ReferenceID", para.ReferenceID),
                new SqlParameter("@CompanyID",para.CompanyID),
                new SqlParameter("@ObjectName",para.ObjectName)
            };
            return await _commonDBFunctionRepo.ReturnDatasetAsync("GetDocumentlist", Param);

        }

       
    }
}
