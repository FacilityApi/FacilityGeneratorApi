using Facility.Core.Http;
using Facility.GeneratorApi.Http;
using Facility.GeneratorApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Facility.GeneratorApi.WebApi
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();

			services.AddSingleton<IFacilityGeneratorApi, FacilityGeneratorApi>();
			services.AddSingleton<ServiceHttpHandlerSettings>();
			services.AddSingleton<FacilityGeneratorApiHttpHandler>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

			app.UseFacilityHttpHandler<FacilityGeneratorApiHttpHandler>();

			app.Use(async (context, next) =>
			{
				var request = context.Request;
				if (request.Method == "GET" && request.Path == "/")
				{
					context.Response.ContentType = "application/json";
					await context.Response.WriteAsync("{\"api\":\"fsdgenapi\"}").ConfigureAwait(false);
				}
				else
				{
					await next().ConfigureAwait(false);
				}
			});
		}
	}
}
