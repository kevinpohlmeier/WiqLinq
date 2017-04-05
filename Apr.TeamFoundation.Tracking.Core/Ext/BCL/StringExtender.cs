using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Apr.TeamFoundation.Tracking.Core.Ext.BCL
{
	public static class StringExtender
	{
		public static string Escape(this string input, Regex pattern, Func<string, string> changeMethod)
		{
			return pattern.Replace(input, new MatchEvaluator((Match match) => changeMethod(match.Value)));
		}
		public static string Escape(this string input, string pattern, Func<string, string> changeMethod)
		{
			return Regex.Replace(input, pattern, new MatchEvaluator((Match match) => changeMethod(match.Value)));
		}

		public static bool EqualsIgnoreCase(this string strA, string strB)
		{
			return (string.Compare(strA, strB, true) == 0);
		}
		public static bool NotEqualsIgnoreCase(this string strA, string strB)
		{
			return (string.Compare(strA, strB, true) != 0);
		}
	}
}
