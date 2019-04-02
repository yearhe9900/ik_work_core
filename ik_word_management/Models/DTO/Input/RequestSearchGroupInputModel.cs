using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Models.DTO.Input
{
    public class RequestSearchGroupInputModel: RequstPageInputModel
    {
        public string Name { get; set; }

        public int Enable { get; set; }

        public DateTime? StartCDT { get; set; }
        public DateTime? EndCDT { get; set; }

        public DateTime? StartUDT { get; set; }
        public DateTime? EndUDT { get; set; }
    }
}
