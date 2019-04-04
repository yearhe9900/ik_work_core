using System;
using System.Collections.Generic;

namespace ik_word_management.Models.Domain
{
    public partial class Refresh
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? RefreshToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
    }
}
