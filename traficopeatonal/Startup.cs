using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CETYS.Posgrado.imi359.Models;

namespace CETYS.Posgrado.imi359
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ACTION: To refresh the models use the following PM command
            // Scaffold-DbContext "server=localhost;userid=eduardo;pwd=elP@ssword1;database=traficopeatonal;sslmode=none;" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -DataAnnotations -f -context "TraficoPeatonalContext"

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var useProdDbText = Environment.GetEnvironmentVariable("USE_PROD_DB");

            var useProdDb = bool.Parse(useProdDbText);
            var isDevelopment = environment == EnvironmentName.Development;
            var connectionString = isDevelopment ?
                Configuration.GetConnectionString("Connection_String" + (useProdDb ? "-PROD" : string.Empty)) :
                Configuration.GetConnectionString("Connection_String");

            services.AddDbContext<TraficoPeatonalContext>(options =>
            {
                options.UseMySQL(
                    connectionString,
                    mySqlOptions => { mySqlOptions.CommandTimeout(30); });
            });
            services.AddMvc()
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddApiVersioning();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
