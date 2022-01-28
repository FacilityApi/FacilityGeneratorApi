using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Facility.GeneratorApi.WebApi;

// from http://blog.stephencleary.com/2016/11/streaming-zip-on-aspnet-core.html
internal sealed class FileCallbackResult : FileResult
{
	public FileCallbackResult(string contentType, Func<Stream, ActionContext, Task> callback)
		: base(contentType)
	{
		Callback = callback;
	}

	public Func<Stream, ActionContext, Task> Callback { get; }

	public override Task ExecuteResultAsync(ActionContext context)
	{
		if (context == null)
			throw new ArgumentNullException(nameof(context));

		context.HttpContext.Features.Get<IHttpBodyControlFeature>()!.AllowSynchronousIO = true;

		var executor = new FileCallbackResultExecutor(context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>());
		return executor.ExecuteAsync(context, this);
	}

	private sealed class FileCallbackResultExecutor : FileResultExecutorBase
	{
		public FileCallbackResultExecutor(ILoggerFactory loggerFactory)
			: base(CreateLogger<FileCallbackResultExecutor>(loggerFactory))
		{
		}

		public Task ExecuteAsync(ActionContext context, FileCallbackResult result)
		{
			SetHeadersAndLog(context, result, fileLength: null, enableRangeProcessing: false);
			return result.Callback!(context.HttpContext.Response.Body, context);
		}
	}
}
