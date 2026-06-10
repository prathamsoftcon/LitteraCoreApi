using LitteraCore.Common.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LitteraCore.Common
{
    public sealed class ApiSecurityOperationFilter : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            var controllerAttributes = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .OfType<Attribute>()
                ?? Enumerable.Empty<Attribute>();
            var actionAttributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<Attribute>();
            var attributes = controllerAttributes.Concat(actionAttributes);

            if (attributes.OfType<IAllowAnonymous>().Any())
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
                return;
            }

            var usesApiKey = attributes
                .OfType<AuthorizeAttribute>()
                .Any(attribute =>
                    string.Equals(
                        attribute.Policy,
                        ApiKeyAuthenticationDefaults.PolicyName,
                        StringComparison.Ordinal));
            var scheme = usesApiKey
                ? ApiKeyAuthenticationDefaults.AuthenticationScheme
                : "Bearer";

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = scheme
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
        }
    }
}
