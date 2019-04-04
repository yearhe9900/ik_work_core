using ik_word_management.Models.DTO.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ik_word_management.Helper
{
    public interface IJwtFactory
    {
        /// <summary>
        /// 生成token令牌
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        Task<ResponseTokenOutputModel> GenerateEncodedToken(string userName, ClaimsIdentity identity, string refreshToken = null);

        /// <summary>
        /// 生成身份原型
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
