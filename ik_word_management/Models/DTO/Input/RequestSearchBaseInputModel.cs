using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Models.DTO.Input
{
    public class RequestSearchBaseInputModel : RequstPageInputModel
    {
        public string Name { get; set; }

        public int Enable { get; set; }

        public List<DateTime?> CDT { get; set; }

        public List<DateTime?> UDT { get; set; }
    }
}
