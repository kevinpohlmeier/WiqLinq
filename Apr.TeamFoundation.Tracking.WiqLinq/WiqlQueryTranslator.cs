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
	internal class WiqlQueryTranslator : ExpressionVisitor
	{
		private QueryBuilder _builder;

		internal WiqlQueryTranslator() { }

		internal WiqlQueryTranslationResult Translate(Expression expression, DateTime? asOf) //, bool p
		{
			_builder = new QueryBuilder();
			this.Visit(expression);
			return _builder.BuildQuery(asOf);
		}

		protected override Expression VisitMethodCall(MethodCallExpression m)
		{
			bool isQueryable = m.Method.DeclaringType == typeof(Queryable);

			if (isQueryable && m.Method.Name == "Where")
			{
				Visit(m.Arguments[0]);

				LambdaExpression lambda = ExpressionHelper.Unquote<LambdaExpression>(m.Arguments[1]);
				ProcessWhereClause(lambda);
				return m;
			}
			else if (isQueryable && m.Method.Name == "Select")
			{
				Visit(m.Arguments[0]);
				LambdaExpression lambda = ExpressionHelper.Unquote<LambdaExpression>(m.Arguments[1]);
				ProcessSelectClause(lambda);
				return m;
			}
			else if (isQueryable && m.Method.Name == "ThenBy")
			{
				Visit(m.Arguments[0]);
				LambdaExpression lambda = ExpressionHelper.Unquote<LambdaExpression>(m.Arguments[1]);
				ProcessOrderClause(lambda, true);
				return m;
			}
			else if (isQueryable && m.Method.Name == "ThenByDescending")
			{
				Visit(m.Arguments[0]);
				LambdaExpression lambda = ExpressionHelper.Unquote<LambdaExpression>(m.Arguments[1]);
				ProcessOrderClause(lambda, false);
				return m;
			}
			else if (isQueryable && m.Method.Name == "OrderBy")
			{
				LambdaExpression lambda = ExpressionHelper.Unquote<LambdaExpression>(m.Arguments[1]);
				ProcessOrderClause(lambda, true);
				Visit(m.Arguments[0]);
				return m;
			}
			else if (isQueryable && m.Method.Name == "OrderByDescending")
			{
				LambdaExpression lambda = ExpressionHelper.Unquote<LambdaExpression>(m.Arguments[1]);
				ProcessOrderClause(lambda, false);
				Visit(m.Arguments[0]);
				return m;
			}
			
			throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
		}

		private void ProcessSelectClause(LambdaExpression lambda)
		{
			MethodCallExpression mc = lambda.Body as MethodCallExpression;
			if (mc == null) return;
			if (mc.Method.Name != "Columns") return;
			if (mc.Arguments.Count != 2) return;
			var columns = ExpressionHelper.Eval<string[]>(mc.Arguments[1]);
			_builder.AddSelectFields(columns);
		}



		private bool IsAWorkItem(Type type)
		{
			if (type == typeof(WIT.WorkItem))
				return true;

			if (type.BaseType != null)
				return IsAWorkItem(type.BaseType);

			return false;
		}

		private void ProcessWhereClause(LambdaExpression lambda)
		{
			if ((lambda.Parameters.Count != 1) ||
				(!(lambda.Parameters[0] is ParameterExpression)) ||
				(!IsAWorkItem(lambda.Parameters[0].Type)))
			{
				throw new InvalidOperationException("invalid where clase");
			}

			WhereClauseTranslator whereTranslator = new WhereClauseTranslator();

			string whereClause = whereTranslator.Translate(lambda.Body, _builder);
			_builder.AddWhereClause(whereClause);
		}

		private void ProcessOrderClause(LambdaExpression lambda, bool isAscending)
		{
			if ((lambda.Parameters.Count != 1) ||
				 (!(lambda.Parameters[0] is ParameterExpression)) ||
				 (!IsAWorkItem(lambda.Parameters[0].Type)))
			{
				throw new InvalidOperationException("invalid order clase");
			}

			Expression body = lambda.Body;
			MemberExpression me = body as MemberExpression;
			string fieldReference = null;
			if (me != null)
			{
				fieldReference = FieldUtility.ExtractFieldReferenceName(me.Member);
				if (fieldReference == null)
				{
					throw new InvalidOperationException("invalid order field");
				}
				_builder.AddOrderClause(fieldReference, isAscending);
				return;
			}

			MethodCallExpression mc = body as MethodCallExpression;
			if (mc != null)
			{
				fieldReference = FieldUtility.ExtractWIFieldFromMethodCall(mc);
				if (fieldReference == null)
				{
					throw new InvalidOperationException("invalid order field");
				}
				_builder.AddOrderClause(fieldReference, isAscending);
			}
		}
	}
}