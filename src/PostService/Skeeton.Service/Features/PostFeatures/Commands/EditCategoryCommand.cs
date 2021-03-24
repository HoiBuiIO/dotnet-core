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

namespace Skeeton.Service.Features.PostFeatures.Commands
{
	public class EditCategoryCommand : IRequest<ServiceResult>
	{
		public readonly CategoryDto Category;

		public EditCategoryCommand(CategoryDto category)
		{
			Category = category;
		}
	}

	public class EditCategoryHandle : IRequestHandler<EditCategoryCommand, ServiceResult>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;

		public EditCategoryHandle(
			IUnitOfWork<ApplicationReadWriteDbContext> unitOfWork,
			ILogger<EditCategoryHandle> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		public async Task<ServiceResult> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var categoryRepo = _unitOfWork.GetRepository<CategoryEntity>();

				var category = await categoryRepo.SingleOrDefaultAsync(
					e => e.Id == request.Category.Id,
					cancellationToken: cancellationToken);

				var categoryRequest = request.Category;
				category.Name = categoryRequest.Name;
				category.Slug = categoryRequest.Slug;

				categoryRepo.Update(category);

				await _unitOfWork.CommitAsync();

				return ServiceResult.Succeeded(category);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error {ex}");
				return ServiceResult.Failed(HttpCode.BadRequest);
			}
		}
	}
}
