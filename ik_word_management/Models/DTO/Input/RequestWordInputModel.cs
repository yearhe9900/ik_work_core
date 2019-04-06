using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Models.DTO.Input
{
    public class RequestWordInputModel
    {
        public Guid Id { get; set; }

        public Guid GroupID { get; set; }

        public string Name { get; set; }
    }
}
