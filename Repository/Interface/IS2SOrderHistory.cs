using S2SOMSAPI.Model;

namespace S2SOMSAPI.Repository.Interface
{
    public interface IS2SOrderHistory
    {
        public Task<S2SOrderHistoryResp> S2SOrderHistory(S2SOrderHistoryReq reqpara);
    }
}
