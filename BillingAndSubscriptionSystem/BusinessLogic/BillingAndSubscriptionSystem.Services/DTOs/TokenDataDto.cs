using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingAndSubscriptionSystem.Services.DTOs
{
    public class TokenDataDto
    {
        public string? Token { get; set; }
        public string Role { get; set; } = "User";
    }
}
