using Facility.AspNetCore;
using Facility.Core.Http;
using Facility.GeneratorApi.Http;
using Facility.GeneratorApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Facility.GeneratorApi.WebApi;

public sealed class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddCors();
		services.AddSingleton<IFacilityGeneratorApi>(new FacilityGeneratorApi());
		services.AddSingleton<ServiceHttpHandlerSettings>();
		services.AddSingleton<FacilityGeneratorApiHttpHandler>();
		services.AddControllers();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
		app.UseFacilityExceptionHandler(includeErrorDetails: env.IsDevelopment());
		app.UseFacilityHttpHandler<FacilityGeneratorApiHttpHandler>();
		app.UseRouting();
		app.UseEndpoints(x => x.MapControllers());
	}
}
