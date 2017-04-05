using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Apr.TeamFoundation.Tracking.Linq
{
	public class WorkItemQueryProvider : IQueryProvider
	{
		private static readonly Func<WIT.WorkItemCollection, WIT.WorkItem> FirstSelector = items => items.Count == 0 ? null : items[0];

		private static readonly Func<WIT.WorkItemCollection, WIT.WorkItem> LastSelector = items => items.Count == 0 ? null : items[items.Count - 1];

		private static readonly Func<WIT.WorkItemCollection, int> CountSelector = items => items.Count;

		private WIT.WorkItemStore Store { get; set; }

		public DateTime? AsOf { get; set; }

		private string queryText;
		private int lastKnownHash = 0;

		private WorkItemQueryProvider(WorkItemQueryProvider copy)
		{
			this.AsOf = copy.AsOf;
			this.Store = copy.Store;
		}

		public WorkItemQueryProvider(WIT.WorkItemStore store)
		{
			Store = store;
		}

		public string GetQueryText(Expression expression)
		{
			bool needTranslate = string.IsNullOrEmpty(queryText) || lastKnownHash != expression.GetHashCode();
			if (needTranslate)
			{
				queryText = this.Translate(expression);
			}
			return queryText;
		}

		//region IQueryProvider Members

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			if (typeof(TElement) != typeof(WIT.WorkItem))
				throw new NotImplementedException();

			return new QueryableWorkItemStore(new WorkItemQueryProvider(this), expression) as IQueryable<TElement>;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			throw new NotImplementedException();
		}

		public TResult Execute<TResult>(Expression expression)
		{
			if (typeof(TResult) == typeof(WIT.WorkItemCollection))
				return (TResult)InternalExecute(expression);
			else if (typeof(TResult) == typeof(WIT.WorkItem))
				return (TResult)InternalSingleItemExecute(expression);

			throw new NotImplementedException();
		}

		private object InternalSingleItemExecute(Expression expression)
		{
			if (expression.NodeType != ExpressionType.Call)
				throw new NotImplementedException();

			MethodCallExpression mc = (MethodCallExpression)expression;
			string methodName = mc.Method.Name;

			Func<WIT.WorkItemCollection, WIT.WorkItem> selector = null;
			switch (methodName)
			{
				case "First":
					selector = FirstSelector;
					break;
				case "Last":
					selector = LastSelector;
					break;
				//case "Count":
				//   selector = CountSelector;
				default:
					throw new NotImplementedException();
			}
			return InternalExecute(mc.Arguments[0], selector);
		}

		public object Execute(Expression expression)
		{
			return InternalExecute(expression);
		}

		private object InternalExecute(Expression expression)
		{
			return InternalExecute(expression, null);
		}

		private object InternalExecute(Expression expression, Func<WIT.WorkItemCollection, WIT.WorkItem> selector)
		{
			var translator = new WiqlQueryTranslator();
			WiqlQueryTranslationResult result = translator.Translate(expression, AsOf);

			if (selector == null)
				return Store.Query(result.Wiql, result.Context);
			else
				return selector(Store.Query(result.Wiql, result.Context));
		}

		private string Translate(Expression expression)
		{
			var translator = new WiqlQueryTranslator();
			WiqlQueryTranslationResult result = translator.Translate(expression, AsOf);
			return result.ToString();
		}
	}

}
