using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace LitteraCore.Common
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var endpoint = context.ApiDescription.ActionDescriptor.RouteValues["action"];
            if (!endpoint.Contains("Get_Activity_Token_Info"))
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "APIKey",
                    In = ParameterLocation.Header,
                    Required = true
                });
            }
          

        }
    }
}
