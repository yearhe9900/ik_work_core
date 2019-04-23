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

        public Groups GetOneGroupById(Guid Id)
        {
            return _iKWordContext.Groups.Where(o => o.Id == Id).FirstOrDefault();
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

        public List<Groups> GetGroupsByName(string groupName)
        {
            Expression<Func<Groups, bool>> expression = o => o.Name.Contains(groupName);

            Expression<Func<Groups, bool>> expressionEnable = o => o.Enable == (int)EnableEnum.Enable;
            expression = expression.And(expressionEnable);

            var result = _iKWordContext.Groups.Where(expression).ToList();

            return result;
        }

        public (List<Groups> groups, int total) GetGroups(RequestSearchGroupInputModel model)
        {
            Expression<Func<Groups, bool>> expression = o => o.Name != null;
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                Expression<Func<Groups, bool>> expressionName = o => o.Name.Contains(model.Name);
                expression = expression.And(expressionName);
            }
            if (model.CDT != null && model.CDT.Count() > 0)
            {
                Expression<Func<Groups, bool>> expressionCDT = o => o.Cdt >= model.CDT[0].Value.ToLocalTime() && o.Cdt < model.CDT[1].Value.ToLocalTime().AddDays(1);
                expression = expression.And(expressionCDT);
            }
            if (model.UDT != null && model.UDT.Count() > 0)
            {
                Expression<Func<Groups, bool>> expressionUDT = o => o.Udt >= model.UDT[0].Value.ToLocalTime() && o.Cdt < model.UDT[1].Value.ToLocalTime().AddDays(1);
                expression = expression.And(expressionUDT);
            }
            if (model.Enable != 0)
            {
                Expression<Func<Groups, bool>> expressionEnable = o => o.Enable == model.Enable;
                expression = expression.And(expressionEnable);
            }

            var result = _iKWordContext.Groups.Where(expression).OrderByDescending(p => p.Udt).Skip((model.PageNo - 1) * model.PageSize).Take(model.PageSize).ToList();

            var total = _iKWordContext.Groups.Where(expression).Count();

            return (result, total);
        }
    }
}
