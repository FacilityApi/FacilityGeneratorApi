using System;
using System.Collections.Generic;
using System.Linq;
using Facility.CSharp;
using Facility.Definition;
using Facility.Definition.CodeGen;
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
				var service = new FsdParser().ParseDefinition(new NamedText(request.Definition.Name + ".fsd", request.Definition.Text));

				var generatorName = request.Generator?.Name;
				switch (generatorName)
				{
				case "csharp":
					return GenerateCode(() => new CSharpGenerator(), g => g.GenerateOutput(service));
				case "fsd":
					return GenerateCode(() => new FsdGenerator(), g => g.GenerateOutput(service));
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

		private IActionResult GenerateCode<T>(Func<T> createGenerator, Func<T, CodeGenOutput> generateOutput)
			where T : CodeGenerator
		{
			var generator = createGenerator();
			generator.GeneratorName = "fsdgenapi";

			return Ok(new GenerateResponseDto
			{
				Output = generateOutput(generator)
					.NamedTexts
					.Select(x => new TextSourceDto
					{
						Name = x.Name,
						Text = x.Text,
					}).ToList(),
			});
		}
	}
}
