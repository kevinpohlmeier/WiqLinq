using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFS = Microsoft.TeamFoundation.Client;

namespace Apr.TeamFoundation.Tracking.Linq
{
	public static class WorkItemStoreExtender
	{
		public static IQueryableWorkitemStore WorkItems(this WIT.WorkItemStore store)
		{
			return new QueryableWorkItemStore(new WorkItemQueryProvider(store));
		}
		public static IQueryableWorkitemStore WorkItems(this WIT.WorkItemStore store, DateTime asof)
		{
			return new QueryableWorkItemStore(new WorkItemQueryProvider(store) { AsOf = asof });
		}
		public static IQueryableWorkitemStore WorkItems(this TFS.TfsTeamProjectCollection server)
		{
			WIT.WorkItemStore store = new WIT.WorkItemStore(server);
			return new QueryableWorkItemStore(new WorkItemQueryProvider(store));
		}
		public static IQueryableWorkitemStore WorkItems(this TFS.TfsTeamProjectCollection server, DateTime asof)
		{
			WIT.WorkItemStore store = new WIT.WorkItemStore(server);
			return new QueryableWorkItemStore(new WorkItemQueryProvider(store) { AsOf = asof });
		}
	}
}
