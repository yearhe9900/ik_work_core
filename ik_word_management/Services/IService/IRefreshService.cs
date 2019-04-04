using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Services.IService
{
    public interface IRefreshService
    {
        int AddRefresh(Guid id, int expiresIn);

        int UpdateRefresh(Guid id, Guid newRefreshToken, int expiresIn);
    }
}
