using Common.Paging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Skeleton.Domain.Contracts
{
	public interface IRepository<T> : IBaseRepository<T>, IDisposable where T : class
	{

		#region Get Async Functions
		Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool enableTracking = false,
			bool ignoreQueryFilters = false,
			CancellationToken cancellationToken = default);

		Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool enableTracking = true,
			CancellationToken cancellationToken = default);

		Task<IPaginate<TResult>> GetPagingListAsync<TResult>(Expression<Func<T, TResult>> selector,
			Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			int index = 1,
			int size = 20,
			bool enableTracking = true,
			CancellationToken cancellationToken = default);

		Task<IPaginate<T>> GetPagingListAsync(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			int index = 1,
			int size = 20,
			bool enableTracking = true,
			CancellationToken cancellationToken = default);

		ICollection<T> GetList(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool enableTracking = true,
			CancellationToken cancellationToken = default,
			bool ignoreQueryFilters = false);

		ICollection<TResult> GetList<TResult>(Expression<Func<T, TResult>> selector,
			Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool enableTracking = true,
			CancellationToken cancellationToken = default,
			bool ignoreQueryFilters = false);
		#endregion

		#region Get Functions
		T SingleOrDefault(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool enableTracking = true,
			bool ignoreQueryFilters = false);
		#endregion

		#region Insert
		T Insert(T entity);
		void Insert(params T[] entities);
		void Insert(IEnumerable<T> entities);
		#endregion

		#region Insert Async
		ValueTask<EntityEntry<T>> InsertAsync(T entity, CancellationToken cancellationToken = default);

		Task InsertAsync(params T[] entities);

		Task InsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
		#endregion

		#region Update
		void Update(T entity);
		void Update(params T[] entities);
		void Update(IEnumerable<T> entities);
		#endregion

		#region Delete
		void Delete(T entity);

		void Delete(Guid id);

		void Delete(params T[] entities);

		void Delete(IEnumerable<T> entities);
		#endregion Delete

		#region DeleteAsync
		Task<bool> DeleteAsync(T entity);
		Task<bool> DeleteAsync(IEnumerable<T> entities);
		Task<bool> DeleteAsync(int id);
		#endregion

		#region Table
		IQueryable<T> Table { get; }
		IQueryable<T> TableNoTracking { get; }
		#endregion
	}
}