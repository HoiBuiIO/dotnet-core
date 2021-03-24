using Common.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Skeleton.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Skeleton.Data.Repository
{
	public class Repository<T> : BaseRepository<T>, IRepository<T> where T : class
	{
		#region Constructor
		public Repository(DbContext context) : base(context)
		{
		}
		#endregion

		#region Delete Function
		public void Delete(T entity)
		{
			_dbSet.Remove(entity);
		}

		public void Delete(Guid id)
		{
			var entities = _dbSet.Find(id.ToString());
			if (entities == null)
			{
				throw new ArgumentNullException(nameof(entities));
			}
			_dbSet.Remove(entities);
		}

		public void Delete(params T[] entities)
		{
			_dbSet.RemoveRange(entities);
		}

		public void Delete(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
		}
		#endregion

		#region Delete Async Function
		public Task<bool> DeleteAsync(T entity)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Dispose Function
		public void Dispose()
		{
			_dbContext?.Dispose();
		}
		#endregion

		#region GetListAsync

		public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool enableTracking = true,
			bool ignoreQueryFilters = false,
			CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _dbSet;

			if (!enableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

			if (orderBy != null) return await orderBy(query).FirstOrDefaultAsync();

			return await query.FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool enableTracking = true,
			CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _dbSet;
			if (!enableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
				return await orderBy(query).ToListAsync(cancellationToken);

			return await query.ToListAsync(cancellationToken);
		}

		public Task<IPaginate<T>> GetPagingListAsync(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			int index = 1,
			int size = 20,
			bool enableTracking = true,
			CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _dbSet;
			if (!enableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
				return orderBy(query).ToPaginateAsync(index, size, 1, cancellationToken);
			return query.ToPaginateAsync(index, size, 1, cancellationToken);
		}

		public Task<IPaginate<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector,
			Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			int index = 1,
			int size = 20,
			bool enableTracking = true,
			CancellationToken cancellationToken = default,
			bool ignoreQueryFilters = false)
			where TResult : class
		{
			IQueryable<T> query = _dbSet;

			if (!enableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

			if (orderBy != null)
				return orderBy(query).Select(selector).ToPaginateAsync(index, size, 1, cancellationToken);

			return query.Select(selector).ToPaginateAsync(index, size, 1, cancellationToken);
		}

		public Task<IPaginate<TResult>> GetPagingListAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 1, int size = 20, bool enableTracking = true, CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _dbSet;
			if (!enableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
				return orderBy(query).Select(selector).ToPaginateAsync(index, size, 1, cancellationToken);
			return query.Select(selector).ToPaginateAsync(index, size, 1, cancellationToken);
		}

		public ICollection<T> GetList(Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool enableTracking = true,
			CancellationToken cancellationToken = default,
			bool ignoreQueryFilters = false)
		{
			IQueryable<T> query = _dbSet;

			if (!enableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

			if (!query.Any())
			{
				throw new ArgumentNullException(nameof(query));
			}

			if (orderBy != null) return orderBy(query).ToList();

			return query.ToList();
		}

		public ICollection<TResult> GetList<TResult>(Expression<Func<T, TResult>> selector,
			Expression<Func<T, bool>> predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool enableTracking = true,
			CancellationToken cancellationToken = default,
			bool ignoreQueryFilters = false)
		{
			IQueryable<T> query = _dbSet;

			if (!enableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

			if (!query.Any())
			{
				throw new ArgumentNullException(nameof(query));
			}

			if (orderBy != null) return orderBy(query).Select(selector).ToList();

			return query.Select(selector).ToList();
		}

		#endregion

		#region Insert Functions
		public virtual T Insert(T entity)
		{
			return _dbSet.Add(entity).Entity;
		}

		public void Insert(params T[] entities)
		{
			_dbSet.AddRange(entities);
		}

		public void Insert(IEnumerable<T> entities)
		{
			_dbSet.AddRange(entities);
		}
		#endregion

		#region Insert Async Functions
		public virtual ValueTask<EntityEntry<T>> InsertAsync(T entity, CancellationToken cancellationToken = default)
		{
			return _dbSet.AddAsync(entity, cancellationToken);
		}


		public virtual Task InsertAsync(params T[] entities)
		{
			return _dbSet.AddRangeAsync(entities);
		}


		public virtual Task InsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
		{
			return _dbSet.AddRangeAsync(entities, cancellationToken);
		}
		#endregion

		#region Get Functions
		public T SingleOrDefault(Expression<Func<T, bool>> predicate = null,
					Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
					Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true,
					bool ignoreQueryFilters = false)
		{
			IQueryable<T> query = _dbSet;

			if (!enableTracking) query = query.AsNoTracking();

			if (include != null) query = include(query);

			if (predicate != null) query = query.Where(predicate);

			if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

			return orderBy != null ? orderBy(query).FirstOrDefault() : query.FirstOrDefault();
		}
		#endregion

		#region Update Functions
		public void Update(T entity)
		{
			_dbSet.Update(entity);
		}

		public void Update(params T[] entities)
		{
			_dbSet.UpdateRange(entities);
		}

		public void Update(IEnumerable<T> entities)
		{
			_dbSet.UpdateRange(entities);
		}
		#endregion

		#region Table
		public IQueryable<T> Table => _dbSet;
		public IQueryable<T> TableNoTracking => _dbSet.AsNoTracking();
		#endregion
	}
}
