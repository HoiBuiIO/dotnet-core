using Microsoft.EntityFrameworkCore;
using Skeleton.Domain.Entities;

namespace Skeleton.Data.EntityFramework
{
	public class ApplicationReadWriteDbContext : DbContext, IApplicationDbContext
	{
		public ApplicationReadWriteDbContext(DbContextOptions<ApplicationReadWriteDbContext> options) : base(options)
		{

		}

		public virtual DbSet<CategoryEntity> Categories { get; set; }
		public virtual DbSet<PostEntity> Posts { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PostEntity>(entity =>
			{
				entity.ToTable("Post");
				entity.HasOne(n => n.Category)
				.WithMany(n => n.Post)
				.HasForeignKey(n => n.Id)
				.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<CategoryEntity>(entity =>
			{
				entity.ToTable("Category");
			});
		}
	}
}