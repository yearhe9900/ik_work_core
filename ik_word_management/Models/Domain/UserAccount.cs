using System;
using System.Collections.Generic;

namespace ik_word_management.Models.Domain
{
    public partial class UserAccount
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime? Cdt { get; set; }
        public int? Enable { get; set; }
    }
}
