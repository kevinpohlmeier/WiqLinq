using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apr.TeamFoundation.Tracking.Linq
{

	internal static class LinqFieldExtenderHelper
	{
		public static bool NotForOutsideUse()
		{
			throw new InvalidOperationException("This property is not intended to be used outside a LINQ To Wiql Query");
		}
	}

	public class LinqFieldExtender
	{
		internal LinqFieldExtender() { }
		// TeamProject: =, <>, In
		// Title: =, <>, <, >, <=, >=, Contains, Does Not Contain, In, Was Ever
		// Id:  =, <>, <, >, <=, >=, In
		// Date: =, <>, <, >, <=, >=, In, Was Ever
		// Path: =, <>, Under, Not Under, In


		/// <summary>
		/// WHERE [System.AreaPath] under 'MyProject\Server\Administration'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool Under(string path)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		/// <summary>
		/// WHERE [System.AreaPath] not under 'MyProject\Feature1'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool NotUnder(string path)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		/// <summary>
		/// WHERE [System.CreatedBy] in ('joselugo', 'jeffhay', 'linaabola')
		/// WHERE [System.CreatedBy] = 'joselugo' OR [System.CreatedBy] = 'jeffhay' OR [System.CreatedBy] = 'linaabola'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public bool In(params object[] values)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		/// <summary>
		/// WHERE ever ([Assigned To] = ‘joselugo')
		/// WHERE [Assigned To] ever ‘joselugo'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool Ever(object value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		/// <summary>
		/// WHERE [System.AssignedTo] not ever 'joselugo'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool NotEver(object value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		public bool Contains(string value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		public bool NotContains(string value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		public static bool operator ==(LinqFieldExtender field, object value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator !=(LinqFieldExtender field, object value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator >(LinqFieldExtender field, object value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator <=(LinqFieldExtender field, object value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator <(LinqFieldExtender field, object value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator >=(LinqFieldExtender field, object value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
	}

	public class LinqFieldExtender<T>
	{
		internal LinqFieldExtender() { }

		/// <summary>
		/// WHERE [System.AreaPath] under 'MyProject\Server\Administration'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool Under(T path)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		/// <summary>
		/// WHERE [System.AreaPath] not under 'MyProject\Feature1'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool NotUnder(T path)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		/// <summary>
		/// WHERE [System.CreatedBy] in ('joselugo', 'jeffhay', 'linaabola')
		/// WHERE [System.CreatedBy] = 'joselugo' OR [System.CreatedBy] = 'jeffhay' OR [System.CreatedBy] = 'linaabola'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public bool In(params T[] values)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		/// <summary>
		/// WHERE ever ([Assigned To] = ‘joselugo')
		/// WHERE [Assigned To] ever ‘joselugo'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool Ever(T value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		/// <summary>
		/// WHERE [System.AssignedTo] not ever 'joselugo'
		/// </summary>
		/// <param name="item"></param>
		/// <param name="fieldName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool NotEver(T value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		public bool Contains(string value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		public bool NotContains(string value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}

		public static bool operator ==(LinqFieldExtender<T> field, T value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator !=(LinqFieldExtender<T> field, T value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator <(LinqFieldExtender<T> field, T value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator >=(LinqFieldExtender<T> field, T value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator >(LinqFieldExtender<T> field, T value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
		public static bool operator <=(LinqFieldExtender<T> field, T value)
		{
			return LinqFieldExtenderHelper.NotForOutsideUse();
		}
	}
}
