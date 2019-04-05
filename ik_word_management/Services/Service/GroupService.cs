using ik_word_management.Helper;
using ik_word_management.Models.Domain;
using ik_word_management.Models.DTO.Input;
using ik_word_management.Models.Enum;
using ik_word_management.Services.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ik_word_management.Services.Service
{
    public class GroupService : IGroupService
    {

        private IKWordContext _iKWordContext = null;

        public GroupService(IKWordContext iKWordContext)
        {
            _iKWordContext = iKWordContext;
        }

        public int AddOneGroup(string name)
        {
            Groups groups = new Groups()
            {
                Id = Guid.NewGuid(),
                Cdt = DateTime.Now,
                Enable = (int)EnableEnum.Enable,
                Udt = DateTime.Now,
                Name = name
            };
            _iKWordContext.Entry(groups).State = EntityState.Added;

            var result = _iKWordContext.SaveChanges();

            return result;
        }

        public int UpdateOneGroup(Guid id, string name)
        {
            var groups = _iKWordContext.Set<Groups>().Single(o => o.Id == id);
            groups.Name = name;
            groups.Udt = DateTime.Now;
            _iKWordContext.Set<Groups>().Attach(groups);
            _iKWordContext.Entry(groups).Property(a => a.Name).IsModified = true; //将EF对Name的管理状态设置为是一个更新
            _iKWordContext.Entry(groups).Property(a => a.Udt).IsModified = true; //将EF对UDT的管理状态设置为是一个更新

            var result = _iKWordContext.SaveChanges();
            return result;
        }

        public (int result, bool isEnable) DelOneGroup(Guid id)
        {
            var groups = _iKWordContext.Set<Groups>().Single(o => o.Id == id);
            var isEnable = groups.Enable == (int)EnableEnum.Enable;
            groups.Enable = isEnable ? (int)EnableEnum.Disable : (int)EnableEnum.Enable;
            groups.Udt = DateTime.Now;
            _iKWordContext.Set<Groups>().Attach(groups);
            _iKWordContext.Entry(groups).Property(a => a.Enable).IsModified = true; //将EF对Enable的管理状态设置为是一个更新
            _iKWordContext.Entry(groups).Property(a => a.Udt).IsModified = true; //将EF对UDT的管理状态设置为是一个更新
            var result = _iKWordContext.SaveChanges();

            return (result, isEnable);
        }

        public List<Groups> GetGroups(RequestSearchGroupInputModel model)
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

            return result;

        }
    }
}
