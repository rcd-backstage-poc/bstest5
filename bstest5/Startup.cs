using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Framework.Api.Exceptions;
using Framework.Api.Middleware;
using Framework.Api.Processors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RockLib.HealthChecks.AspNetCore.ResponseWriter;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using bstest5.EntryPoints.Configuration;
using RockLib.Metrics.AspNetCore;
using RockLib.Metrics.DependencyInjection;


namespace bstest5
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
			services.AddControllers();
			services.AddCors();
			services.AddResponseCompression();
			services.AddApiVersioning(ConfigureApiVersioning);
			services.AddVersionedApiExplorer(ConfigureVersionedApiExplorer);
			services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
			services.AddSwaggerGen(ConfigureSwaggerGen);
			services.AddHealthChecks();
			services.AddMetricFactory();
			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseRouting();
			app.UseRockLibMetrics();
			
			//app.UseHttpsRedirection(); - most apps enforce this at the LB level.
			app.UseCors();
			app.UseResponseCompression();
			app.UseMiddleware<UnhandledExceptionLogger>();
			app.UseProblemExceptionHandler();
			app.UseSwagger();
			app.UseSwaggerUI(options => ConfigureSwaggerUI(options, apiVersionDescriptionProvider));
			app.UseEndpoints(ConfigureEndpoints);
		}

		private static void ConfigureApiVersioning(ApiVersioningOptions options)
		{
			options.ReportApiVersions = true;
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ApiVersionReader = new ApiRequestVersionReader();
			options.ApiVersionSelector = new LatestApiVersionSelector();
		}

		private static void ConfigureVersionedApiExplorer(ApiExplorerOptions options)
		{
			// add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
			// note: the specified format code will format the version as "'v'major[.minor][-status]"
			options.GroupNameFormat = "'v'VVV";
		}

		private static void ConfigureEndpoints(IEndpointRouteBuilder config)
		{
			config.MapHealthChecks("/healthcheck",
								   new HealthCheckOptions
								   {
									   ResponseWriter = RockLibHealthChecks.ResponseWriter
								   });
			config.MapControllers();
		}

		private static void ConfigureSwaggerGen(SwaggerGenOptions options)
		{
			options.OperationFilter<SwaggerDefaultValues>();

			var xmlFile = $"{typeof(Startup).GetTypeInfo().Assembly.GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			options.IncludeXmlComments(xmlPath);
		}

		private static void ConfigureSwaggerUI(SwaggerUIOptions options, IApiVersionDescriptionProvider provider)
		{
			foreach (var description in provider.ApiVersionDescriptions.Reverse())
			{
				options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
			}
		}
	}
}
