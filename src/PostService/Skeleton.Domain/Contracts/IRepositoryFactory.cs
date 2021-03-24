using System;
using System.Collections.Generic;
using System.Text;

namespace Skeleton.Domain.Contracts
{
	public interface IRepositoryFactory
	{
		IRepository<T> GetRepository<T>() where T : class;
	}
}