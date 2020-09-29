using AutoMapper;
using Football.API.AutoMapper;
using Football.API.Extensions;
using Football.Client;
using Football.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Football.API
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
            services.AddDbContext<FootballContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("FootballConnectionString"))
                    .EnableSensitiveDataLogging()
                );

            services.AddScoped<IFootballRepository, FootballRepository>();

            services.AddAutoMapper(typeof(FootballProfile));

            services.AddSingleton<IFootballClient>(
                new FootballClient(
                    Configuration.GetValue<string>("FootballApiClient:Url"),
                    Configuration.GetValue<string>("FootballApiClient:HeaderKey"),
                    Configuration.GetValue<string>("FootballApiClient:HeaderValue"),
                    Configuration.GetValue<int>("FootballApiClient:TimeOut"))
            );

            services.AddMvc();

            services.AddSwaggerGen(swagger =>
                swagger.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Football API",
                        Version = "1",
                        Description = "Football data API importer"
                    }
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, FootballContext footballContext)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Football API v1");
            });

            if (env.IsDevelopment())
            {
                footballContext.Database.Migrate();
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler();

            app.UseMvc();
        }
    }
}
