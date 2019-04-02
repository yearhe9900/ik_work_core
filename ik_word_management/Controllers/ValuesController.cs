using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ik_word_management.Models.Domain;
using ik_word_management.Models.DTO.Input;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ik_word_management.Controllers
{
    [Route("api/Values/"), EnableCors("AllowSameDomain")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet, HttpPost]
        public string Get()
        {
            HttpContext.Response.Headers.Add("Last-Modified", "12");
            HttpContext.Response.Headers.Add("ETag", "ETag");
            string result = "格兰富\n兰富\n格兰\n格\n兰\n富\n";
            return result;
        }
    }
}
