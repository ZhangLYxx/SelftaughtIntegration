using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.JWT
{
    public class ManagerAuthorizeAttribute: AuthorizeAttribute
    {
        public ManagerAuthorizeAttribute() : base("Manager")
        {

        }
    }
}
