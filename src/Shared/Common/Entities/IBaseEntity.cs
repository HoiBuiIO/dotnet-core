using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
	/// <summary>
	/// Entity base
	/// </summary>
	/// <typeparam name="T">Generic type</typeparam>
	public interface IBaseEntity<T>
	{
		public T Id { get; set; }
	}
}
