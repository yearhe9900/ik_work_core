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
    }
}
