// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Facility.Core;

namespace Facility.GeneratorApi
{
	/// <summary>
	/// A delegating implementation of FacilityGeneratorApi.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public partial class DelegatingFacilityGeneratorApi : IFacilityGeneratorApi
	{
		/// <summary>
		/// Creates an instance with the specified delegator.
		/// </summary>
		public DelegatingFacilityGeneratorApi(ServiceDelegator delegator) =>
			m_delegator = delegator ?? throw new ArgumentNullException(nameof(delegator));

		/// <summary>
		/// Gets information about the API.
		/// </summary>
		public virtual async Task<ServiceResult<GetApiInfoResponseDto>> GetApiInfoAsync(GetApiInfoRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(FacilityGeneratorApiMethods.GetApiInfo, request, cancellationToken).ConfigureAwait(false)).Cast<GetApiInfoResponseDto>();

		/// <summary>
		/// Generates code from a service definition.
		/// </summary>
		public virtual async Task<ServiceResult<GenerateResponseDto>> GenerateAsync(GenerateRequestDto request, CancellationToken cancellationToken = default) =>
			(await m_delegator(FacilityGeneratorApiMethods.Generate, request, cancellationToken).ConfigureAwait(false)).Cast<GenerateResponseDto>();

		private readonly ServiceDelegator m_delegator;
	}
}