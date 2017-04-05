using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apr.TeamFoundation.Tracking.Core
{
	public static class MiscFieldReferenceNames
	{
		public static readonly string Triage = "Microsoft.VSTS.Common.Triage";
		public static readonly string IterationPath = "System.IterationPath";
		public static readonly string StepsToReproduce = "Microsoft.VSTS.CMMI.StepsToReproduce";
		public static readonly string ResolvedBy = "Microsoft.VSTS.Common.ResolvedBy";
		public static readonly string ResolvedDate = "Microsoft.VSTS.Common.ResolvedDate";
		public static readonly string Estimate = "Microsoft.VSTS.CMMI.Estimate";

		public static readonly string CompletedWork = "Microsoft.VSTS.Scheduling.CompletedWork";

		private static string[] m_all = new string[] 
		{
			Triage,
			IterationPath,
			StepsToReproduce,
			ResolvedBy,
			ResolvedDate,
			Estimate,

			CompletedWork
		};

		// Properties
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
