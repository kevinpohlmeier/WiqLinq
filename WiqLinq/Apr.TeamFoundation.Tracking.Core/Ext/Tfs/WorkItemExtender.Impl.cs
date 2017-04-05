using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Apr.TeamFoundation.Tracking.Core.Ext.Tfs
{
	public static partial class WorkItemExtender
	{
		// Private Methods
		internal static T ValueOf<T>(object value)
		{
			T result = default(T);
			result = (value is T) ? (T)value : result;
			return result;
		}

		internal static T ValueAs<T>(object value)
		{
			T result = default(T);
			if (value is T)
				result = (T)value; // allowing nulls
			else if (typeof(T) == typeof(string))
			{
				result = (T)(object)((value ?? string.Empty).ToString());
			}
			else
			{
				try
				{
					result = (T)Convert.ChangeType(value, typeof(T));
				}
				catch (InvalidCastException) { }
				catch (ArgumentException) { }
			}
			return result;
		}
	}
}
