using System;
using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Linq;

namespace Apr.TeamFoundation.Tracking.Linq
{
	public interface IQueryableWorkitemStore : IQueryable<WIT.WorkItem>, IOrderedQueryable<WIT.WorkItem>
	{
		WIT.WorkItemCollection Execute();
	}
}
