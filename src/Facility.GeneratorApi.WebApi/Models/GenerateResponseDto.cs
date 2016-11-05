using System.Collections.Generic;

namespace Facility.GeneratorApi.WebApi.Models
{
	public class GenerateResponseDto
	{
		public IReadOnlyList<TextSourceDto> Output { get; set; }
		public ParseErrorDto ParseError { get; set; }
	}
}
