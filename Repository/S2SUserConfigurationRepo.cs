using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;
using System.Data;
using Microsoft.Data.SqlClient;

namespace S2SOMSAPI.Repository
{
    public class S2SUserConfigurationRepo : IS2SUserConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly string _connstr;

        SqlParameter[] Param;
        public S2SUserConfigurationRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connstr = _configuration.GetConnectionString("GWC_ConnectionString")!;
        }

        public async Task<UserConfigListResponse> UserConfigList(S2SUserConfigList UserReqt)
        {
            var UserGridList = new List<UserGridList>();
            var Response = new UserConfigListResponse();
            try
            {
                DataSet ds = new DataSet();
                ds = await GetS2SUserConfigList(UserReqt);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    Response.TotalRecords = int.Parse(ds.Tables[0].Rows[0]["TotalRecord"]?.ToString()!);
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var Userconfig = new UserGridList();
                        {
                            Userconfig.ID = int.Parse(row["ID"]?.ToString()!);
                            Userconfig.Customer = row["CustomerName"]?.ToString() ?? string.Empty;
                            Userconfig.UserName = row["UserName"]?.ToString() ?? string.Empty;
                            Userconfig.UserType = row["UserType"]?.ToString() ?? string.Empty;
                            Userconfig.Active = row["Active"]?.ToString() ?? string.Empty;
                        }
                        UserGridList.Add(Userconfig);
                    }
                    Response.statuscode = 200;
                    Response.status = "success";
                    Response.UserGridList = UserGridList;
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

        public async Task<DataSet> GetS2SUserConfigList(S2SUserConfigList para)
        {
            Param = new SqlParameter[]
            {
                new SqlParameter("@CurrentPage", para .CurrentPage),
                new SqlParameter("@RecordLimit",para.RecordLimit),
                new SqlParameter("@CompanyID",para.CompanyID),
                new SqlParameter("@UserID",para.UserID),
                new SqlParameter("@Search",para.Search),
                new SqlParameter("@Filter",para.Filter)
            };
            return await Return_dataset("S2S_GetUserConfigureList", Param);
        }

        public async Task<S2SUserListResp> GetUserList(UserListReq ParaReq)
        {
            var Response = new S2SUserListResp();
            var UserLst = new List<UserList>();

            DataSet ds = new DataSet();
            Param = new SqlParameter[]
            {
                new SqlParameter("@CompanyID", ParaReq.CompanyID),
                new SqlParameter("@SearchFName",ParaReq.SearchFName)
            };
            ds = await Return_dataset("S2S_UserList", Param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var ObjUsers = new UserList();
                    {
                        ObjUsers.ID = int.Parse(row["ID"]?.ToString()!);
                        ObjUsers.FirstName = row["FirstName"]?.ToString() ?? string.Empty;
                        ObjUsers.LastName = row["LastName"]?.ToString() ?? string.Empty;
                        ObjUsers.UserType = row["UserType"]?.ToString() ?? string.Empty;
                        ObjUsers.UserName = row["UserName"]?.ToString() ?? string.Empty;
                        ObjUsers.EmailID = row["EmailID"]?.ToString() ?? string.Empty;
                    };
                    UserLst.Add(ObjUsers);
                }
                Response.statuscode = 200;
                Response.status = "success";
                Response.UserList = UserLst;
            }
            else
            {
                Response.statuscode = 404;
                Response.status = "Records Not Found.";
            }
           return Response;
        }

        public async Task<SaveUserConfiResp> SaveUserConfig(InsertUserConfigReq SaveReq)
        {
            var saveResponce = new SaveUserConfiResp();
            string Result = "";

            Param = new SqlParameter[]
            {
            new SqlParameter("@ObjectName", SaveReq.ObjectName),
            new SqlParameter("@UserID", SaveReq.UserID),
            new SqlParameter("@CreatedBy", SaveReq.CreatedBy),
            new SqlParameter("@CompanyID", SaveReq.CompanyID)         
            };
            Result = await Return_SaveScalerValues("S2S_InsertS2SUserConfigure", Param);
            if(Result == "Success")
            {
                saveResponce.statuscode = 200;
                saveResponce.status = "Saved Successfully.";
            }
            else
            {
                saveResponce.statuscode = 201;
                saveResponce.status = "Error Occured.";
            }

            return saveResponce;
        }

        public async Task<SaveUserConfiResp> RemoveConfigUser(RemoveConfigReq RemReq)
        {
            var RemoveResponce = new SaveUserConfiResp();
            string Result = "";

            Param = new SqlParameter[]
            {
            new SqlParameter("@ObjectName", RemReq.ObjectName),
            new SqlParameter("@ID", RemReq.ID)
            };
            Result = await Return_SaveScalerValues("S2S_RemoveConfigUser", Param);
            if (Result == "Success")
            {
                RemoveResponce.statuscode = 200;
                RemoveResponce.status = "Removed Successfully.";
            }
            else
            {
                RemoveResponce.statuscode = 201;
                RemoveResponce.status = "Error Occured.";
            }

            return RemoveResponce;
        }


        public async Task<DataSet> Return_dataset(string procname, params SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(_connstr);
            DataSet ds = new DataSet();
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
                    await conn.OpenAsync();
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
                    await conn.CloseAsync();
                }
                ds.Dispose();
            }
            return ds;
        }

        public async Task<string> Return_SaveScalerValues(string ProcName, params SqlParameter[] Param)
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
    }
   
}
