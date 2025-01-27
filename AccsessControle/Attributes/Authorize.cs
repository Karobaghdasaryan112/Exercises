using AccsessControl.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccsessControl.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorize : Attribute
    {
        public Roles Role { get; set; }
        public CustomAuthorize(Roles role)
        {
            Role = role;
        }

    }
}
