using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Apr.TeamFoundation.Tracking.Linq
{
	internal static class ExpressionHelper
	{
		public static Expression Unquote(Expression e)
		{
			while (e.NodeType == ExpressionType.Quote)
				e = ((UnaryExpression)e).Operand;
			return e;
		}

		public static T Unquote<T>(Expression e) where T : Expression
		{
			while (e.NodeType == ExpressionType.Quote)
				e = ((UnaryExpression)e).Operand;
			return e as T;
		}

		public static object Eval(Expression e)
		{
			LambdaExpression lambda = Expression.Lambda(e);
			Delegate fn = lambda.Compile();
			return fn.DynamicInvoke(null);
		}

		public static T Eval<T>(Expression e)
		{
			object result = Eval(e);
			if (result is T)
				return (T)(result);
			else
				return Activator.CreateInstance<T>();
		}
	}
}
