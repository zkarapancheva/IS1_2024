﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain
{
    public class StripeSettings
    {
        public string? PublishableKey { get; set; }
        public string? SecretKey { get; set; }
    }
}
