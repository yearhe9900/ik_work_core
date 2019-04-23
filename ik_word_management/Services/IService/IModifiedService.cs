using ik_word_management.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Services.IService
{
    public interface IModifiedService
    {
        Modified GetLastModified();

        int AddOneModified();

    }
}
