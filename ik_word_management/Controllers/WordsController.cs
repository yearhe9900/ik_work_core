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
        private IGroupService _groupService = null;
        private IModifiedService _modifiedService = null;

        public WordsController(IWordService wordService, IGroupService groupService, IModifiedService modifiedService)
        {
            _wordService = wordService;
            _groupService = groupService;
            _modifiedService = modifiedService;
        }

        [HttpPost("[action]")]
        public IActionResult AddOneWord([FromBody]RequestWordInputModel model)
        {
            var result = _wordService.AddOneWord(model);

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Message = result > 0 ? "添加成功" : "添加失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult GetWords([FromBody]RequestSearchWordInputModel model)
        {
            var (words, total) = _wordService.GetWords(model);
            var result = words.Select(o => new ResponseWordOutputModel()
            {
                Id = o.Id,
                GroupId = o.GroupId,
                GroupName = _groupService.GetOneGroupById(o.GroupId)?.Name,
                Name = o.Name,
                Cdt = o.Cdt,
                Udt = o.Udt,
                Enable = o.Enable
            }).ToList();
            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = (int)CodeEnum.Success,
                Message = "查询成功",
                Content = new
                {
                    result,
                    total
                }
            });
        }

        [HttpPost("[action]")]
        public IActionResult UpdateWord([FromBody]RequestWordInputModel model)
        {
            var result = _wordService.UpdateOneWord(model.Id, model.Name, model.GroupId);

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Message = result > 0 ? "修改成功" : "修改失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult ModifyWordStatus([FromBody]RequestWordInputModel model)
        {
            var (result, isEnable) = _wordService.DelOneWord(model.Id);

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Message = result > 0 ? isEnable ? "禁用成功" : "启用成功" : isEnable ? "禁用失败" : "启用失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult Publish()
        {
            var result = _modifiedService.AddOneModified();

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Message = result > 0 ? "发布成功" : "发布失败"
            });
        }
    }
}