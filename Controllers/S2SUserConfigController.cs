using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;

namespace S2SOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S2SUserConfigController : ControllerBase
    {
        public readonly IS2SUserConfiguration _UserConfig;
        public S2SUserConfigController(IS2SUserConfiguration userConfig)
        {
            _UserConfig = userConfig;
        }

        [HttpPost("GetS2SUserConfigList")]
        public async Task<ActionResult> UserConfigList(S2SUserConfigList ConfiglstReq)
        {
            var responce = await _UserConfig.UserConfigList(ConfiglstReq);
            return Ok(responce);
        }

        [HttpPost("GetS2SUserList")]
        public async Task<ActionResult> GetUserList(UserListReq UserReq)
        {
            var responce = await _UserConfig.GetUserList(UserReq);
            return Ok(responce);
        }

        [HttpPost("SaveS2SUserConfig")]
        public async Task<ActionResult> SaveS2SUserConfig(InsertUserConfigReq SaveReq)
        {
            var Responce = await _UserConfig.SaveUserConfig(SaveReq);
            return Ok(Responce);
        }

        [HttpPost("RemovedS2SUserConfig")]
        public async Task<ActionResult> RemoveConfigUser(RemoveConfigReq Rempara)
        {
            var Responce = await _UserConfig.RemoveConfigUser(Rempara);
            return Ok(Responce);
        }

    }
}
