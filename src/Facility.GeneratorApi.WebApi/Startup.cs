using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
			services.AddMvc()
				.AddJsonOptions(options =>
				{
					var serializerSettings = options.SerializerSettings;
					serializerSettings.NullValueHandling = NullValueHandling.Ignore;
					serializerSettings.DateParseHandling = DateParseHandling.None;
					serializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
					serializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
				});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			app.UseMvc();
		}
	}
}
