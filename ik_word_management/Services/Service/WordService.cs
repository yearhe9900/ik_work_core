using ik_word_management.Models.Domain;
using ik_word_management.Models.Enum;
using ik_word_management.Services.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Services.Service
{
    public class WordService : IWordService
    {
        private IKWordContext _iKWordContext = null;

        public WordService(IKWordContext iKWordContext)
        {
            _iKWordContext = iKWordContext;
        }

        public int AddOneWord(string name, Guid groupId)
        {
            Words word = new Words()
            {
                Id = Guid.NewGuid(),
                Cdt = DateTime.Now,
                Enable = (int)EnableEnum.Enable,
                Udt = DateTime.Now,
                Name = name,
                GroupId = groupId
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
    }
}
