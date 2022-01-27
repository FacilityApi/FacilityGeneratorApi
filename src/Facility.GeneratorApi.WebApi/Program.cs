using Microsoft.AspNetCore.Hosting;

namespace Facility.GeneratorApi.WebApi;

public static class Program
{
	public static void Main()
	{
		new WebHostBuilder()
			.UseKestrel()
			.UseUrls("http://localhost:45054")
			.UseStartup<Startup>()
			.Build()
			.Run();
	}
}
