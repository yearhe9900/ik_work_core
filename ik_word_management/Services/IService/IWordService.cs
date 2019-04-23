using ik_word_management.Models.Domain;
using ik_word_management.Models.DTO.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Services.IService
{
    public interface IWordService
    {
        (List<Words>, int total) GetWords(RequestSearchWordInputModel model);

        List<string> GetAllEnableWordsName();

        int AddOneWord(RequestWordInputModel model);

        int UpdateOneWord(Guid id, string name, Guid? groupId = null);

        (int result, bool isEnable) DelOneWord(Guid id);
    }
}
