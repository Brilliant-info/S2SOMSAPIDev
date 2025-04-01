using S2SOMSAPI.Model;

namespace S2SOMSAPI.Repository.Interface
{
    public interface IS2SOrderView
    {
        public Task<S2SOrderViewResp> OrderView(S2SOrderViewReq req);
    }
}
