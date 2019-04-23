using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Models.Enum
{
    public enum CodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,

        /// <summary>
        /// 登录成功
        /// </summary>
        LoginSuccess = 201,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 600,

        /// <summary>
        /// 登录失败
        /// </summary>
        LoginFail = 601,
      
        Error = 3000
    }
}
