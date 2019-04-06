﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Services.IService
{
    public interface IWordService
    {
        int AddOneWord(string name, Guid groupId);

        int UpdateOneWord(Guid id, string name, Guid? groupId = null);

        (int result, bool isEnable) DelOneWord(Guid id);
    }
}
