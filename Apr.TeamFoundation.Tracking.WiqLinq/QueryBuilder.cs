using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Apr.TeamFoundation.Tracking.Linq
{
	/// <summary>
	/// Class used to build the query
	/// </summary>
	internal class QueryBuilder
	{
		private const string MACROFORMAT = "P{0}";
		private const string DEFAULTCOLUMN = "[System.Id]";
		private List<string> _selectFieldList = null;
		private List<string> _whereList = new List<string>();
		private List<string> _orderbyList = new List<string>();
		private int _macroIndex = 0;
		private WiqlQueryTranslationResult _query;

		private Dictionary<string, object> _macroDictionnary = new Dictionary<string, object>();

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryBuilder"/> class.
		/// </summary>
		public QueryBuilder()
		{
		}

		/// <summary>
		/// Adds a column to select clause in the query
		/// </summary>
		/// <param name="whereClause">The where clause.</param>
		public void AddSelectField(string selectField)
		{
			if (string.IsNullOrEmpty(selectField)) return;
			if (_selectFieldList == null)
				_selectFieldList = new List<string>();
			_selectFieldList.Add(selectField);
		}
	
		/// <summary>
		/// Adds several columns to select clause in the query
		/// </summary>
		/// <param name="whereClause">The where clause.</param>
		public void AddSelectFields(params string[] selectFields)
		{
			if ((selectFields == null) || (selectFields.Length == 0)) return;
			if (_selectFieldList == null)
				_selectFieldList = new List<string>();
			_selectFieldList.AddRange(selectFields);
		}

		/// <summary>
		/// Adds a where clause in the query
		/// </summary>
		/// <param name="whereClause">The where clause.</param>
		public void AddWhereClause(string whereClause)
		{
			_whereList.Add(whereClause);
		}
		/// <summary>
		/// Adds an order clause in the query
		/// </summary>
		/// <param name="field">The field.</param>
		/// <param name="isAscending">if set to <c>true</c> [is ascending].</param>
		public void AddOrderClause(string field, bool isAscending)
		{
			_orderbyList.Add(field + (isAscending ? " asc" : " desc"));
		}
		/// <summary>
		/// Generates a variable name and associate the given value
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public string GenerateMacro(object value)
		{
			string macroName = String.Format(MACROFORMAT, _macroIndex);
			_macroIndex++;
			_macroDictionnary.Add(macroName, value);
			return "@" + macroName;
		}

		/// <summary>
		/// Builds the query.
		/// </summary>
		/// <param name="server">The server.</param>
		/// <param name="project">The project.</param>
		/// <param name="asOf">As of.</param>
		/// <returns></returns>
		public WiqlQueryTranslationResult BuildQuery(DateTime? asOf) //Microsoft.TeamFoundation.Client.TeamFoundationServer server, Microsoft.TeamFoundation.WorkItemTracking.Client.Project project, 
		{
			if (_query != null)
			{
				return _query;
			}
			StringBuilder builder = new StringBuilder();
			builder.Append("SELECT ");
			if (_selectFieldList == null)
			{
				builder.Append(DEFAULTCOLUMN);
			}
			else
			{
				builder.AppendFormat("[{0}]", String.Join("], [", _selectFieldList.ToArray()));
			}
			builder.AppendLine();
			builder.AppendLine("FROM WORKITEMS ");
			if (_whereList.Count > 0)
			{
				builder.Append("WHERE ");
				if (_whereList.Count == 1)
				{
					builder.AppendLine(_whereList[0]);
				}
				else
				{
					builder.AppendFormat("({0})", String.Join(") AND (", _whereList.ToArray()));
					builder.AppendLine();
				}

				//builder.Append("(");
				//builder.Append(String.Join(") AND (", _whereList.ToArray()));
				//builder.AppendLine(")");
			}
			if (_orderbyList.Count > 0)
			{
				builder.Append("ORDER BY ");
				builder.AppendLine(String.Join(", ", _orderbyList.ToArray()));
			}
			if (asOf.HasValue)
			{
				builder.AppendFormat(" ASOF {0}", GenerateMacro(asOf.Value));
			}
			string wiql = builder.ToString();

			_query = new WiqlQueryTranslationResult
			{
				Wiql = wiql,
				Context = _macroDictionnary
			};
			//_query = new WorkItemQuery(server, project, "LINQ Query", wiql, _macroDictionnary);
			
			return _query;
		}
		///// <summary>
		///// Gets or sets the select method. This is used to generate the final list of work item
		///// </summary>
		///// <value>The select method.</value>
		//public MethodInfo SelectMethod
		//{
		//   get;
		//   set;
		//}

		/// <summary>
		/// Gets or sets the select lambda. This is used to generate the final list of work item
		/// </summary>
		/// <value>The select lambda.</value>
		public LambdaExpression SelectLambda
		{
			get;
			set;
		}
		/// <summary>
		/// Gets the query.
		/// </summary>
		/// <value>The query.</value>
		public WiqlQueryTranslationResult Query
		{
			get
			{
				return _query;
			}
		}

	}
}
