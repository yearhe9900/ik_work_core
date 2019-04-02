using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ik_word_management.Controllers
{
    [Route("api/Auth/")]
    public class AuthController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Login()
        {
            return new OkObjectResult("haha");
        }
    }
}