using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Collections;

namespace Apr.TeamFoundation.Tracking.Linq
{
	public static class WorkItemCollectionExtender
	{
		public static IEnumerator<T> GetEnumerator<T>(this WIT.WorkItemCollection workItemCollection)
		{
			return new WorkItemCollectionEnumerator<T>(workItemCollection);
		}

		private class WorkItemCollectionEnumerator<T> : IEnumerator<T>
		{
			private IEnumerator workItemCollectionEnumerator = null;
			private bool isDisposed = false;

			internal WorkItemCollectionEnumerator(WIT.WorkItemCollection collection)
			{
				if (typeof(T) != typeof(WIT.WorkItem))
					throw new NotImplementedException();
				workItemCollectionEnumerator = collection.GetEnumerator();
			}			

			//region IEnumerator<T> Members
			public T Current
			{
				get { return (T)workItemCollectionEnumerator.Current; }
			}

			//region IDisposable Members
			public void Dispose()
			{
				if (isDisposed) return;
				workItemCollectionEnumerator = null;
				isDisposed = true;
			}

			//region IEnumerator Members
			object System.Collections.IEnumerator.Current
			{
				get { return workItemCollectionEnumerator.Current; }
			}

			public bool MoveNext()
			{
				return workItemCollectionEnumerator.MoveNext();
			}

			public void Reset()
			{
				workItemCollectionEnumerator.Reset();
			}
		}
	}
}
