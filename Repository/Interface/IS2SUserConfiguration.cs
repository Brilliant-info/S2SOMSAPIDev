using S2SOMSAPI.Model;

namespace S2SOMSAPI.Repository.Interface
{
    public interface IS2SUserConfiguration
    {
        public Task<UserConfigListResponse> UserConfigList(S2SUserConfigList UserReqt);

        public Task<S2SUserListResp> GetUserList(UserListReq ParaReq);

        public Task<SaveUserConfiResp> SaveUserConfig(InsertUserConfigReq SaveReq);

        public Task<SaveUserConfiResp> RemoveConfigUser(RemoveConfigReq reqpara);
    }
}
