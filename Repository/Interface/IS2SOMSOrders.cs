using S2SOMSAPI.Model;

namespace S2SOMSAPI.Repository.Interface
{
    public interface IS2SOMSOrders
    {
        public Task<S2SOrderListRespn> GetS2SOrders(S2SOrderListReq para);

    }
}