using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPlatformApi.Mapping.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}