namespace Facility.GeneratorApi.WebApi;

public static class Program
{
	public static void Main()
	{
		new WebHostBuilder()
			.UseKestrel()
			.UseUrls("http://0.0.0.0:45054")
			.UseStartup<Startup>()
			.Build()
			.Run();
	}
}
