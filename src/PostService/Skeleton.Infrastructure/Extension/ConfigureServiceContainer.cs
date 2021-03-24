using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Skeleton.Data.EntityFramework;
using Microsoft.OpenApi.Models;

namespace Skeleton.Infrastructure.Extension
{
	public static class ConfigureServiceContainer
	{
		public static void AddDbContext(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddDbContext<ApplicationReadWriteDbContext>(opt =>
			opt.UseMySQL(Environment.GetEnvironmentVariable("SKELETON_READ_WRITE_CONNECTION_STRING")));

			serviceCollection.AddDbContext<ApplicationReadOnlyDbContext>(opt =>
			opt.UseMySQL(Environment.GetEnvironmentVariable("SEKELETON_READ_ONLY_CONNECTION_STRING")));
		}

		public static void AddSwaggerOpenAPI(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSwaggerGen(setupAction =>
			{
				setupAction.SwaggerDoc(
					"OpenAPISpecification",
					new Microsoft.OpenApi.Models.OpenApiInfo()
					{
						Title = "User API",
						Version = "1"
					});
			});

		}

	}
}