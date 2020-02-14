using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.CustomPolicy
{
    // policy {type}
    public static class SecurityPolicyType
    {
        public static IEnumerable<string> Get()
        {
            yield return SecurityLevel;
            yield return Department;
        }

        public const string SecurityLevel = "SecurityLevel";
        public const string Department = "Department";
    }
}
