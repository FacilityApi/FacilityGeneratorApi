using Facility.Definition;
using Facility.Definition.Fsd;
using Facility.GeneratorApi.WebApi.Models;
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
		public IActionResult Post([FromBody] GenerateRequestDto request)
		{
			if (request == null)
				return BadRequest(new { message = "Missing JSON request." });
			if (request.Definition?.Text == null)
				return BadRequest(new { message = "Definition text required." });

			try
			{
				var service = new FsdParser().ParseDefinition(new ServiceTextSource(request.Definition.Text).WithName(request.Definition.Name));

				var generatorName = request.Generator?.Name;
				switch (generatorName)
				{
				case "fsd":
					return GenerateFsd(service);
				default:
					return BadRequest(new { message = $"Unrecognized generator '{generatorName}'." });
				}
			}
			catch (ServiceDefinitionException exception)
			{
				return Ok(new GenerateResponseDto
				{
					ParseError = new ParseErrorDto
					{
						Message = exception.Error,
						Line = exception.Position.LineNumber,
						Column = exception.Position.ColumnNumber,
					},
				});
			}
		}

		private IActionResult GenerateFsd(ServiceInfo service)
		{
			var generator = new FsdGenerator
			{
				GeneratorName = "fsdgenapi",
			};
			var output = generator.GenerateOutput(service);

			return Ok(new GenerateResponseDto
			{
				Output = new[]
				{
					new TextSourceDto
					{
						Name = output.Name,
						Text = output.Text,
					},
				},
			});
		}
	}
}
