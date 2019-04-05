using ik_word_management.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Services.IService
{
    public interface IUserAccountService
    {
        UserAccount GetUserAccountByLoginInfo(string name, string password);

        UserAccount GetUserAccountByID(Guid id);
    }
}
