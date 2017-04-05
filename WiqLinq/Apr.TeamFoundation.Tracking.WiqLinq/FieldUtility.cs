using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIT = Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Reflection;

namespace Apr.TeamFoundation.Tracking.Linq
{
	public static class FieldUtility
	{
		private static readonly Dictionary<string, string> fieldNames;

		static FieldUtility()
		{
			var coreFieldReferenceNames = from FieldInfo refField in typeof(WIT.CoreFieldReferenceNames).GetFields(BindingFlags.Static | BindingFlags.Public)
													join PropertyInfo witField in typeof(WIT.WorkItem).GetProperties(BindingFlags.Public | BindingFlags.Instance)
													on refField.Name equals witField.Name
													select new { Key = refField.Name, Value = refField.GetValue(null) };

			fieldNames = coreFieldReferenceNames.ToDictionary(
				item => item.Key, 
				item => item.Value as string, 
				StringComparer.OrdinalIgnoreCase);
		}

		internal static string ExtractFieldReferenceName(System.Reflection.MemberInfo memberInfo)
		{
			if (memberInfo.MemberType != System.Reflection.MemberTypes.Property)
				throw new NotImplementedException();
			if (memberInfo.DeclaringType.Name != "WorkItem")
				throw new NotImplementedException();
			return fieldNames.ContainsKey(memberInfo.Name) 
						? string.Format("[{0}]", fieldNames[memberInfo.Name])
						: memberInfo.Name;
		}

		internal static string ExtractWIFieldFromMethodCall(System.Linq.Expressions.MethodCallExpression mc)
		{
			throw new NotImplementedException();
		}
	}
}
