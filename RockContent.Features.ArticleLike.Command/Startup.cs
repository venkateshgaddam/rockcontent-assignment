using System.Collections.Generic;
using System.Data;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Npgsql;
using RockContent.Common;
using RockContent.Common.DAL;
using RockContent.Common.Redis;
using RockContent.Features.ArticleLike.Command.Biz;
using RockContent.Features.ArticleLike.Command.DataService;
using RockContent.Features.ArticleLike.Command.Validator;

namespace RockContent.Features.ArticleLike.Command
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
            ThreadPool.SetMinThreads(500, 50);

            //Add Dependency Injection
            var redisConfig = Configuration.GetSection(GlobalConstants.REDIS_CONFIG_KEY).Get<RedisConfig>();
            var redisInstance = new RedisConnector(
                new RedisConfig()
                {
                    HostName = redisConfig.HostName,
                    Key = redisConfig.Key,
                    Port = redisConfig.Port,
                    SSL = redisConfig.SSL
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                        builder => builder
                        .WithOrigins(Configuration["AllowedOrigins"].Split(','))
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        );
            });

            services.AddSingleton(x => redisInstance);
            services.AddScoped<IArticleBiz, ArticleBiz>();
            services.AddScoped<IArticleDataService, ArticleDataService>();
            services.AddScoped<IRequestValidator, RequestValidator>();
            services.AddScoped<IRedisCacheRepository, RedisCacheRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IDbConnection>(x =>
            {
                return new NpgsqlConnection(Configuration[GlobalConstants.DATABASE_CONFIG_KEY]);
            });
            services.AddMvc(o => o.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = GlobalConstants.SWAGGER_TITLE, Version = "v1", Description = GlobalConstants.SWAGGER_DESCRIPTION });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"

                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference=new OpenApiReference{ Type=ReferenceType.SecurityScheme,Id="Bearer"},
                                Scheme="oauth2",
                                Name="Bearer",
                                In=ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentname}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(GlobalConstants.SWAGGER_URL, string.Format(GlobalConstants.SWAGGER_APP_NAME, "Command"));
                c.RoutePrefix = "swagger";
            });

            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
        }
    }
}
