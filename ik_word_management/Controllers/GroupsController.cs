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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ik_word_management.Controllers
{
    [Produces("application/json")]
    [Route("api/Groups")]
    public class GroupsController : Controller
    {
        private IKWordContext _iKWordContext = null;

        public GroupsController(IKWordContext iKWordContext)
        {
            _iKWordContext = iKWordContext;
        }

        [HttpPost("[action]")]
        public IActionResult AddGroup([FromBody]RequestGroupInputModel model)
        {
            Groups groups = new Groups()
            {
                Id = Guid.NewGuid(),
                Cdt = DateTime.Now,
                Enable = (int)EnableEnum.Enable,
                Udt = DateTime.Now,
                Name = model.Name
            };
            _iKWordContext.Entry(groups).State = EntityState.Added;

            var result = _iKWordContext.SaveChanges();

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Msg = result > 0 ? "添加成功" : "添加失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult UpdateGroup([FromBody]RequestGroupInputModel model)
        {
            var groups = _iKWordContext.Set<Groups>().Single(o => o.Id == model.Id);
            groups.Name = model.Name;
            groups.Udt = DateTime.Now;
            _iKWordContext.Set<Groups>().Attach(groups);
            _iKWordContext.Entry(groups).Property(a => a.Name).IsModified = true; //将EF对Name的管理状态设置为是一个更新
            _iKWordContext.Entry(groups).Property(a => a.Udt).IsModified = true; //将EF对UDT的管理状态设置为是一个更新

            var result = _iKWordContext.SaveChanges();

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Msg = result > 0 ? "修改成功" : "修改失败"
            });
        }

        [HttpPost("[action]")]
        public IActionResult DelGroup([FromBody]RequestGroupInputModel model)
        {
            var groups = _iKWordContext.Set<Groups>().Single(o => o.Id == model.Id);
            var isEnable = groups.Enable == (int)EnableEnum.Enable;
            groups.Enable = isEnable ? (int)EnableEnum.Disable : (int)EnableEnum.Enable;
            groups.Udt = DateTime.Now;
            _iKWordContext.Set<Groups>().Attach(groups);
            _iKWordContext.Entry(groups).Property(a => a.Enable).IsModified = true; //将EF对Enable的管理状态设置为是一个更新
            _iKWordContext.Entry(groups).Property(a => a.Udt).IsModified = true; //将EF对UDT的管理状态设置为是一个更新
            var result = _iKWordContext.SaveChanges();

            return new OkObjectResult(new ResponseResultBaseModel
            {
                Code = result > 0 ? (int)CodeEnum.Success : (int)CodeEnum.Fail,
                Msg = result > 0 ? isEnable ? "禁用成功" : "启用成功" : isEnable ? "禁用失败" : "启用失败"
            });
        }

        [HttpPost("[action]")]
        [Authorize]
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