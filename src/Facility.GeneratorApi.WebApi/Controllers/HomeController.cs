using System;
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
				return BadRequest();
			if (request.Definition?.Text == null)
				return BadRequest();

			var generatorName = request.Generator?.Name;
			switch (generatorName)
			{
			case "reflect":
				return GenerateReflect(request.Definition);
			case "test":
				return GenerateTest(request.Definition);
			default:
				return BadRequest();
			}
		}

		private IActionResult GenerateReflect(TextSourceDto definition)
		{
			return Ok(new GenerateResponseDto
			{
				Output = new[]
				{
					new TextSourceDto
					{
						Name = "README",
						Text = "This test generator simply reflects the source as-is.",
					},
					definition,
				},
			});
		}

		private IActionResult GenerateTest(TextSourceDto definition)
		{
			return Ok(new GenerateResponseDto
			{
				Output = new[]
				{
					new TextSourceDto
					{
						Name = "README.md",
						Text = string.Join(Environment.NewLine,
							"# Test Generator",
							"",
							"This test generator generates bogus data in a number of file formats. This paragraph has some unnecessary words at the end so that we can see what happens with long lines.",
							""),
					},
					new TextSourceDto
					{
						Name = "Api.cs",
						Text = string.Join(Environment.NewLine,
							"public static class Api",
							"{",
							"\tpublic static string Test => \"This is a test string that is long enough to go off the edge.\";",
							"}",
							""),
					},
				},
			});
		}
	}
}
