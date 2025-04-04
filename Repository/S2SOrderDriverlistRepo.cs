using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;
using System.Data;
using Microsoft.Data.SqlClient;
using S2SOrderDriverlist = S2SOMSAPI.Model.S2SOrderDriverlist;
using Microsoft.AspNetCore.Http;
using Azure;

namespace S2SOMSAPI.Repository
{
    public class S2SOrderDriverlistRepo : IS2SOrderDriverlist
    {
        private readonly IConfiguration _configuration;
        private readonly string _connstr;
        public DriverlistReq reqpara;
        public AssignDriverReq AssignReq;
        //public AssignDriverlistReq reqpara;

        DataSet ds = new DataSet();

        SqlParameter[] Param;

        public S2SOrderDriverlistRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connstr = configuration.GetConnectionString("GWC_ConnectionString")!;
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
                        var driverID = row["driverID"]?.ToString() ?? "";
                        var DriverName = row["DriverName"]?.ToString() ?? "";
                        var ContactNo = row["ContactNo"]?.ToString() ?? "";
                        var EmailID = row["EmailID"]?.ToString() ?? "";

                        var S2SOrderDriverlist = new S2SOrderDriverlist
                        {
                            driverID = driverID,
                            DriverName = DriverName,
                            ContactNo = ContactNo,
                            EmailID = EmailID
                        };

                        Driverlist.Add(S2SOrderDriverlist);
                    }
                    Response.statuscode = 200;
                    Response.status = "success";
                    Response.S2SOrderDriverlist = Driverlist;
                }
                else
                {
                    Response.statuscode = 404;
                    Response.status = "Driver Not found";
                }
            }
            catch
            {
                Response.statuscode = 505;
                Response.status = "Something Went Wrong";
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
            SqlConnection conn = new SqlConnection(_connstr);
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


        #region New Code for Driver Assign
        public async Task<SaveReqResponce> AssignDriver(AssignDriverReq AssignReq)
        {
            var Response = new SaveReqResponce();
            try
            {
                string result = "";
                Param = new SqlParameter[]
                    {
            new SqlParameter("@ObjectName", AssignReq.ObjectName),
            new SqlParameter("@ReferenceID", AssignReq.ReferenceID),
            new SqlParameter("@DriverId", AssignReq.DriverId),
            new SqlParameter("@AssignBy", AssignReq.AssignBy),
            new SqlParameter("@VehicleDetail", AssignReq.VehicleDetail),
                };
                result = await Return_ScalerValues("S2S_AssignDriver", Param);

                if (result == "Success")
                {
                    Response.statuscode = 200;
                    Response.status = "Success";
                }
                else if (result == "Order Already assigned")
                {
                    Response.statuscode = 601;
                    Response.status = "Order Already assigned";
                }
                else if (result == "Status Not Valid")
                {
                    Response.statuscode = 602;
                    Response.status = "Can Not Assign Driver for Pick and Drop Status";
                }
                else
                {
                    Response.statuscode = 603;
                    Response.status = "Something went wrong";
                }
            }
            catch (Exception ex)
            {
                Response.statuscode = 603;
                Response.status = "Something went wrong";
            }
            return Response;
        }

        public async Task<SaveReqResponce> RemoveAssignDriver(RemoveDriverAssignReq RemoveReq)
        {
            var Response = new SaveReqResponce();
            try
            {
                string result = "";
                Param = new SqlParameter[]
                    {
            new SqlParameter("@ObjectName", RemoveReq.ObjectName),
            new SqlParameter("@ReferenceID", RemoveReq.ReferenceID),
            new SqlParameter("@DriverId", RemoveReq.DriverId),
                };
                result = await Return_ScalerValues("S2S_RemoveAssignedDriver", Param);

                if (result == "Success")
                {
                    Response.statuscode = 200;
                    Response.status = "Success";
                }
                else if (result == "Order Not Assigned")
                {
                    Response.statuscode = 601;
                    Response.status = "Order Not Assigned to Driver";
                }
                else if (result == "Can Not Remove")
                {
                    Response.statuscode = 602;
                    Response.status = "Selected Order Not Available";
                }
                else
                {
                    Response.statuscode = 603;
                    Response.status = "Error";
                }
            }
            catch (Exception ex)
            {
                Response.statuscode = 603;
                Response.status = "Error";
            }
            return Response;
        }

        public async Task<string> Return_ScalerValues(string ProcName, params SqlParameter[] Param)
        {
            string result = "";
            using (SqlConnection conn = new SqlConnection(_connstr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(ProcName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (Param != null)
                    {
                        foreach (SqlParameter p in Param)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }
                    if (conn.State == ConnectionState.Closed)
                    {
                        await conn.OpenAsync();
                    }
                    var returnval = cmd.ExecuteScalar();
                    if (returnval != null)
                    {
                        result = returnval.ToString();
                    }
                    else
                    {
                        result = "Fail";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        await conn.CloseAsync();
                    }
                }
            }
            return result;
        }


        #endregion

        public async Task<S2SAssignDriverlistResp> AssignDriverlist(AssignDriverlistReq reqpara)
        {
            var Response = new S2SAssignDriverlistResp();
            var ADriverlist = new List<S2SDriverlist>();
            try
            {
                ds = await GetAssignDriverlist(reqpara);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var Id = row["Id"]?.ToString() ?? "";
                        var AssignDate = row["AssignDate"]?.ToString() ?? ""; 
                        var DriverId = row["DriverId"]?.ToString() ?? "";
                        var DriverName = row["DriverName"]?.ToString() ?? "";
                        var TrackDetails = row["TrackDetails"]?.ToString() ?? "";
                        var ReferenceId = row["ReferenceId"]?.ToString() ?? "";

                        var DriverList = new S2SDriverlist
                        {
                            Id = Id,
                            AssignDate = AssignDate,
                            DriverId = DriverId,
                            DriverName = DriverName,
                            TrackDetails = TrackDetails,
                            ReferenceId = ReferenceId
                        };

                        ADriverlist.Add(DriverList);
                    }
                    Response.statuscode = 200;
                    Response.status = "success";
                    Response.S2SDriverlist = ADriverlist;
                }
                else
                {
                    Response.statuscode = 404;
                    Response.status = "Driver Not found";
                }
            }
            catch
            {
                Response.statuscode = 505;
                Response.status = "Something Went Wrong";
            }
            finally
            {

            }
            return Response;
        }

        public async Task<DataSet> GetAssignDriverlist(AssignDriverlistReq reqpara)
        {
            Param = new SqlParameter[]
            {
                new SqlParameter("ReferenceID",reqpara.ReferenceID),
                new SqlParameter("objectName",reqpara.objectName)
            };
            return Return_dataset("AssignDriverlist", Param);
        }

    }


}
