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
	public class AddCategoryCommand : IRequest<ServiceResult>
	{
		public readonly CategoryDto Category;

		public AddCategoryCommand(CategoryDto category)
		{
			Category = category;
		}
	}

	public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, ServiceResult>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;

		public AddCategoryHandler(IUnitOfWork<ApplicationReadWriteDbContext> unitOfWork, 
			ILogger<AddCategoryHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task<ServiceResult> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
		{
			// Should validate and check condition for block bellow
			try
			{
				var categoryRequest = request.Category;
				var categoryRepository = _unitOfWork.GetRepository<CategoryEntity>();

				var newCategory = new CategoryEntity()
				{
					Id = Guid.NewGuid(),
					Name = categoryRequest.Name,
					Slug = categoryRequest.Slug,
				};

				await categoryRepository.InsertAsync(newCategory);

				await _unitOfWork.CommitAsync();

				return ServiceResult.Succeeded(newCategory);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error {ex}");
				return ServiceResult.Failed(HttpCode.BadRequest);
			}
		}
	}
}
