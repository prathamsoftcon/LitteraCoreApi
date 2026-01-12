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
            if (!endpoint.Contains("Get_Activity_Token_Info") && !endpoint.Contains("Littera_Events") && !endpoint.Contains("User_Session_Details") && !endpoint.Contains("trainingplan")
&& !endpoint.Contains("UserInfo_wk") && !endpoint.Contains("GenerateOTP_wk") && !endpoint.Contains("VerifyOTP_wk") && !endpoint.Contains("Participants_training_wk") && !endpoint.Contains("TRG_PARTICIPANT_DETAILS_wk")
&& !endpoint.Contains("GET_CONTENT_DETAILS_wk") && !endpoint.Contains("GenerateActivityToken_wk") && !endpoint.Contains("GET_REACT_APP_CONFIGURATION_wk") && !endpoint.Contains("Check_First_Login_wk") && !endpoint.Contains("SAVE_USER_LOG_wk") && !endpoint.Contains("Save_Audit_Trail_wk")
&& !endpoint.Contains("Learning_Time_wk") && !endpoint.Contains("check_content_learning_exist_wk") && !endpoint.Contains("Update_Session_Status_wk")
)
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
