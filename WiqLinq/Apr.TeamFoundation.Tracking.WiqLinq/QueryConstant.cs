using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Apr.TeamFoundation.Tracking.Linq
{
	public static class QueryConstant
	{
		public static string Me
		{
			get
			{
				throw new InvalidOperationException("This property is not intended to be used outside a LINQ To Wiql Query");
			}
		}

		public static DateTime Today
		{
			get
			{
				throw new InvalidOperationException("This property is not intended to be used outside a LINQ To Wiql Query");
			}

		}
	}
}
