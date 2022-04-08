using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.IO;
using System.Text;
using webapi.Data;
using webapi.Data.Interfaces;
using webapi.DomainServices.ApiServices;
using webapi.DomainServices.Infrastructure.ApiServices;
using webapi.DomainServices.Interfaces;
using webapi.DomainServices.Interfaces.ApiServices;
using webapi.DomainServices.Services;

namespace webapi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IApiService<ICocktailAPI> CocktailApi;


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            CreateSerilogLogger();
            CocktailApi = new ApiService<ICocktailAPI>(Configuration.GetValue<string>("RemoteApiBasePath"), 0);
        }

        private static void SetLoggingAndTelemetry(IServiceCollection services)
        {
            services
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddSerilog(Log.Logger, dispose: true);
                });
        }

        private void CreateSerilogLogger()
        {
            var parsedLogLevel = LogEventLevel.Information;
            //this config wll only exist on DEBUG. FOr release it will try to use azure after configured following instructions described below
            var logFolderPath = Configuration.GetValue<string>("SerilogDebugLogPath");
            var logFilePath = string.Empty;

            if (!string.IsNullOrEmpty(logFolderPath))
            {
                //DEBUG
                logFilePath = Path.Combine(logFolderPath, $"webAPILOG.txt");
                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }
            }
            else
            {
                //Azure ENV
                //Azure AppService inner storage
                //Make sure Azure is properly configured as described in this post first:
                //https://shawn-shi.medium.com/proper-use-of-serilog-for-log-stream-and-filesystem-on-azure-app-service-a69e17e54b7b
                logFilePath = "D:\\home\\LogFiles\\http\\RawLogs\\AzureBotLog_.txt";
            }

            var configuredLog = new LoggerConfiguration()
                .WriteTo.Console(new RenderedCompactJsonFormatter(), parsedLogLevel)
                .WriteTo.Trace(outputTemplate: DateTime.Now.ToString())  //This is for Windows Azure Console Tracing
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, shared: true)
                .Enrich
                .FromLogContext()
                .CreateLogger();

            Log.Logger = configuredLog;
            Log.Logger?.Warning($"[{nameof(CreateSerilogLogger)}], Logger File Created at: {logFilePath}");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            SetLoggingAndTelemetry(services);
            var dbFolder = Configuration.GetSection("SQLiteDatabase").GetValue<string>("Folder");
            var dbName = Configuration.GetSection("SQLiteDatabase").GetValue<string>("Name");
            var dbEncriptionKey = Configuration.GetSection("SQLiteDatabase").GetValue<string>("EncriptionKey");
            var documentsPath = System.Environment.GetFolderPath((Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), dbFolder));
            var dbPath = Path.Combine(documentsPath, dbName);
            var azureStorageConnectionString = Configuration.GetSection("AzureSettings").GetValue<string>("BlobStorageConnectionString");

            //Configure your services in here
            //Stateless should be configured as singleton
            services.AddScoped<ISqlLiteContext>(provider => new SqlLiteContext(dbPath, dbEncriptionKey));
            services.AddScoped<ICocktailApiManagerService>(provider => new CocktailApiManagerService(CocktailApi));
            services.AddScoped<IAccountService, AccountsService>();
            services.AddScoped<IAzureBlobStorageService>(provider => new AzureBlobStorageService(azureStorageConnectionString));

            //Mongo Services
            services.AddScoped<IMongoContext, MongoContext>();


            services.AddHttpContextAccessor();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            var secret = Encoding.ASCII.GetBytes(Configuration["JwtConfig:secret"]);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidIssuer = "localhost:44314",
                        ValidAudience = "localhost:44314",
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            AddSwagger(services);
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerOptionalParametersFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "New API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
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

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1.0");
            });

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });

        }
    }

}
