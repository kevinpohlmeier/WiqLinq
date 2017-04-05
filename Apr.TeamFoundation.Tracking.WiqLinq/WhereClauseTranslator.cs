using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;
//using Apr.TeamFoundation.Tracking.Core.Ext.Tfs;

namespace Apr.TeamFoundation.Tracking.Linq
{
	internal enum WhereLocationEnum { LeftOperatorClause, RightOperatorClause, ElseWhere }

	internal class WhereClauseTranslator : ExpressionVisitor
	{

		private StringBuilder _builder;
		private QueryBuilder _queryBuilder;
		private Stack<WhereLocationEnum> _locationStack;

		private WhereLocationEnum CurrentLocation
		{
			get
			{
				if (_locationStack.Count == 0)
				{
					return WhereLocationEnum.ElseWhere;
				}
				return _locationStack.Peek();
			}
		}

		private void PushLocation(WhereLocationEnum location)
		{
			_locationStack.Push(location);
		}

		private void PopLocation()
		{
			_locationStack.Pop();
		}


		internal WhereClauseTranslator()
		{
			_locationStack = new Stack<WhereLocationEnum>();
		}

		internal string Translate(Expression expression, QueryBuilder queryBuilder)
		{
			expression = Evaluator.PartialEval(expression);
			_builder = new StringBuilder();
			_queryBuilder = queryBuilder;

			this.Visit(expression);

			return _builder.ToString();
		}

		private static Expression StripQuotes(Expression e)
		{
			while (e.NodeType == ExpressionType.Quote)
				e = ((UnaryExpression)e).Operand;
			return e;
		}

		protected override Expression VisitUnary(UnaryExpression u)
		{
			// throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));

			switch (u.NodeType)
			{
				case ExpressionType.Not:
					_builder.Append(" NOT ");
					this.Visit(u.Operand);
					break;
				//case ExpressionType.
				default:
					throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));

			}
			return u;
		}

		protected override Expression VisitParameter(ParameterExpression p)
		{
			return base.VisitParameter(p);
		}

		protected override Expression VisitBinary(BinaryExpression b)
		{
			_builder.Append("(");

			PushLocation(WhereLocationEnum.LeftOperatorClause);
			this.Visit(b.Left);
			PopLocation();

			switch (b.NodeType)
			{
				case ExpressionType.And:
				case ExpressionType.AndAlso:
					_builder.Append(" AND ");
					break;

				case ExpressionType.Or:
				case ExpressionType.OrElse:
					_builder.Append(" OR ");
					break;

				case ExpressionType.Equal:
					_builder.Append(" = ");
					break;

				case ExpressionType.NotEqual:
					_builder.Append(" <> ");
					break;

				case ExpressionType.LessThan:
					_builder.Append(" < ");
					break;

				case ExpressionType.LessThanOrEqual:
					_builder.Append(" <= ");
					break;

				case ExpressionType.GreaterThan:
					_builder.Append(" > ");
					break;

				case ExpressionType.GreaterThanOrEqual:
					_builder.Append(" >= ");
					break;

				//case ExpressionType.Modulo:
				//case ExpressionType.RightShift:
				//   _builder.Append(" UNDER ");
				//   break;

				//case ExpressionType.ExclusiveOr:
				//   _builder.Append(" EVER ");
				//   break;

				default:
					throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
			}

			PushLocation(WhereLocationEnum.RightOperatorClause);
			this.Visit(b.Right);
			PopLocation();

			_builder.Append(")");
			return b;
		}

		protected override Expression VisitConstant(ConstantExpression c)
		{
			if (CurrentLocation != WhereLocationEnum.RightOperatorClause)
				throw new NotSupportedException("constant location not supported");

			string macro = _queryBuilder.GenerateMacro(c.Value);
			
			_builder.Append(macro);
			
			return c;
		}

		protected override Expression VisitMemberAccess(MemberExpression m)
		{
			ParameterExpression param = m.Expression as ParameterExpression;

			if (param != null)
			{
				if (CurrentLocation != WhereLocationEnum.LeftOperatorClause)
					throw new NotSupportedException("Member position not supported");

				string fieldRefName = FieldUtility.ExtractFieldReferenceName(m.Member);

				if (fieldRefName != null)
				{
					_builder.Append(fieldRefName);
					return m;
				}
			}

			if (m.Member.DeclaringType == typeof(QueryConstant))
			{
				if (CurrentLocation != WhereLocationEnum.RightOperatorClause)
					throw new NotSupportedException("Member position not supported");

				_builder.Append("@" + m.Member.Name);
				return m;
			}

			if ((m.Member.DeclaringType == typeof(WIT.Field))
				&& (m.Member.Name == "Value"))
			{
				string fieldRefName;
				if (LinqRegexp.TryGetFieldRefNameFromExpression(m, out fieldRefName))
				{
					_builder.AppendFormat("[{0}]", fieldRefName);
					return m;
				}
			}

			throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
		}

		protected override Expression VisitMethodCall(MethodCallExpression m)
		{
			if ((m.Object is ParameterExpression) && (m.Object.Type == typeof(WIT.WorkItem)))
			{
				if (CurrentLocation != WhereLocationEnum.LeftOperatorClause)
				{
					throw new InvalidOperationException("Methodcall");
				}
				string fieldReference = FieldUtility.ExtractWIFieldFromMethodCall(m);

				if (fieldReference == null)
				{
					throw new NotSupportedException("MethodCall");
				}
				_builder.Append(fieldReference);
			}
			else if (CheckCustomOperatorCalls(m))
			{
				return m;
			}
			else if (CheckCustomStaticOperatorCalls(m))
			{
				return m;
			}
			//else if (CheckFieldExtensionMethodCall(m))
			//{
			//   return m;
			//}
			else
			{
				MemberExpression me = m.Object as MemberExpression;
				if ((me == null) ||
					 (!(me.Expression is ParameterExpression)) ||
					 (m.Arguments.Count != 1))
				{
					throw new NotSupportedException("MethodCall");
				}
				else
				{
					ConstantExpression constant = m.Arguments[0] as ConstantExpression;

					string op = null;
					if (me.Type == typeof(string))
					{
						if (m.Method.Name == "Contains")
						{
							op = "CONTAINS";
						}
						else if (m.Method.Name == "StartsWith")
						{
							op = "UNDER";
						}
					}

					if (op == null)
					{
						throw new NotSupportedException("MethodCall");
					}
					PushLocation(WhereLocationEnum.LeftOperatorClause);
					Visit(me);
					PopLocation();
					_builder.Append(" " + op + " ");

					PushLocation(WhereLocationEnum.RightOperatorClause);
					Visit(m.Arguments[0]);
					PopLocation();
				}
			}
			return m;
		}

		private bool CheckCustomStaticOperatorCalls(MethodCallExpression m)
		{
			if (m.Object != null) return false;
			if (m.Arguments.Count != 2) return false;
			if (m.Arguments[0].Type != typeof(WIT.WorkItem)) return false;
			if (m.Arguments[1].Type != typeof(string)) return false;
			if ((m.Method.ReturnType != typeof(LinqFieldExtender)) &&
				(m.Method.ReturnType.GetGenericTypeDefinition() != typeof(LinqFieldExtender<>))) return false;
			if (m.Method.Name != "Field") return false;

			string fieldReference;
			if (TryGetFieldReference(m.Arguments[1], out fieldReference))
			{
				_builder.AppendFormat("[{0}]", fieldReference);
			}
			return true;
		}

		private bool TryGetFieldReference(Expression e, out string fieldRefName)
		{
			fieldRefName = null;
			ConstantExpression ce = e as ConstantExpression;
			if (ce == null) return false;
			if (ce.Type != typeof(string)) return false;
			fieldRefName = (string)ce.Value;
			return true;
		}

		private bool CheckCustomOperatorCalls(MethodCallExpression m)
		{
			MethodCallExpression @object = m.Object as MethodCallExpression;
			if (@object == null) return false;

			if (@object.Method.Name != "Field") return false;
			if (@object.Arguments.Count != 2) return false;
			if (@object.Arguments[0].Type != typeof(WIT.WorkItem)) return false;
			if (@object.Arguments[1].Type != typeof(string)) return false;

			string op = null;

			if (m.Method.ReturnType == typeof(bool))
			{
				if (QueryOperators.Operators.ContainsKey(m.Method.Name))
					op = QueryOperators.Operators[m.Method.Name];
			}

			if (op == null)
			{
				throw new NotSupportedException("MethodCall");
			}

			string fieldReference;
			_builder.Append("(");
			if (TryGetFieldReference(@object.Arguments[1], out fieldReference))
			{
				_builder.AppendFormat("[{0}]", fieldReference);
			}
			else
			{
				PushLocation(WhereLocationEnum.LeftOperatorClause);
				Visit(@object.Arguments[1]);
				PopLocation();
			}

			_builder.Append(" " + op + " ");

			PushLocation(WhereLocationEnum.RightOperatorClause);
			Visit(m.Arguments[0]);
			PopLocation();
			_builder.Append(")");

			return true;
		}

		#region Not Supported Expressions
		protected override MemberBinding VisitBinding(MemberBinding binding)
		{
			throw new NotSupportedException("MemberBinding");
		}

		protected override ElementInit VisitElementInitializer(ElementInit initializer)
		{
			throw new NotSupportedException("ElementInit");
		}

		protected override Expression VisitConditional(ConditionalExpression c)
		{
			throw new NotSupportedException("ConditionalExpression");
		}

		protected override IEnumerable<MemberBinding> VisitBindingList(System.Collections.ObjectModel.ReadOnlyCollection<MemberBinding> original)
		{
			throw new NotSupportedException("BindingList");
		}

		protected override IEnumerable<ElementInit> VisitElementInitializerList(System.Collections.ObjectModel.ReadOnlyCollection<ElementInit> original)
		{
			throw new NotSupportedException("ElementInitializerList");
		}

		protected override System.Collections.ObjectModel.ReadOnlyCollection<Expression> VisitExpressionList(System.Collections.ObjectModel.ReadOnlyCollection<Expression> original)
		{
			throw new NotSupportedException("ExpressionList");
		}

		protected override Expression VisitInvocation(InvocationExpression iv)
		{
			throw new NotSupportedException("Invocation");
		}

		protected override Expression VisitLambda(LambdaExpression lambda)
		{
			throw new NotSupportedException("Lambda");
		}

		protected override Expression VisitListInit(ListInitExpression init)
		{
			throw new NotSupportedException("ListInit");
		}

		protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
		{
			throw new NotSupportedException("MemberAssignment");
		}

		protected override Expression VisitMemberInit(MemberInitExpression init)
		{
			throw new NotSupportedException("MemberInit");
		}

		protected override MemberListBinding VisitMemberListBinding(MemberListBinding binding)
		{
			throw new NotSupportedException("MemberListBinding");
		}

		protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
		{
			throw new NotSupportedException("MemberMemberBinding");
		}

		protected override NewExpression VisitNew(NewExpression nex)
		{
			throw new NotSupportedException("New");
		}

		protected override Expression VisitNewArray(NewArrayExpression na)
		{
			throw new NotSupportedException("NewArray");
		}

		protected override Expression VisitTypeIs(TypeBinaryExpression b)
		{
			throw new NotSupportedException("TypeIs");
		}
		#endregion

	}

}