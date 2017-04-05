using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Apr.TeamFoundation.Tracking.Linq
{
	public class WorkItemEnumerator : IEnumerator<WIT.WorkItem>
	{
		private WIT.WorkItemCollection collection;
		public WorkItemEnumerator(WIT.WorkItemCollection collection)
		{
			this.collection = collection;
		}

		//region IEnumerator<WorkItem> Members

		public WIT.WorkItem Current
		{
			get { throw new NotImplementedException(); }
		}

		//region IDisposable Members

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		//region IEnumerator Members

		object System.Collections.IEnumerator.Current
		{
			get { throw new NotImplementedException(); }
		}

		public bool MoveNext()
		{
			throw new NotImplementedException();
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}
	}
}
