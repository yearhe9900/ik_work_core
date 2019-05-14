using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ik_word_management.Models.DTO.Output;
using ik_word_management.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ik_word_management.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    [Authorize]
    public class UserController : Controller
    {
        [HttpPost("[action]")]
        public IActionResult Info()
        {
            var result = 1;
            string[] roles = { "admin" };
            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Message = result > 0 ? "查询用户成功" : "查询用户失败",
                Content = new
                {
                    introduction = "I am a super administrator",
                    avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                    name = "Super Admin",
                    roles
                }
            });
        }
    }
}