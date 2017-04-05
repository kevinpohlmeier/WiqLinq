using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;


namespace Apr.TeamFoundation.Tracking.Linq
{
	public static class WorkItemLinqExtender
	{
		public static LinqFieldExtender Field(this WIT.WorkItem item, string fieldName)
		{
			return new LinqFieldExtender();
		}

		public static LinqFieldExtender<T> Field<T>(this WIT.WorkItem item, string fieldName)
		{
			return new LinqFieldExtender<T>();
		}

		public static WIT.WorkItem Columns(this WIT.WorkItem item, params string[] fieldNames)
		{
			return item;
		}
	}
}
