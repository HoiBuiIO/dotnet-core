using Common.ErrorResult;
using Common.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skeleton.Data.Dto;
using Skeleton.Data.EntityFramework;
using Skeleton.Domain.Contracts;
using Skeleton.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Skeeton.Service.Features.PostFeatures.Queries
{
	public class GetCategoryByIdQuery : IRequest<ServiceResult>
	{
		public Guid CategoryId { get; set; }
		public GetCategoryByIdQuery(Guid id)
		{
			CategoryId = id;
		}
	}

	public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, ServiceResult>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;

		public GetCategoryByIdHandler(
			IUnitOfWork<ApplicationReadOnlyDbContext> unitOfWork,
			ILogger<GetCategoryByIdHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task<ServiceResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var category = await _unitOfWork.GetRepository<CategoryEntity>().TableNoTracking
					.Where(u => u.Id == request.CategoryId)
					.Select(n => new CategoryDto
					{
						Id = n.Id,
						Name = n.Name,
						Slug = n.Slug
						// Some properties is we have
					})
					.FirstOrDefaultAsync(cancellationToken);

				if (category == null)
				{
					return ServiceResult.Failed(ErrorCode.ERROR);
				}

				return ServiceResult.Succeeded(category);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error {ex}");
				return ServiceResult.Failed(HttpCode.InternalServerError);
			}
		}
	}
}
