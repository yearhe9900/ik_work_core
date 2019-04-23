using ik_word_management.Helper;
using ik_word_management.Models.Domain;
using ik_word_management.Models.DTO.Input;
using ik_word_management.Models.DTO.Output;
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
    public class WordService : IWordService
    {
        private IKWordContext _iKWordContext = null;
        private IGroupService _groupService;

        public WordService(IKWordContext iKWordContext, IGroupService groupService)
        {
            _iKWordContext = iKWordContext;
            _groupService = groupService;
        }

        public int AddOneWord(RequestWordInputModel model)
        {
            Words word = new Words()
            {
                Id = Guid.NewGuid(),
                Cdt = DateTime.Now,
                Enable = (int)EnableEnum.Enable,
                Udt = DateTime.Now,
                Name = model.Name,
                GroupId = model.GroupId
            };
            _iKWordContext.Entry(word).State = EntityState.Added;

            var result = _iKWordContext.SaveChanges();

            return result;
        }


        public int UpdateOneWord(Guid id, string name, Guid? groupId = null)
        {
            var word = _iKWordContext.Set<Words>().Single(o => o.Id == id);
            word.Name = name;
            word.Udt = DateTime.Now;
            if (groupId != null)
            {
                word.GroupId = groupId.Value;
            }
            _iKWordContext.Set<Words>().Attach(word);
            _iKWordContext.Entry(word).Property(a => a.Name).IsModified = true; //将EF对Name的管理状态设置为是一个更新
            _iKWordContext.Entry(word).Property(a => a.Udt).IsModified = true; //将EF对UDT的管理状态设置为是一个更新

            if (groupId != null)
            {
                _iKWordContext.Entry(word).Property(a => a.GroupId).IsModified = true;
            }

            var result = _iKWordContext.SaveChanges();
            return result;
        }

        public (int result, bool isEnable) DelOneWord(Guid id)
        {
            var word = _iKWordContext.Set<Words>().Single(o => o.Id == id);
            var isEnable = word.Enable == (int)EnableEnum.Enable;
            word.Enable = isEnable ? (int)EnableEnum.Disable : (int)EnableEnum.Enable;
            word.Udt = DateTime.Now;
            _iKWordContext.Set<Words>().Attach(word);
            _iKWordContext.Entry(word).Property(a => a.Enable).IsModified = true; //将EF对Enable的管理状态设置为是一个更新
            _iKWordContext.Entry(word).Property(a => a.Udt).IsModified = true; //将EF对UDT的管理状态设置为是一个更新
            var result = _iKWordContext.SaveChanges();

            return (result, isEnable);
        }

        public List<string> GetAllEnableWordsName()
        {
            var result = _iKWordContext.Words.Where(o=>o.Enable == (int)EnableEnum.Enable).Select(p=>p.Name).ToList();
            return result;
        }

        public (List<Words>, int total) GetWords(RequestSearchWordInputModel model)
        {
            Expression<Func<Words, bool>> expression = o => o.Name != null;

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                Expression<Func<Words, bool>> expressionName = o => o.Name.Contains(model.Name);
                expression = expression.And(expressionName);
            }
            if (model.CDT != null && model.CDT.Count() > 0)
            {
                Expression<Func<Words, bool>> expressionCDT = o => o.Cdt >= model.CDT[0].Value.ToLocalTime() && o.Cdt < model.CDT[1].Value.ToLocalTime().AddDays(1);
                expression = expression.And(expressionCDT);
            }
            if (model.UDT != null && model.UDT.Count() > 0)
            {
                Expression<Func<Words, bool>> expressionUDT = o => o.Udt >= model.UDT[0].Value.ToLocalTime() && o.Cdt < model.UDT[1].Value.ToLocalTime().AddDays(1);
                expression = expression.And(expressionUDT);
            }
            if (model.Enable != 0)
            {
                Expression<Func<Words, bool>> expressionEnable = o => o.Enable == model.Enable;
                expression = expression.And(expressionEnable);
            }
            if (!string.IsNullOrWhiteSpace(model.GroupName))
            {
                var groups = _groupService.GetGroupsByName(model.GroupName);
                var groupIds = groups.Select(o => o.Id).ToList();
                Expression<Func<Words, bool>> expressionGroupId = o => groupIds.Contains(o.GroupId);
                expression = expression.And(expressionGroupId);
            }

            var result = _iKWordContext.Words.Where(expression).OrderByDescending(p => p.Udt).Skip((model.PageNo - 1) * model.PageSize).Take(model.PageSize).ToList();

            var total = _iKWordContext.Words.Where(expression).Count();

            return (result, total);
        }
    }
}
