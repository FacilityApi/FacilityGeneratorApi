// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using Facility.Core;

namespace Facility.GeneratorApi
{
	/// <summary>
	/// Request for GetApiInfo.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class GetApiInfoRequestDto : ServiceDto<GetApiInfoRequestDto>
	{
		/// <summary>
		/// Creates an instance.
		/// </summary>
		public GetApiInfoRequestDto()
		{
		}

		/// <summary>
		/// The JSON serializer.
		/// </summary>
		protected override JsonServiceSerializer JsonSerializer => SystemTextJsonServiceSerializer.Instance;

		/// <summary>
		/// Determines if two DTOs are equivalent.
		/// </summary>
		public override bool IsEquivalentTo(GetApiInfoRequestDto? other)
		{
			return other != null;
		}
	}
}
