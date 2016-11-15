// DO NOT EDIT: generated by fsdgencsharp
using System;
using System.Collections.Generic;
using Facility.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facility.GeneratorApi.WebApi.Models
{
	/// <summary>
	/// Response for Generate.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class GenerateResponseDto : ServiceDto<GenerateResponseDto>
	{
		/// <summary>
		/// Creates an instance.
		/// </summary>
		public GenerateResponseDto()
		{
		}

		/// <summary>
		/// The output from the generator.
		/// </summary>
		public IReadOnlyList<NamedTextDto> Output { get; set; }

		/// <summary>
		/// The failure, if any.
		/// </summary>
		public FailureDto Failure { get; set; }

		/// <summary>
		/// Determines if two DTOs are equivalent.
		/// </summary>
		public override bool IsEquivalentTo(GenerateResponseDto other)
		{
			return other != null &&
				ServiceDataUtility.AreEquivalentArrays(Output, other.Output, ServiceDataUtility.AreEquivalentDtos) &&
				ServiceDataUtility.AreEquivalentDtos(Failure, other.Failure);
		}
	}
}
