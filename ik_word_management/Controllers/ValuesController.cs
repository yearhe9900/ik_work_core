using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ik_word_management.Models.Domain;
using ik_word_management.Models.DTO.Input;
using ik_word_management.Services.IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ik_word_management.Controllers
{
    [Route("api/Values/"), EnableCors("AllowSameDomain")]
    public class ValuesController : Controller
    {
        private IWordService _wordService = null;
        private IModifiedService _modifiedService = null;

        public ValuesController(IWordService wordService, IModifiedService modifiedService)
        {
            _modifiedService = modifiedService;
            _wordService = wordService;
        }

        // GET api/values
        [HttpGet,HttpHead]
        public string Get()
        {
            var modified = _modifiedService.GetLastModified();

            var words = _wordService.GetAllEnableWordsName();

            HttpContext.Response.Headers.Add("Last-Modified", modified == null ? "null" : modified.Cdt.Value.ToString());
            HttpContext.Response.Headers.Add("ETag", modified == null ? "null" : modified.Cdt.Value.ToString());
            string result = "";
            foreach (var item in words)
            {
                result = item + "\n"+ result;
            }
            return result;
        }
    }
}
