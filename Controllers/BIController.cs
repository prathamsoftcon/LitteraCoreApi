using Azure.Core;
using Google.Apis.Auth.OAuth2;
using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace LitteraCore.Controllers
{
    public class BIController : Controller
    {
        private readonly ILogger<BIController> _logger;

        private readonly IConfiguration _configuration;
        public BIController(IConfiguration configuration, ILogger<BIController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        private const string AuthorityUrl = "https://login.microsoftonline.com/8e5ba931-d2f6-458b-afce-5911ffeee8e9";
        private const string ResourceUrl = "https://analysis.windows.net/powerbi/api";
        private const string ApiUrl = "https://api.powerbi.com/";

        private const string ClientId = "b752de65-9e5d-4021-8038-575065ea111e";
        private const string ClientSecret = "jGX8Q~A8eEcQc5QcgZT6gFbmIpZTw9478FUg8byc";
       

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/Get_BI_Report_List")]
        [SwaggerOperation("To get BI report list.")]
        public IActionResult Get_BI_Report_List(PaginationParam param = null, string search = null)
        {
            PagedResult<BIReports>  B =new PagedResult<BIReports> ();
            BIBL bbl = new BIBL(_configuration);
            B = bbl.Get_BI_Report_List(param, search);

            return Ok(B);
        }

        [HttpGet]
        [Route("api/GetEmbedToken")]
        [SwaggerOperation("To generate BI embed token.")]
        public async Task<IActionResult> GetEmbedTokenAsync(string ReportId, string WorkspaceId)
        {
            var app = ConfidentialClientApplicationBuilder.Create(ClientId)
                .WithClientSecret(ClientSecret)
                .WithAuthority(new Uri(AuthorityUrl))
                .Build();

            var authResult = await app.AcquireTokenForClient(new[] { $"{ResourceUrl}/.default" }).ExecuteAsync();

            var tokenCredentials = new TokenCredentials(authResult.AccessToken, "Bearer");

            using var powerBIClient = new PowerBIClient(new Uri(ApiUrl), tokenCredentials);

            var report = await powerBIClient.Reports.GetReportInGroupAsync(Guid.Parse(WorkspaceId), Guid.Parse(ReportId));

            var tokenRequest = new GenerateTokenRequest(accessLevel: "view");

            var embedToken = await powerBIClient.Reports.GenerateTokenAsync(Guid.Parse(WorkspaceId), Guid.Parse(ReportId), tokenRequest);

            var result = new PowerBIEmbedDetails
            {
                AccessToken = embedToken.Token,
                EmbedUrl = report.EmbedUrl,
                ReportId = report.Id.ToString()
            };

            return Ok(result);
        }
    }
}
