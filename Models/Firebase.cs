using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace LitteraCore.Models
{
    public class Firebase
    {
        private readonly IConfiguration _configuration;
        public Firebase(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class NotificationRequest
        {
            public string UserToken { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
        }
        public class NotificationUsers
        {
            public NotifyAgencies[] users { get; set; }
        }


        public class NotifyAgencies
        {
            public string agencyid { get; set; }
        }
        public class root
        {
            public tokens[] tokendetails { get; set; }
        }
        public class tokens
        {
            public string tyft_agencyid { get; set; }
            public string tyft_token { get; set; }

            public DateTime tyft_createdon { get; set; }
        }

        private static readonly string[] SCOPES = { "https://www.googleapis.com/auth/firebase.messaging" };

        //static async Task Main(string[] args)
        //{
        //    // Start the HTTP server
        //    HttpListener listener = new HttpListener();
        //    listener.Prefixes.Add("http://localhost:5000/token/");
        //    listener.Start();
        //    Console.WriteLine("Server running on http://localhost:5000/token");

        //    while (true)
        //    {
        //        try
        //        {
        //            // Handle incoming HTTP requests
        //            HttpListenerContext context = await listener.GetContextAsync();
        //            if (context.Request.HttpMethod == "GET")
        //            {
        //                try
        //                {
        //                    string accessToken = await GetAccessTokenAsync();
        //                    var response = new { access_token = accessToken };

        //                    // Send a successful response with the access token
        //                    string jsonResponse = JsonSerializer.Serialize(response);
        //                    context.Response.ContentType = "application/json";
        //                    context.Response.StatusCode = 200;
        //                    using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
        //                    {
        //                        writer.Write(jsonResponse);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    // Handle errors and send an error response
        //                    var errorResponse = new { error = "Failed to get access token", details = ex.Message };
        //                    string jsonResponse = JsonSerializer.Serialize(errorResponse);
        //                    context.Response.ContentType = "application/json";
        //                    context.Response.StatusCode = 500;
        //                    using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
        //                    {
        //                        writer.Write(jsonResponse);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                // Send a 404 response for unsupported endpoints
        //                context.Response.StatusCode = 404;
        //                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
        //                {
        //                    writer.Write("Endpoint not found");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Server error: {ex.Message}");
        //        }
        //    }
        //}

        // Function to get an access token
        private static async Task<string> GetAccessTokenAsync()
        {
            
            string keyPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "service-account.json");
            if (!File.Exists(keyPath))
            {
                throw new FileNotFoundException("Service account file not found", keyPath);
            }

            string fileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "service-account.json");

            string scopes = "https://www.googleapis.com/auth/firebase.messaging";
            var bearertoken = ""; // Bearer Token in this variable

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                bearertoken = GoogleCredential
                  .FromStream(stream) // Loads key file
                  .CreateScoped(scopes) // Gathers scopes requested
                  .UnderlyingCredential // Gets the credentials
                  .GetAccessTokenForRequestAsync().Result; // Gets the Access Token
            }
            return bearertoken;
        }


        public  async Task Send_Notification(string usertoken,string title,string message)
        {
            string url = _configuration["FIREBASE_API_PATH"];

            // Authorization Bearer Token
            string bearerToken = await GetAccessTokenAsync();

            // Request body
            string jsonBody = $@"
            {{
               ""message"": {{
                  ""token"": ""{usertoken}"",
                  ""notification"": {{
                    ""body"": ""{message}"",
                    ""title"": ""{title}""
                  }}
               }}
            }}";


            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Add Authorization header
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                    // Prepare the request
                    HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    // Send the request
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Handle the response
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Success! Response: " + responseBody);
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Failed! Status Code: {response.StatusCode}, Response: {errorResponse}");
                        throw new Exception(errorResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }


}
