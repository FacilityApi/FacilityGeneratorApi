namespace Facility.GeneratorApi.WebApi.Models
{
	public class GenerateRequestDto
	{
		public TextSourceDto Definition { get; set; }
		public GeneratorDto Generator { get; set; }
	}
}
