using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace Skeleton.Infrastructure.Middleware
{
	public class CustomExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<CustomExceptionMiddleware> _logger;
		public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception exceptionObj)
			{
				await HandleExceptionAsync(context, exceptionObj, _logger);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<CustomExceptionMiddleware> logger)
		{
			var code = HttpStatusCode.InternalServerError;

			logger.LogError(ex.Message);

			var result = JsonSerializer.Serialize(new { StatusCode = (int)code, ErrorMessage = ex.Message });
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;
			return context.Response.WriteAsync(result);
		}
	}

}