using System;
using System.Collections.Generic;

namespace ik_word_management.Models.Domain
{
    public partial class Modified
    {
        public Guid Id { get; set; }
        public DateTime? Cdt { get; set; }
        public string Etag { get; set; }
        public int? Enable { get; set; }
    }
}
