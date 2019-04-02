using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Models.DTO.Output
{
    public class ResponseResultBaseModel
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public object Content { get; set; }
    }
}
