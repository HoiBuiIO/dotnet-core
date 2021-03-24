using Common.ErrorResult;
using Common.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using Skeleton.Data.Dto;
using Skeleton.Data.EntityFramework;
using Skeleton.Domain.Contracts;
using Skeleton.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Skeeton.Service.Features.PostFeatures.Queries
{

	public class GetCategoryPagingQuery : IRequest<ServiceResult>
	{
		public int PageSize { get; set; }
		public int PageIndex { get; set; }

		public GetCategoryPagingQuery(int pageSize, int pageIndex)
		{
			PageSize = pageSize;
			PageIndex = pageIndex;
		}
	}

	public class GetCategoryPagingHandler : IRequestHandler<GetCategoryPagingQuery, ServiceResult>
	{
		private readonly IUnitOfWork  _unitOfWorkRead;
		private readonly ILogger<GetCategoryPagingHandler> _logger;

		public GetCategoryPagingHandler(IUnitOfWork<ApplicationReadOnlyDbContext> unitOfWorkRead, ILogger<GetCategoryPagingHandler> logger)
		{
			_unitOfWorkRead = unitOfWorkRead;
			_logger = logger;
		}

		public async Task<ServiceResult> Handle(GetCategoryPagingQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var now = DateTime.UtcNow;
				var categoryRepo = _unitOfWorkRead.GetRepository<CategoryEntity>();
				
				var categoryList = await categoryRepo.GetPagingListAsync(selector: f => new CategoryDto
				{
					Id = f.Id,
					Name = f.Name,
					Slug = f.Slug

				},
				size: request.PageSize,
				index: request.PageIndex,
				cancellationToken: cancellationToken);

				return ServiceResult.Succeeded(categoryList);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error {ex}");
				return ServiceResult.Failed(HttpCode.BadRequest);
			}
		}
	}
}
