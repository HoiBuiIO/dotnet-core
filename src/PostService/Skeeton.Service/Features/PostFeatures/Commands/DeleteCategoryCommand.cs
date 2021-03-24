using Common.ErrorResult;
using Common.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using Skeleton.Data.EntityFramework;
using Skeleton.Domain.Contracts;
using Skeleton.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Skeeton.Service.Features.PostFeatures.Commands
{
	public class DeleteCategoryCommand : IRequest<ServiceResult>
	{
		public readonly Guid Id;

		public DeleteCategoryCommand(Guid id)
		{
			Id = id;
		}
	}

	public class DeleteCategoryHandle : IRequestHandler<DeleteCategoryCommand, ServiceResult>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;

		public DeleteCategoryHandle( 
			IUnitOfWork<ApplicationReadWriteDbContext> unitOfWork,
			ILogger<DeleteCategoryHandle> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		public async Task<ServiceResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
		{
			// Should handle check null and valid condition for below code
			try
			{
				var categoryRepo = _unitOfWork.GetRepository<CategoryEntity>();

				var category = await categoryRepo.SingleOrDefaultAsync(
					e => e.Id == request.Id,
					cancellationToken: cancellationToken);

				// soft delete
				category.IsDelete = true;

				categoryRepo.Update(category);

				// Delete
				// await categoryRepo.DeleteAsync(category);

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


