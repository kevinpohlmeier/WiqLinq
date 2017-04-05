using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Apr.TeamFoundation.Tracking.Core.Ext.Tfs;

namespace Apr.TeamFoundation.Tracking.Core.LinqResolve
{
	public static partial class CoreWorkItemExtender
	{
		// WorkItem class
		public static object Field(this CoreWorkItem workItem, string fieldName)
		{
			if (!workItem.WorkItem.Fields.Contains(fieldName)) return null;
			return workItem.WorkItem.Fields[fieldName].Value;
		}
		public static object Field(this CoreWorkItem workItem, CoreField fieldRef)
		{
			if (!workItem.WorkItem.Fields.Contains((int)fieldRef)) return null;
			return workItem.WorkItem.Fields[fieldRef].Value;
		}

		public static object Field(this CoreWorkItem workItem, string fieldName, Func<object, Field, object> checker)
		{
			if (!workItem.WorkItem.Fields.Contains(fieldName)) return null;
			return (checker == null) ? workItem.WorkItem.Fields[fieldName].Value : checker(workItem.WorkItem.Fields[fieldName].Value, workItem.WorkItem.Fields[fieldName]);
		}
		public static object Field(this CoreWorkItem workItem, CoreField fieldRef, Func<object, Field, object> checker)
		{
			if (!workItem.WorkItem.Fields.Contains((int)fieldRef)) return null;
			return (checker == null) ? workItem.WorkItem.Fields[fieldRef].Value : checker(workItem.WorkItem.Fields[fieldRef].Value, workItem.WorkItem.Fields[fieldRef]);
		}


		public static T FieldOf<T>(this CoreWorkItem workItem, string fieldName) // where T : class
		{
			object value = Field(workItem, fieldName);
			return WorkItemExtender.ValueOf<T>(value);
		}
		public static T FieldOf<T>(this CoreWorkItem workItem, CoreField fieldRef) // where T : class
		{
			object value = Field(workItem, fieldRef);
			return WorkItemExtender.ValueOf<T>(value);
		}

		public static T Field<T>(this CoreWorkItem workItem, string fieldName) // where T : class
		{
			object value = Field(workItem, fieldName);
			return WorkItemExtender.ValueAs<T>(value);
		}
		public static T Field<T>(this CoreWorkItem workItem, CoreField fieldRef) // where T : class
		{
			object value = Field(workItem, fieldRef);
			return WorkItemExtender.ValueAs<T>(value);
		}


		public static string FieldAsString(this CoreWorkItem workItem, string fieldName)
		{
			return (workItem.Field(fieldName) ?? string.Empty).ToString();
		}
		public static string FieldAsString(this CoreWorkItem workItem, CoreField fieldRef)
		{
			return (workItem.Field(fieldRef) ?? string.Empty).ToString();
		}

		public static string FieldAsString(this CoreWorkItem workItem, string fieldName, Func<object, Field, object> checker)
		{
			return (workItem.Field(fieldName, checker) ?? string.Empty).ToString();
		}
		public static string FieldAsString(this CoreWorkItem workItem, CoreField fieldRef, Func<object, Field, object> checker)
		{
			return (workItem.Field(fieldRef, checker) ?? string.Empty).ToString();
		}
	}
}
