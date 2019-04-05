using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ik_word_management.Models.DTO.Input;
using ik_word_management.Models.DTO.Output;
using ik_word_management.Models.Enum;
using ik_word_management.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ik_word_management.Controllers
{
    [Produces("application/json")]
    [Route("api/Words/")]
    [Authorize]
    public class WordsController : Controller
    {
        private IWordService _wordService = null;

        public WordsController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [HttpPost("[action]")]
        public IActionResult AddWord([FromBody]RequestWordInputModel model)
        {
            var result = _wordService.AddOneWord(model.Name, model.GroupID);

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Msg = result > 0 ? "添加成功" : "添加失败"
            });
        }
    }
}