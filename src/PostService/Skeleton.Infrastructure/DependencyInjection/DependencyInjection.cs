using Common.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skeeton.Service.Features.PostFeatures.Commands;
using Skeeton.Service.Features.PostFeatures.Queries;
using Skeleton.Data.Repository;
using Skeleton.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Skeleton.Infrastructure.DependencyInjection
{
	public static class DependencyInjection
	{
		public static void AddMediatorCQRS(this IServiceCollection services)
		{
			services.AddMediatR(Assembly.GetExecutingAssembly());
		}

		public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
			where TContext : DbContext
		{
			services.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
			services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
			services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

			return services;
		}

		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IRequestHandler<AddCategoryCommand, ServiceResult>, AddCategoryHandler>();
			services.AddScoped<IRequestHandler<EditCategoryCommand, ServiceResult>, EditCategoryHandle>();
			services.AddScoped<IRequestHandler<DeleteCategoryCommand, ServiceResult>, DeleteCategoryHandle>();
			services.AddScoped<IRequestHandler<GetCategoryByIdQuery, ServiceResult>, GetCategoryByIdHandler>();
			services.AddScoped<IRequestHandler<GetCategoryPagingQuery, ServiceResult>, GetCategoryPagingHandler>();

			return services;
		}
	}
}
