using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Authorization.Infrastructure.CustomPolicy
{
    public static class SecurityAuthorizationPolicyFactory
    {
        public static AuthorizationPolicy Create(string policyName)
        {
            var policyParts = policyName.Split('.');
            var type = policyParts.First();
            var value = policyParts.Last();

            switch(type)
            {
                case SecurityPolicyType.Department:
                    return new AuthorizationPolicyBuilder()
                        .RequireClaim("Department", value)
                        .Build();
                case SecurityPolicyType.SecurityLevel:
                    return new AuthorizationPolicyBuilder()
                        .AddRequirements(new SecurityLevelRequirement(Convert.ToInt32(value)))
                        .Build();
                default:
                    return null;
            }
        }
    }
}
