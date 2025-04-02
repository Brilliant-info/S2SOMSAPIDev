using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;
using System.Security.Cryptography.X509Certificates;

namespace S2SOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S2SOMSOrderController : ControllerBase
    {
        public readonly IS2SOMSOrders _Injection;
        public readonly IS2SOrderHistory _History;
        public readonly IS2SOrderDriverlist _Driver;
        public readonly IS2SOrderView _OrderView;
       

        public S2SOMSOrderController(IS2SOMSOrders Injection, IS2SOrderHistory history , IS2SOrderDriverlist Driver, IS2SOrderView OrderView) 
        {
            _Injection = Injection;
            _History = history;
            _Driver = Driver;
            _OrderView = OrderView;
        }

        [HttpPost("GetOrderList")]
        public async Task<ActionResult> GetS2SOrders(S2SOrderListReq para)
        {
            var response = await _Injection.GetS2SOrders(para);
            return Ok(response);
        }

        [HttpPost("GetOrderHistory")]

        public async Task<ActionResult> S2SOrderHistory(S2SOrderHistoryReq reqpara)
        {
            var response = await _History.S2SOrderHistory(reqpara);
            return Ok(response);
        }

        [HttpPost("GetOrderview")]
        public async Task<ActionResult> OrderView(S2SOrderViewReq reqpara)
        {
            var response = await _OrderView.OrderView(reqpara);
            return Ok(response);
        }
        [HttpPost("GetDriverlist")]
        public async Task<ActionResult> Driverlist(DriverlistReq reqpara)
        {
            var response = await _Driver.Driverlist(reqpara);
            return Ok(response);
        }

        [HttpPost("S2SAssignDriver")]
        public async Task<ActionResult> S2SAssignDriver(AssignDriverReq reqpara)
        {
            var response = await _Driver.AssignDriver(reqpara);
            return Ok(response);
        }

    }
}
