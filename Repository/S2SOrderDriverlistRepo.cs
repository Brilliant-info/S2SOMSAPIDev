using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;
using System.Data;
using System.Data.SqlClient;
using S2SOrderDriverlist = S2SOMSAPI.Model.S2SOrderDriverlist;

namespace S2SOMSAPI.Repository
{
    public class S2SOrderDriverlistRepo : IS2SOrderDriverlist
    {
        private readonly IConfiguration _configuration;
        private readonly string _connstr;
        public DriverlistReq reqpara;
        DataSet ds = new DataSet();

        SqlParameter[] Param;

        public S2SOrderDriverlistRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connstr = configuration.GetConnectionString("GWC_ConnectionString");
        }
        public async Task<S2SOrderDriverlistResp> Driverlist(DriverlistReq reqpara)
        {
            var Response = new S2SOrderDriverlistResp();
            var Driverlist = new List<S2SOrderDriverlist>();
            try
            {

                ds = await GetDriverlist(reqpara);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var DriverName = row["DriverName"].ToString();
                        var ContactNo = row["ContactNo"].ToString();
                        var EmailID = row["EmailID"].ToString();

                        var S2SOrderDriverlist = new S2SOrderDriverlist
                        {
                            DriverName = DriverName,
                            ContactNo = ContactNo,
                            EmailID = EmailID
                        };

                        Driverlist.Add(S2SOrderDriverlist);
                    }

                    int statuscode = 200;
                    string status = "success";
                    Response.statuscode = statuscode;
                    Response.status = status;
                    Response.S2SOrderDriverlist = Driverlist;
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

        public async Task<DataSet> GetDriverlist(DriverlistReq reqpara)
        {
            Param = new SqlParameter[]
            {
                new SqlParameter("CompanyID",reqpara.CompanyID),
                new SqlParameter("UserID",reqpara.UserID)
            };
            return Return_dataset("GetDriverlist",Param);
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
