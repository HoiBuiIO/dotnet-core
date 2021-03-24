﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
	public interface ICreatedEntity
	{
		DateTime? CreatedAt { get; set; }
		string CreatedBy { get; set; }
	}
}
