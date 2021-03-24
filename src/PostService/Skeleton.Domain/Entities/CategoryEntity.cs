using Common.Entities;
using System;
using System.Collections.Generic;

namespace Skeleton.Domain.Entities
{
	public class CategoryEntity : IBaseEntity<Guid>, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDelete { get; set; } = false;

        public CategoryEntity()
        {
            Post = new HashSet<PostEntity>();
        }
        public ICollection<PostEntity> Post { get; set; }
    }
}