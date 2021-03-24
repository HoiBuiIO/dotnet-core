using Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Skeleton.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Data.Repository
{
	public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>
	   where TContext : DbContext, IDisposable
	{
		public TContext Context { get; }
		private readonly IHttpContextAccessor _httpContextAccessor;

		#region Constructor
		public UnitOfWork(
			TContext context,
			IHttpContextAccessor httpContextAccessor = null)
		{
			Context = context ?? throw new ArgumentNullException(nameof(DbContext));
			_httpContextAccessor = httpContextAccessor;
		}
		#endregion

		public int Commit()
		{
			TrackChanges();
			return Context.SaveChanges();
		}

		public async Task<int> CommitAsync()
		{
			TrackChanges();
			return await Context.SaveChangesAsync();
		}

		public void Dispose()
		{
			Context?.Dispose();
		}

		public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
		{
			return (IRepository<TEntity>)GetOrAddRepository(typeof(TEntity), new Repository<TEntity>(Context));
		}

		public object GetOrAddRepository(Type type, object repo)
		{
			_repositories ??= new Dictionary<(Type type, string Name), object>();

			if (_repositories.TryGetValue((type, repo.GetType().FullName), out var repository)) return repository;
			_repositories.Add((type, repo.GetType().FullName), repo);
			return repo;
		}

		#region Private Field
		private Dictionary<(Type type, string name), object> _repositories;
		#endregion

		#region Private Methods
		private void TrackChanges()
		{
			var validationErrors = Context.ChangeTracker
				.Entries<IValidatableObject>()
				.SelectMany(e => e.Entity.Validate(null))
				.Where(r => r != ValidationResult.Success)
				.ToArray();

			if (validationErrors.Any())
			{
				var exceptionMessage = string.Join(Environment.NewLine, validationErrors.Select(error =>
					$"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
				throw new Exception(exceptionMessage);
			}

			foreach (var entry in Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
			{
				if (!(entry?.Entity is ICreatedEntity createdEntity)) continue;

				//createdEntity.CreatedBy = ""];
				createdEntity.CreatedAt = DateTime.UtcNow;
			}

			foreach (var entry in Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
			{
				if (!(entry?.Entity is IUpdatedEntity updatedEntity)) continue;

				//updatedEntity.UpdatedBy = "";
				updatedEntity.UpdatedAt = DateTime.UtcNow;
			}
		}
		#endregion
	}
}