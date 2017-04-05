using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Apr.TeamFoundation.Tracking.Core.Ext.Tfs
{
	public static partial class WorkItemExtender
	{
		// WorkItem class

		public static void SetField(this WorkItem workItem, string fieldName, object value)
		{
			if (!workItem.Fields.Contains(fieldName)) return;
			workItem.Fields[fieldName].Value = value;
		}
		public static void SetField(this WorkItem workItem, CoreField fieldRef, object value)
		{
			if (!workItem.Fields.Contains((int)fieldRef)) return;
			workItem.Fields[fieldRef].Value = value;;
		}

		public static void SetField<T>(this WorkItem workItem, string fieldName, T value)
		{
			if (!workItem.Fields.Contains(fieldName)) return;
			workItem.Fields[fieldName].Value = value;
		}
		public static void SetField<T>(this WorkItem workItem, CoreField fieldRef, T value)
		{
			if (!workItem.Fields.Contains((int)fieldRef)) return;
			workItem.Fields[fieldRef].Value = value;;
		}


		public static object Field(this WorkItem workItem, string fieldName)
		{
			if (!workItem.Fields.Contains(fieldName)) return null;
			return workItem.Fields[fieldName].Value;
		}
		public static object Field(this WorkItem workItem, CoreField fieldRef)
		{
			if (!workItem.Fields.Contains((int)fieldRef)) return null;
			return workItem.Fields[fieldRef].Value;
		}

		public static object Field(this WorkItem workItem, string fieldName, Func<object, Field, object> checker)
		{
			if (!workItem.Fields.Contains(fieldName)) return null;
			return (checker == null) ? workItem.Fields[fieldName].Value : checker(workItem.Fields[fieldName].Value, workItem.Fields[fieldName]);
		}
		public static object Field(this WorkItem workItem, CoreField fieldRef, Func<object, Field, object> checker)
		{
			if (!workItem.Fields.Contains((int)fieldRef)) return null;
			return (checker == null) ? workItem.Fields[fieldRef].Value : checker(workItem.Fields[fieldRef].Value, workItem.Fields[fieldRef]);
		}

		public static T FieldOf<T>(this WorkItem workItem, string fieldName) // where T : class
		{
			object value = Field(workItem, fieldName);
			return ValueOf<T>(value);
		}
		public static T FieldOf<T>(this WorkItem workItem, CoreField fieldRef) // where T : class
		{
			object value = Field(workItem, fieldRef);
			return ValueOf<T>(value);
		}

		public static T Field<T>(this WorkItem workItem, string fieldName) // where T : class
		{
			object value = Field(workItem, fieldName);
			return ValueAs<T>(value);
		}
		public static T Field<T>(this WorkItem workItem, CoreField fieldRef) // where T : class
		{
			object value = Field(workItem, fieldRef);
			return ValueAs<T>(value);
		}


		public static string FieldAsString(this WorkItem workItem, string fieldName)
		{
			return (workItem.Field(fieldName) ?? string.Empty).ToString();
		}
		public static string FieldAsString(this WorkItem workItem, CoreField fieldRef)
		{
			return (workItem.Field(fieldRef) ?? string.Empty).ToString();
		}

		public static string FieldAsString(this WorkItem workItem, string fieldName, Func<object, Field, object> checker)
		{
			return (workItem.Field(fieldName, checker) ?? string.Empty).ToString();
		}
		public static string FieldAsString(this WorkItem workItem, CoreField fieldRef, Func<object, Field, object> checker)
		{
			return (workItem.Field(fieldRef, checker) ?? string.Empty).ToString();
		}


	}
}
