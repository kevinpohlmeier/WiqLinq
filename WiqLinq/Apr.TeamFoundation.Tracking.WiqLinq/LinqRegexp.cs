using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

namespace Apr.TeamFoundation.Tracking.Linq
{
	internal static class LinqRegexp
	{
		private static Regex fieldNameReference;
		private static Regex FieldNameReference{
			get
			{
				if (fieldNameReference == null)
				{
					fieldNameReference = new Regex(@"\.get_Item\(\""(?<RefName>[A-Za-z.]+)\""\)\.", RegexOptions.Compiled);
				}
				return fieldNameReference;
			}
		}

		public static bool TryGetFieldRefNameFromExpression(Expression expression, out string fieldRefName)
		{
			string result = null;
			string input = expression.ToString();

			var match = FieldNameReference.Match(input);
			var refName = match.Groups["RefName"];
			if (refName.Success)
				result = refName.Value;

			fieldRefName = result;
			return !string.IsNullOrEmpty(fieldRefName);
		}
	}
}
