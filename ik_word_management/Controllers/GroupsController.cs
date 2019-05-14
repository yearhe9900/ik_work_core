using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ik_word_management.Helper;
using ik_word_management.Models.Domain;
using ik_word_management.Models.DTO.Input;
using ik_word_management.Models.DTO.Output;
using ik_word_management.Models.Enum;
using ik_word_management.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ik_word_management.Controllers
{
    [Produces("application/json")]
    [Route("api/Groups")]
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly IKWordContext _iKWordContext = null;
        private IGroupService _groupService = null;

        public GroupsController(IKWordContext iKWordContext, IGroupService groupService)
        {
            _iKWordContext = iKWordContext;
            _groupService = groupService;
        }

        [HttpPost("[action]")]
        public IActionResult AddGroup([FromBody]RequestGroupInputModel model)
        {
            var result = _groupService.AddOneGroup(model.Name);
          
            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Message = result > 0 ? "添加成功" : "添加失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult UpdateGroup([FromBody]RequestGroupInputModel model)
        {
            var result = _groupService.UpdateOneGroup(model.Id, model.Name);

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Message = result > 0 ? "修改成功" : "修改失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult ModifyGroupStatus([FromBody]RequestGroupInputModel model)
        {
            var (result, isEnable) = _groupService.DelOneGroup(model.Id);

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Message = result > 0 ? isEnable ? "禁用成功" : "启用成功" : isEnable ? "禁用失败" : "启用失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult GetGroup([FromBody]RequestSearchGroupInputModel model)
        {
            var (result,total) = _groupService.GetGroups(model);
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
        public IActionResult GetEnableGroupByName([FromBody]RequestSearchGroupInputModel model)
        {
            var result = _groupService.GetGroupsByName(model.Name).Select(p => new
            {
                p.Id,
                p.Name
            }).ToList();

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = (int)CodeEnum.Success,
                Message = "查询成功",
                Content = result
            });
        }
    }
}