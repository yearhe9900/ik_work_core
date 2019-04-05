using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ik_word_management.Models.Domain;
using ik_word_management.Models.DTO.Input;

namespace ik_word_management.Services.IService
{
    public interface IGroupService
    {
        int AddOneGroup(string name);

        int UpdateOneGroup(Guid id, string name);

        (int result, bool isEnable) DelOneGroup(Guid id);

        List<Groups> GetGroups(RequestSearchGroupInputModel model);
    }
}
