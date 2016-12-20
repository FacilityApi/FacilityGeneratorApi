using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Facility.Core;
using Facility.CSharp;
using Facility.Definition;
using Facility.Definition.CodeGen;
using Facility.Definition.Fsd;
using Facility.Definition.Swagger;
using Facility.JavaScript;
using Facility.Markdown;

#pragma warning disable 1998

namespace Facility.GeneratorApi.Services
{
	public sealed class FacilityGeneratorApi : IFacilityGeneratorApi
	{
		public Task<ServiceResult<GetApiInfoResponseDto>> GetApiInfoAsync(GetApiInfoRequestDto request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var version = GetType().GetTypeInfo().Assembly.GetName().Version;

			return Task.FromResult(ServiceResult.Success(new GetApiInfoResponseDto
			{
				Api = "fsdgenapi",
				Version = $"{version.Major}.{version.Minor}.{version.Build}",
			}));
		}

		public async Task<ServiceResult<GenerateResponseDto>> GenerateAsync(GenerateRequestDto request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			try
			{
				var input = new NamedText(request.Definition?.Name ?? "", request.Definition?.Text ?? "");
				bool isSwagger = ServiceDefinitionUtility.DetectFormat(input) == ServiceDefinitionFormat.Swagger;
				var service = isSwagger ? new SwaggerParser().ParseDefinition(input) : new FsdParser().ParseDefinition(input);

				var generatorName = request.Generator?.Name;
				switch (generatorName)
				{
				case "csharp":
					return ServiceResult.Success(GenerateCode(() => new CSharpGenerator(), g => g.GenerateOutput(service)));
				case "javascript":
					return ServiceResult.Success(GenerateCode(() => new JavaScriptGenerator(), g => g.GenerateOutput(service)));
				case "typescript":
					return ServiceResult.Success(GenerateCode(() => new JavaScriptGenerator { TypeScript = true }, g => g.GenerateOutput(service)));
				case "markdown":
					return ServiceResult.Success(GenerateCode(() => new MarkdownGenerator(), g => g.GenerateOutput(service)));
				case "fsd":
					return ServiceResult.Success(GenerateCode(() => new FsdGenerator(), g => g.GenerateOutput(service)));
				case "swagger-json":
					return ServiceResult.Success(GenerateCode(() => new SwaggerGenerator(), g => g.GenerateOutput(service)));
				case "swagger-yaml":
					return ServiceResult.Success(GenerateCode(() => new SwaggerGenerator { Yaml = true }, g => g.GenerateOutput(service)));
				case "crash":
					throw new InvalidOperationException("Intentional exception for diagnostic purposes.");
				default:
					return ServiceResult.Failure(ServiceErrors.CreateInvalidRequest($"Unrecognized generator '{generatorName}'."));
				}
			}
			catch (ServiceDefinitionException exception)
			{
				return ServiceResult.Success(new GenerateResponseDto
				{
					Failure = new FailureDto
					{
						Message = exception.Error,
						Line = exception.Position.LineNumber,
						Column = exception.Position.ColumnNumber,
					},
				});
			}
		}

		private GenerateResponseDto GenerateCode<T>(Func<T> createGenerator, Func<T, CodeGenOutput> generateOutput)
			where T : CodeGenerator
		{
			var generator = createGenerator();
			generator.GeneratorName = "fsdgenapi";
			generator.IndentText = "  ";
			generator.NewLine = "\n";

			return new GenerateResponseDto
			{
				Output = generateOutput(generator)
					.NamedTexts
					.Select(x => new NamedTextDto
					{
						Name = x.Name,
						Text = x.Text,
					}).ToList(),
			};
		}
	}
}
