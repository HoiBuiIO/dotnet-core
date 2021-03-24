using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Paging
{
	public static class PaginateExtensions
	{

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="index"></param>
		/// <param name="size"></param>
		/// <param name="from"></param>
		/// <returns></returns>
		public static IPaginate<T> ToPaginate<T>(this IEnumerable<T> source, int index, int size, int from = 1)
		{
			return new Paginate<T>(source, index, size, from);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="index"></param>
		/// <param name="size"></param>
		/// <param name="cancellationToken"></param>
		/// <param name="from"></param>
		/// <returns></returns>
		public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> queryable, int index, int size, CancellationToken cancellationToken, int from = 1)
		{
			var total = await queryable.CountAsync(cancellationToken);
			var items = await queryable.Skip((index - from) * size).Take(size).ToListAsync(cancellationToken);

			return new Paginate<T>()
			{
				Count = total,
				From = from,
				Index = index,
				Items = items,
				Pages = (int)Math.Ceiling(total / (double)size),
				Size = size,
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="converter"></param>
		/// <param name="index"></param>
		/// <param name="size"></param>
		/// <param name="from"></param>
		/// <returns></returns>
		public static IPaginate<TResult> ToPaginate<TSource, TResult>(this IEnumerable<TSource> source,
			Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int index, int size, int from = 1)
		{
			return new Paginate<TSource, TResult>(source, converter, index, size, from);
		}
	}
}