//Right now, removing this file should have no impact.

// I checked the repo, and `AddRequiredHeaderParameter` is not referenced anywhere except its own declaration in [AddRequiredHeaderParameter.cs](c:\Projects\LitteraCoreReactAPI\Common\AddRequiredHeaderParameter.cs:10). The active Swagger filter is [ApiSecurityOperationFilter.cs](c:\Projects\LitteraCoreReactAPI\Common\ApiSecurityOperationFilter.cs:8), and it is the one registered in [Program.cs](c:\Projects\LitteraCoreReactAPI\Program.cs:235).

// So the practical answer is:

// - no runtime impact
// - no Swagger impact
// - no compile impact, as long as nothing else starts referencing this class

// The app currently uses:
// - [Program.cs](c:\Projects\LitteraCoreReactAPI\Program.cs:235) -> `c.OperationFilter<ApiSecurityOperationFilter>();`

// Not this older file.

// If you want, we can remove `AddRequiredHeaderParameter.cs` as dead code and keep the codebase a bit cleaner.

// using System.Collections.Generic;
// using Microsoft.AspNetCore.Mvc.ApiExplorer;
// using Microsoft.OpenApi.Models;
// using Swashbuckle.AspNetCore.Swagger;
// using Swashbuckle.AspNetCore.SwaggerGen;


// namespace LitteraCore.Common
// {
//     public class AddRequiredHeaderParameter : IOperationFilter
//     {
//         public void Apply(OpenApiOperation operation, OperationFilterContext context)
//         {
//             var endpoint = context.ApiDescription.ActionDescriptor.RouteValues["action"];
//             // Adds the APIKey header in Swagger for all endpoints except the actions intentionally excluded below.
//             if (!endpoint.Contains("Get_Activity_Token_Info") && !endpoint.Contains("Littera_Events") && !endpoint.Contains("User_Session_Details") && !endpoint.Contains("trainingplan")
//  && !endpoint.Contains("GenerateOTP_wk") && !endpoint.Contains("VerifyOTP_wk") && !endpoint.Contains("Participants_training_wk") && !endpoint.Contains("TRG_PARTICIPANT_DETAILS_wk")
//  && !endpoint.Contains("GET_CONTENT_DETAILS_wk") && !endpoint.Contains("GET_REACT_APP_CONFIGURATION_wk") && !endpoint.Contains("Check_First_Login_wk") && !endpoint.Contains("SAVE_USER_LOG_wk") && !endpoint.Contains("Save_Audit_Trail_wk")
//  && !endpoint.Contains("Learning_Time_wk") && !endpoint.Contains("check_content_learning_exist_wk") && !endpoint.Contains("Update_Session_Status_wk")
//             )
//             {
//                 if (operation.Parameters == null)
//                     operation.Parameters = new List<OpenApiParameter>();

//                 operation.Parameters.Add(new OpenApiParameter
//                 {
//                     Name = "APIKey",
//                     In = ParameterLocation.Header,
//                     Required = true
//                 });
//             }
          

//         }
//     }
// }
