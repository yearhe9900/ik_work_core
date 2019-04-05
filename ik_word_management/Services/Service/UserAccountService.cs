using ik_word_management.Models.Domain;
using ik_word_management.Models.Enum;
using ik_word_management.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Services.Service
{
    public class UserAccountService : IUserAccountService
    {
        private IKWordContext _iKWordContext = null;

        public UserAccountService(IKWordContext iKWordContext)
        {
            _iKWordContext = iKWordContext;
        }

        public UserAccount GetUserAccountByLoginInfo(string name, string password)
        {
            var user = _iKWordContext.UserAccount.Where(o => o.Name == name && o.Password == password && o.Enable == (int)EnableEnum.Enable).FirstOrDefault();
            return user;
        }

        public UserAccount GetUserAccountByID(Guid id)
        {
            var user = _iKWordContext.UserAccount.Where(o => o.Id == id).FirstOrDefault();
            return user;
        }
    }
}
