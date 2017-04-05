using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Apr.TeamFoundation.Tracking.Core.Ext.Tfs
{
	public static partial class WorkItemExtender
	{
		// Revision

		public static object Field(this Revision workItem, string fieldName)
		{
			if (!workItem.Fields.Contains(fieldName)) return null;
			return workItem.Fields[fieldName].Value;
		}
		public static object Field(this Revision workItem, CoreField fieldRef)
		{
			if (!workItem.Fields.Contains((int)fieldRef)) return null;
			return workItem.Fields[fieldRef].Value;
		}

		public static object Field(this Revision workItem, string fieldName, Func<object, Field, object> checker)
		{
			if (!workItem.Fields.Contains(fieldName)) return null;
			return (checker == null) ? workItem.Fields[fieldName].Value : checker(workItem.Fields[fieldName].Value, workItem.Fields[fieldName]);
		}
		public static object Field(this Revision workItem, CoreField fieldRef, Func<object, Field, object> checker)
		{
			if (!workItem.Fields.Contains((int)fieldRef)) return null;
			return (checker == null) ? workItem.Fields[fieldRef].Value : checker(workItem.Fields[fieldRef].Value, workItem.Fields[fieldRef]);
		}


		public static T FieldOf<T>(this Revision workItem, string fieldName) // where T : class
		{
			object value = Field(workItem, fieldName);
			return ValueOf<T>(value);
		}
		public static T FieldOf<T>(this Revision workItem, CoreField fieldRef) // where T : class
		{
			object value = Field(workItem, fieldRef);
			return ValueOf<T>(value);
		}

		public static T Field<T>(this Revision workItem, string fieldName) // where T : class
		{
			object value = Field(workItem, fieldName);
			return ValueAs<T>(value);
		}
		public static T Field<T>(this Revision workItem, CoreField fieldRef) // where T : class
		{
			object value = Field(workItem, fieldRef);
			return ValueAs<T>(value);
		}


		public static string FieldAsString(this Revision workItem, string fieldName)
		{
			return (workItem.Field(fieldName) ?? string.Empty).ToString();
		}
		public static string FieldAsString(this Revision workItem, CoreField fieldRef)
		{
			return (workItem.Field(fieldRef) ?? string.Empty).ToString();
		}

		public static string FieldAsString(this Revision workItem, string fieldName, Func<object, Field, object> checker)
		{
			return (workItem.Field(fieldName, checker) ?? string.Empty).ToString();
		}
		public static string FieldAsString(this Revision workItem, CoreField fieldRef, Func<object, Field, object> checker)
		{
			return (workItem.Field(fieldRef, checker) ?? string.Empty).ToString();
		}
	}
}
