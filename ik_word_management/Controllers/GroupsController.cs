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
        private IKWordContext _iKWordContext = null;
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
                Msg = result > 0 ? "添加成功" : "添加失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult UpdateGroup([FromBody]RequestGroupInputModel model)
        {
            var result = _groupService.UpdateOneGroup(model.Id, model.Name);

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Msg = result > 0 ? "修改成功" : "修改失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult DelGroup([FromBody]RequestGroupInputModel model)
        {
            var (result, isEnable) = _groupService.DelOneGroup(model.Id);

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Msg = result > 0 ? isEnable ? "禁用成功" : "启用成功" : isEnable ? "禁用失败" : "启用失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult GetGroup([FromBody]RequestSearchGroupInputModel model)
        {
            Expression<Func<Groups, bool>> expression = o => o.Name != null;
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                Expression<Func<Groups, bool>> expressionName = o => o.Name.Contains(model.Name);
                expression = expression.And(expressionName);
            }
            if (model.StartCDT != null && model.EndCDT != null)
            {
                Expression<Func<Groups, bool>> expressionCDT = o => o.Cdt >= model.StartCDT.Value && o.Cdt < model.EndCDT.Value.AddDays(1);
                expression = expression.And(expressionCDT);
            }
            if (model.StartUDT != null && model.EndUDT != null)
            {
                Expression<Func<Groups, bool>> expressionUDT = o => o.Udt >= model.StartUDT.Value && o.Cdt < model.EndUDT.Value.AddDays(1);
                expression = expression.And(expressionUDT);
            }
            if (model.Enable != 0)
            {
                Expression<Func<Groups, bool>> expressionEnable = o => o.Enable == model.Enable;
                expression = expression.And(expressionEnable);
            }

            var result = _iKWordContext.Groups.Where(expression).Skip((model.PageNo - 1) * model.PageSize).Take(model.PageSize).ToList();

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = (int)CodeEnum.Success,
                Msg = "查询成功",
                Content = result
            });
        }

        [HttpPost("[action]")]
        public IActionResult GetEnableGroup()
        {
            var result = _iKWordContext.Groups.Where(o => o.Enable == (int)EnableEnum.Enable).Select(p => new
            {
                p.Id,
                p.Name
            }).ToList();

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = (int)CodeEnum.Success,
                Msg = "查询成功",
                Content = result
            });
        }
    }
}