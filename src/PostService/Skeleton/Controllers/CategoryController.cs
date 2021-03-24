using Common.ApiResult;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skeeton.Service.Features.PostFeatures.Commands;
using Skeeton.Service.Features.PostFeatures.Queries;
using Skeleton.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CategoryController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("add-category")]
		public async Task<IActionResult> AddCategory(CategoryDto category)
		{
			var result = await _mediator.Send(new AddCategoryCommand(category));

			return this.Result(result);
		}

		[HttpPut("update-category")]
		public async Task<IActionResult> EditCategory(CategoryDto category)
		{
			var result = await _mediator.Send(new EditCategoryCommand(category));

			return this.Result(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategoryByIdAsync(Guid id)
		{
			var result = await _mediator.Send(new DeleteCategoryCommand(id));

			return this.Result(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategoryByIdAsync(Guid id)
		{
			var result = await _mediator.Send(new GetCategoryByIdQuery(id));

			return this.Result(result);
		}

		[HttpGet("category-list")]
		public async Task<IActionResult> GetCategoryPagingAsync(string search = null,
			int pageSize = 20,
			int pageIndex = 1)
		{
			var result = await _mediator.Send(new GetCategoryPagingQuery(pageSize, pageIndex));
			return this.Result(result);
		}
	}
}
