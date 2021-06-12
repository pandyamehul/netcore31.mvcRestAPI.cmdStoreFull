using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using CommandAPIClient;

namespace CommandAPIClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading Config");
            AuthConfig authConfig = AuthConfig.ReadConfigFromJsonFile("appsettings.json");
            Console.WriteLine($"Authority: {authConfig.Authority}");
            Console.WriteLine("Getting JWT Token....");
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            AuthConfig authConfig = AuthConfig.ReadConfigFromJsonFile("appsettings.json");
            IConfidentialClientApplication app;
            app = ConfidentialClientApplicationBuilder.Create(authConfig.ClientId)
                .WithClientSecret(authConfig.ClientSecret)
                .WithAuthority(new Uri (authConfig.Authority))
                .Build();
            string[] ResourceIds = new string[] { authConfig.ResourceId};
            AuthenticationResult authResult = null;
            try
            {
                authResult = await app.AcquireTokenForClient(ResourceIds).ExecuteAsync();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token Acquired .... \n");
                Console.WriteLine(authResult.AccessToken);
                Console.ResetColor();
            }
            catch (MsalClientException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error .... \n");
                Console.WriteLine(ex.Message);
                Console.ResetColor();                
            }
            if(!string.IsNullOrEmpty(authResult.AccessToken))
            {
                var httpClient = new HttpClient();
                var defaultReqHeader = httpClient.DefaultRequestHeaders;

                if(defaultReqHeader.Accept == null || !defaultReqHeader.Accept.Any(m => m.MediaType == "application/json"))
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));                    
                }
                defaultReqHeader.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                HttpResponseMessage response = await httpClient.GetAsync(authConfig.BaseAddress);
                if(response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(json);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed Response : {response.StatusCode}");
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
                Console.ResetColor();
            }
        }
    }
}
