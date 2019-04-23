using ik_word_management.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Models.DTO.Output
{
    public class ResponseWordOutputModel : Words
    {
        public string GroupName { get; set; }
    }
}
