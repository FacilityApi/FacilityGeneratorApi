// <auto-generated>
// DO NOT EDIT: generated by fsdgencsharp
// </auto-generated>
#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Facility.Core;
using Facility.Core.Http;

namespace Facility.GeneratorApi.Http
{
	/// <summary>
	/// Generates code from Facility Service Definitions.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class FacilityGeneratorApiHttpHandler : ServiceHttpHandler
	{
		/// <summary>
		/// Creates the handler.
		/// </summary>
		public FacilityGeneratorApiHttpHandler(IFacilityGeneratorApi service, ServiceHttpHandlerSettings? settings = null)
			: base(settings, s_defaults)
		{
			m_service = service ?? throw new ArgumentNullException(nameof(service));
		}

		/// <summary>
		/// Creates the handler.
		/// </summary>
		public FacilityGeneratorApiHttpHandler(Func<HttpRequestMessage, IFacilityGeneratorApi> getService, ServiceHttpHandlerSettings? settings = null)
			: base(settings, s_defaults)
		{
			m_getService = getService ?? throw new ArgumentNullException(nameof(getService));
		}

		/// <summary>
		/// Attempts to handle the HTTP request.
		/// </summary>
		public override async Task<HttpResponseMessage?> TryHandleHttpRequestAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken = default)
		{
			return await AdaptTask(TryHandleGetApiInfoAsync(httpRequest, cancellationToken)).ConfigureAwait(true) ??
				await AdaptTask(TryHandleGenerateAsync(httpRequest, cancellationToken)).ConfigureAwait(true);
		}

		/// <summary>
		/// Gets information about the API.
		/// </summary>
		public Task<HttpResponseMessage?> TryHandleGetApiInfoAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken = default) =>
			TryHandleServiceMethodAsync(FacilityGeneratorApiHttpMapping.GetApiInfoMapping, httpRequest, GetService(httpRequest).GetApiInfoAsync, cancellationToken);

		/// <summary>
		/// Generates code from a service definition.
		/// </summary>
		public Task<HttpResponseMessage?> TryHandleGenerateAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken = default) =>
			TryHandleServiceMethodAsync(FacilityGeneratorApiHttpMapping.GenerateMapping, httpRequest, GetService(httpRequest).GenerateAsync, cancellationToken);

		private IFacilityGeneratorApi GetService(HttpRequestMessage httpRequest) => m_service ?? m_getService!(httpRequest);

		private static readonly ServiceHttpHandlerDefaults s_defaults = new ServiceHttpHandlerDefaults
		{
			ContentSerializer = HttpContentSerializer.Create(SystemTextJsonServiceSerializer.Instance),
		};

		private readonly IFacilityGeneratorApi? m_service;
		private readonly Func<HttpRequestMessage, IFacilityGeneratorApi>? m_getService;
	}
}
