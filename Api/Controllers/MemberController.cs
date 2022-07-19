using Integration.Application.Contracts.SecondHandCar;
using Integration.Service.StartUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// 会员
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberController : ControllerBase
    {
        private readonly IMember _memberservice;

        public MemberController(IMember memberservice)
        {
            _memberservice = memberservice;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(PermissionConstantKey.ExaminePolicy)]
        public  async Task<string> Login()
        {
            await _memberservice.GetAsync();
            return "1";
        }
    }
}
