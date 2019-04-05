using System;
using System.Collections.Generic;

namespace ik_word_management.Models.Domain
{
    public partial class Words
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public DateTime? Cdt { get; set; }
        public int? Enable { get; set; }
        public DateTime? Udt { get; set; }
    }
}
