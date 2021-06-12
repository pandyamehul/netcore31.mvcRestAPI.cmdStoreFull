using System;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace CommandAPIClient
{
    public class AuthConfig
    {
         public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";
         public string TenantId { get; set; }
         public string ClientId { get; set; }
         public string Authority {
             get {
                 return string.Format(CultureInfo.InvariantCulture, Instance, TenantId);
             }
         }
         public string ClientSecret { get; set;}
         public string BaseAddress { get; set; }
         public string ResourceId { get; set; }

         public static AuthConfig ReadConfigFromJsonFile(string path)
         {
             IConfiguration _configuration;
             var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path);
            _configuration = builder.Build();
            return _configuration.Get<AuthConfig>();
         }
    }
}