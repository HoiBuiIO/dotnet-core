using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
	public interface IUpdatedEntity
	{
		DateTime? UpdatedAt { get; set; }
		string UpdatedBy { get; set; }
	}
}
