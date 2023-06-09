﻿using Microsoft.AspNetCore.Identity;

namespace Project.Identity.Entities
{
    public class AppUser:IdentityUser<string>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
