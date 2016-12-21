using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Facility.Core;
using Facility.Core.Http;
using Microsoft.AspNetCore.Mvc;

namespace Facility.GeneratorApi.WebApi.Controllers
{
	public class HomeController : Controller
	{
		public HomeController(IFacilityGeneratorApi api)
		{
			m_api = api;
		}

		[HttpPost("generate/zip")]
		public async Task<IActionResult> GenerateZip([FromForm] string definitionName, [FromForm] string definitionText, [FromForm] string generatorName, CancellationToken cancellationToken)
		{
			var request = new GenerateRequestDto
			{
				Definition = new NamedTextDto
				{
					Name = definitionName,
					Text = definitionText,
				},
				Generator = new GeneratorDto
				{
					Name = generatorName,
				},
			};

			var result = await m_api.GenerateAsync(request, cancellationToken).ConfigureAwait(true);
			if (result.IsFailure)
				return CreateActionResultFromError(result.Error);

			var response = result.Value;
			var failure = response.Failure;
			if (failure != null)
				return CreateActionResultFromError(ServiceErrors.CreateInvalidRequest($"({failure.Line},{failure.Column}): {failure.Message}"));

			return new FileCallbackResult("application/zip", async (outputStream, _) =>
			{
				using (var zipArchive = new ZipArchive(new WriteOnlyStreamWrapper(outputStream), ZipArchiveMode.Create))
				{
					foreach (var namedText in response.Output)
					{
						var zipEntry = zipArchive.CreateEntry(namedText.Name);
						using (var zipStream = zipEntry.Open())
						using (var writer = new StreamWriter(zipStream))
							await writer.WriteAsync(namedText.Text).ConfigureAwait(false);
					}
				}
			})
			{
				FileDownloadName = $"{request.Generator.Name}.zip"
			};
		}

		private static IActionResult CreateActionResultFromError(ServiceErrorDto error)
		{
			return new ContentResult
			{
				Content = ServiceJsonUtility.ToJson(error),
				ContentType = HttpServiceUtility.JsonMediaType,
				StatusCode = (int) (HttpServiceErrors.TryGetHttpStatusCode(error.Code) ?? HttpStatusCode.InternalServerError),
			};
		}

		readonly IFacilityGeneratorApi m_api;
	}
}
