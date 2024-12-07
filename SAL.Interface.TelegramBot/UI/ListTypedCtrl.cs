using System;
using System.Reflection;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Типизированный элемент управления постраничного списка</summary>
	public class ListTypedCtrl : ListCtrl
	{
		private readonly MethodInfo _rowClickedMethod;
		private readonly MethodInfo _pagingMethod;

		/// <summary>Создание типизированного элемента управления постраничным списком с передачей метода нажатия на строку в произвольном виде</summary>
		/// <param name="rowClickedMethodName">Строковая комманда нажатия на строку в произвольном виде</param>
		/// <param name="pagingMethod">Метод постраничного спросмотра</param>
		/// <param name="maxPageSize">Максимальное кол-во элементов на странице</param>
		public ListTypedCtrl(String rowClickedMethodName, MethodInfo pagingMethod, Int32 maxPageSize)
			: base(rowClickedMethodName, null, maxPageSize)
		{
			this._pagingMethod = pagingMethod;
		}

		/// <summary>Создание элемента управления постраничным списком с передачей строготипизированных методов нажатия на кнопку и постраничного просмотра</summary>
		/// <param name="rowClickedMethod">Метод нажатия на строку</param>
		/// <param name="pagingMethod">Метод постраничного просмотра</param>
		/// <param name="maxPageSize">Максимальное кол-во элементов на странице</param>
		public ListTypedCtrl(MethodInfo rowClickedMethod, MethodInfo pagingMethod, Int32 maxPageSize)
			: base(null, null, maxPageSize)
		{
			this._rowClickedMethod = rowClickedMethod;
			this._pagingMethod = pagingMethod;
		}

		/// <summary>Метод для создания кнопок постраничного просмотра</summary>
		/// <param name="title">Текст на кнопке</param>
		/// <param name="pagingKey">Ключ постраничного просмотра</param>
		/// <param name="skipIndex">Кол-во элементов для пропуска с начала массива</param>
		/// <returns>Кнопка для постраничного просмотра</returns>
		protected override InlineButton CreatePagingButton(String title, String pagingKey, Int32 skipIndex)
			=> pagingKey == null
				? MethodInvoker.CreateInlineButton(title, this._pagingMethod, skipIndex)
				: MethodInvoker.CreateInlineButton(title, this._pagingMethod, pagingKey, skipIndex);

		/// <summary>Метод для создания кнопки нажатия на ряд</summary>
		/// <param name="title">Текст на кнопке</param>
		/// <param name="key">Ключ ряда из списка</param>
		/// <returns>Кнопка для вызова метода нажатия на ряд</returns>
		protected override InlineButton CreateRowClickedButton(String title, String key)
			=> this._rowClickedMethod == null
				? base.CreateRowClickedButton(title, key)
				: MethodInvoker.CreateInlineButton(title, this._rowClickedMethod, key);
	}
}