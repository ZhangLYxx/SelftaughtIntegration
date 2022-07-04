using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.JWT
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public IEnumerable<string> Roles { get; }
        public RoleRequirement(params string[] roles)
        {
            Roles = roles ?? throw new ArgumentNullException(nameof(roles));
        }
    }
}
