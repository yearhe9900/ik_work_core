using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Models.DTO.Input
{
    public class RequestSearchWordInputModel : RequestSearchBaseInputModel
    {
        public string GroupName { get; set; }
    }
}
