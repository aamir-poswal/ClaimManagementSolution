using ClaimManagement.Application;
using ClaimManagement.Application.Claims.Commands;
using ClaimManagement.Infrastructure.CosmosDB;
using ClaimManagement.Infrastructure.CosmosDB.Persistence;
using FluentValidation.AspNetCore;
using InsuranceClaimHandler.API;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.IO;


namespace InsuranceClaimHandler.WriteAPI
{
    public class Startup
    {
        private static string GetKeyVaultEndpoint() => "https://ClaimKeyVault.vault.azure.net/";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Insurance Claim Handler API", Version = "v1" });
            });

            var builder = new ConfigurationBuilder();
            var keyVaultEndpoint = GetKeyVaultEndpoint();
            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(
                   new KeyVaultClient.AuthenticationCallback(
                      azureServiceTokenProvider.KeyVaultTokenCallback));
                builder.AddAzureKeyVault(
                   keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
            }
            var configurationRoot = builder.Build();

            services.AddLogging(logging =>
            {
                logging.AddConfiguration(Configuration.GetSection("Logging"));
                logging.AddConsole();
            }).Configure<LoggerFilterOptions>(options => options.MinLevel =
                                              LogLevel.Information);
            
            var cosmosDbAccount = configurationRoot["CosmosDbAccount"];
            var cosmosDbKey = configurationRoot["CosmosDbKey"];
            var cosmosDbDatabaseName = configurationRoot["CosmosDbDatabaseName"];
            var cosmosDbContainerName = configurationRoot["CosmosDbContainerName"];

            services.AddSingleton<ICosmosDbClaimDocumentService>(CosmosDBContext.InitializeCosmosClientInstanceAsync(cosmosDbAccount, cosmosDbKey, cosmosDbDatabaseName, cosmosDbContainerName).GetAwaiter().GetResult());

            services.AddSingleton<IConfiguration>(configurationRoot);

            services.AddTransient<IPublishEvent, PublishEvent>();

            services.AddMediatR(typeof(AddClaimCommandHandler));

            services.AddMediatR(typeof(Startup));

            services.AddHealthChecks();

            services.AddCors(o => o.AddPolicy("ClaimApiAccessPolicy", b =>
            {
                b.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add(typeof(ValidationFilter));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
               .AddFluentValidation(configuration =>
               {
                   configuration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                   configuration.RegisterValidatorsFromAssemblyContaining<AddClaimCommandValidator>();
               });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance Claim Handler API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health");

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseMvc();
        }
    }
}
