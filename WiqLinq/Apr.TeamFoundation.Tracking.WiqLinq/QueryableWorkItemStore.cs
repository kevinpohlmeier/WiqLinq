using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Linq.Expressions;
using System.Collections;

namespace Apr.TeamFoundation.Tracking.Linq
{
	internal class QueryableWorkItemStore : IQueryableWorkitemStore // IQueryable<WIT.WorkItem>, IOrderedQueryable<WIT.WorkItem>
	{
		private IQueryProvider provider;
		private Expression expression;

		/// <summary>
		/// Creates source for initial expression, i.e. From
		/// </summary>
		/// <param name="provider"></param>
		internal QueryableWorkItemStore(WorkItemQueryProvider provider)
		{
			this.provider = provider;
			this.expression = Expression.Constant(this);
		}

		/// <summary>
		/// Creates sources for consequitive expressions .Where, .OrderBy, etc.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="expression"></param>
		internal QueryableWorkItemStore(WorkItemQueryProvider provider, Expression expression)
		{
			this.provider = provider;
			this.expression = expression;
		}

		//region IEnumerable<WorkItem> Members
		public IEnumerator<WIT.WorkItem> GetEnumerator()
		{
			return ((WIT.WorkItemCollection)(this.provider.Execute(this.expression))).GetEnumerator<WIT.WorkItem>();
		}

		//region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
		}

		//region IQueryable Members

		public Type ElementType
		{
			get { return typeof(WIT.WorkItem); }
		}

		public Expression Expression
		{
			get { return expression; }
			internal set { expression = value; }
		}

		public IQueryProvider Provider
		{
			get { return provider; }
		}

		public override string ToString()
		{
			return ((WorkItemQueryProvider)provider).GetQueryText(expression);
		}

		#region IQueryableWorkitemStore Members

		public WIT.WorkItemCollection Execute()
		{
			return ((WorkItemQueryProvider)provider).Execute<WIT.WorkItemCollection>(expression);
		}

		#endregion
	}
}
