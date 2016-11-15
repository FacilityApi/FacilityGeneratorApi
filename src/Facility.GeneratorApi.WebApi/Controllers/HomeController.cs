using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Facility.Core.Http;
using Facility.GeneratorApi.WebApi.Models;
using Facility.GeneratorApi.WebApi.Models.Http;
using Microsoft.AspNetCore.Mvc;

namespace Facility.GeneratorApi.WebApi.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet("")]
		public IActionResult Index()
		{
			return Ok(new { api = "fsdgenapi" });
		}

		[HttpPost("generate")]
		public async Task<HttpResponseMessage> Generate(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var response = await GetServiceHttpHandler().TryHandleGenerateAsync(request, cancellationToken).ConfigureAwait(false);
			if (response == null)
				throw new InvalidOperationException("Failed to handle request.");
			return response;
		}

		private FacilityGeneratorApiHttpHandler GetServiceHttpHandler()
		{
			return new FacilityGeneratorApiHttpHandler(new FacilityGeneratorApi(),
				new ServiceHttpHandlerSettings
				{
					RootPath = HttpContext.Request.PathBase,
				});
		}
	}
}
