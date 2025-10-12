using System;
using System.Reflection;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Typed Paged List Control</summary>
	public class ListTypedCtrl : ListCtrl
	{
		private readonly MethodInfo _rowClickedMethod;
		private readonly MethodInfo _pagingMethod;

		/// <summary>Create a typed paginated list control with a custom row click method</summary>
		/// <param name="rowClickedMethodName">String command to click on a line in any format</param>
		/// <param name="pagingMethod">Page-by-page browsing method</param>
		/// <param name="maxPageSize">Maximum number of elements per page</param>
		public ListTypedCtrl(String rowClickedMethodName, MethodInfo pagingMethod, Int32 maxPageSize)
			: base(rowClickedMethodName, null, maxPageSize)
		{
			this._pagingMethod = pagingMethod;
		}

		/// <summary>Create a paginated list control passing strongly typed button click and pagination methods</summary>
		/// <param name="rowClickedMethod">Line click method</param>
		/// <param name="pagingMethod">Paging method</param>
		/// <param name="maxPageSize">Maximum number of elements per page</param>
		public ListTypedCtrl(MethodInfo rowClickedMethod, MethodInfo pagingMethod, Int32 maxPageSize)
			: base(null, null, maxPageSize)
		{
			this._rowClickedMethod = rowClickedMethod;
			this._pagingMethod = pagingMethod;
		}

		/// <summary>Method for creating pagination buttons</summary>
		/// <param name="title">Text on the button</param>
		/// <param name="pagingKey">Pagination key</param>
		/// <param name="skipIndex">Number of elements to skip from the beginning of the array</param>
		/// <returns>Button for pagination</returns>
		protected override InlineButton CreatePagingButton(String title, String pagingKey, Int32 skipIndex)
			=> pagingKey == null
				? MethodInvoker.CreateInlineButton(title, this._pagingMethod, skipIndex)
				: MethodInvoker.CreateInlineButton(title, this._pagingMethod, pagingKey, skipIndex);

		/// <summary>Method to create a click button on a row</summary>
		/// <param name="title">Text on the button</param>
		/// <param name="key">Row key from list</param>
		/// <returns>Button to call the method of clicking on a row</returns>
		protected override InlineButton CreateRowClickedButton(String title, String key)
			=> this._rowClickedMethod == null
				? base.CreateRowClickedButton(title, key)
				: MethodInvoker.CreateInlineButton(title, this._rowClickedMethod, key);
	}
}