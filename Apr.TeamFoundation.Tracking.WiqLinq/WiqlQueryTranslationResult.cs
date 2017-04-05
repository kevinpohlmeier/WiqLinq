using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Apr.TeamFoundation.Tracking.Linq
{
	class WiqlQueryTranslationResult
	{
		public string Wiql { get; set; }
		public Dictionary<string, object> Context { get; set; }

		public WiqlQueryTranslationResult()
		{
			Wiql = string.Empty;
			Context = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		}

		public override string ToString()
		{
			Func<object, Type, string> conversion = (obj, rtype) =>
			{
				if (rtype == typeof(string))
					return string.Format("'{0}'", ((string)obj).Replace("'", "''"));
				else
					return obj.ToString().Replace("'", "''");
			};

			return Regex.Replace(Wiql, @"@(?<Key>P\d+)",
				new MatchEvaluator((Match match) =>
				{
					var found = match.Groups["Key"].Value;
					if (!Context.ContainsKey(found)) return match.Value;

					object value = Context[found] ?? string.Empty;

					Type type = value.GetType();
					if (type.IsArray)
					{
						var query = from object obj in ((Array)(value))
										select conversion(obj, type);

						return string.Format("({0})", string.Join(", ", query.ToArray()));
					}
					else
						return conversion(value, type);
				})
			);
		}
	}
}
