using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Facility.GeneratorApi.WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseUrls("http://local.fsdgenapi.facility.io:45054")
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}
	}
}
