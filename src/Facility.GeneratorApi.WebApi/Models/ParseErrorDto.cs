namespace Facility.GeneratorApi.WebApi.Models
{
	public class ParseErrorDto
	{
		public int? Line { get; set; }
		public int? Column { get; set; }
		public string Message { get; set; }
	}
}
