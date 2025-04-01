using S2SOMSAPI.Model;

namespace S2SOMSAPI.Repository.Interface
{
    public interface IS2SOrderDriverlist
    {
        public  Task<S2SOrderDriverlistResp> Driverlist(DriverlistReq reqpara);
    }
}
