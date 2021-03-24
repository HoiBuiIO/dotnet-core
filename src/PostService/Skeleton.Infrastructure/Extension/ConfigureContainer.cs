using Microsoft.AspNetCore.Builder;
using Skeleton.Infrastructure.Middleware;

namespace Skeleton.Infrastructure.Extension
{
	public static class ConfigureContainer
	{
		public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<CustomExceptionMiddleware>();
		}

		public static void ConfigureSwagger(this IApplicationBuilder app)
		{
			app.UseSwagger();

			app.UseSwaggerUI(setupAction =>
			{
				setupAction.SwaggerEndpoint("/swagger/OpenAPISpecification/swagger.json", "API");
				setupAction.RoutePrefix = "OpenAPI";
			});
		}
	}
}
