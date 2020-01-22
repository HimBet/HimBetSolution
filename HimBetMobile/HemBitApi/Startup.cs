using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HemBit.Model;
using HemBit.Services;
using HemBitApi.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace HemBitApi
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
            services.Configure<HemBitDatabaseSettings>(
            Configuration.GetSection(nameof(HemBitDatabaseSettings)));
            services.AddSingleton<IHemBitDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<HemBitDatabaseSettings>>().Value);

            services.AddSingleton<HemBitDBService<Team>>();
            services.AddSingleton<PlayerDBService>();
            services.AddSingleton<PlayerStatisticsDBService>();
            services.AddSingleton<HemBitDBService<Game>>();
            services.AddSingleton<HemBitDBService<PlayerStatistics>>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IPlayerController, PlayerController>();
            services.AddSingleton<IPlayerStatisticsController, PlayerStatisticsController>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "HemBit API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HemBit API V1");
            });
        }
    }
}
