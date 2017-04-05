using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Apr.TeamFoundation.Tracking.Linq
{
	public static class QueryOperators
	{
		public static readonly Dictionary<string, string> Operators = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "Under", "UNDER" },
			{ "NotUnder", "NOT UNDER" },
			{ "In", "IN" },
			{ "Ever", "EVER" },
			{ "NotEver", "NOT EVER" },
			{ "Contains", "CONTAINS" }
		};
	}
}
