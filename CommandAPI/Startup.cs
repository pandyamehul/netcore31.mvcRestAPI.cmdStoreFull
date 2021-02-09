using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CommandAPI.Models;
using Npgsql;

namespace CommandAPI
{
    public class Startup
    {
        public IConfiguration _configuration {get;}
        public Startup(IConfiguration configuration) => _configuration = configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("Starting Configure Service");
            var builder = new NpgsqlConnectionStringBuilder();
            builder.ConnectionString = _configuration.GetConnectionString("PostgreSqlConnection");
            Console.WriteLine(_configuration.GetConnectionString("PostgreSqlConnection"));
            builder.Username = _configuration["UserID"];
            builder.Password = _configuration["Password"];
            Console.WriteLine("Adding DB Context");
            services.AddDbContext<CommandContext> (opt => opt.UseNpgsql(builder.ConnectionString));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CommandContext context)
        {
            Console.WriteLine("Configuring DB");
            context.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //Below are default line added by the scaffhold
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Hello World!");
                // });
                endpoints.MapControllers();
            });
        }
    }
}
