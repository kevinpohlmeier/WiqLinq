using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Apr.TeamFoundation.Tracking.Core
{
	public static class AllFieldReferenceNames
	{
		private static readonly string[] m_all = CoreFieldReferenceNames.All.Concat(MiscFieldReferenceNames.All).ToArray();

		public static string[] All
		{
			get { return m_all; }
		}

		public static int Count
		{
			get { return m_all.Length; }
		}
	}
}
