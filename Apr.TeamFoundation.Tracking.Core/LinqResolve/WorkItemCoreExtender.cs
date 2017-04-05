using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Apr.TeamFoundation.Tracking.Core.LinqResolve
{
	public static class WorkItemCoreExtender
	{
		public static CoreWorkItem Core(this WorkItem workItem)
		{
			return new CoreWorkItem() { WorkItem = workItem };
		}
	}
}
