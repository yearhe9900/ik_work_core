﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Models.DTO.Input
{
    public class RequestRefreshTokenInputModel
    {
        public Guid RefreshToken { get; set; }

        public Guid AccountID { get; set; }
    }
}
