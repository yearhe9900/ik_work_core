using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Models.DTO.Output
{
    public class ResponseTokenOutputModel
    {
        public string AuthToken { get; set; }

        public int ExpiresIn { get; set; }

        public string TokenType { get; set; } = "Bearer";

        public string RefreshToken { get; set; }
    }
}
