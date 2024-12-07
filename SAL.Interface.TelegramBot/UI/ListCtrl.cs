using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Элемент управления постраничного списка</summary>
	public class ListCtrl
	{
		/// <summary>Ряд с кнопкой</summary>
		public class ButtonRow
		{
			/// <summary>Ключ, передаваемый при нажатии на кнопку</summary>
			public String Key { get; set; }
			/// <summary>Текст отображаемый клиенту </summary>
			public String Text { get; set; }

			/// <summary>Создание элкземпляра DTO для создания кнопки</summary>
			/// <param name="key">Ключ, передаваемый при нажатии на кнопку</param>
			/// <param name="text">Текст отображаемый на кнопке</param>
			public ButtonRow(String key, String text)
			{
				this.Key = key;
				this.Text = text;
			}
		}

		/// <summary>Ряд со списком</summary>
		public class ListRow
		{
			/// <summary>Ключ при нажатии на кнопку</summary>
			public String Key { get; set; }
			/// <summary>Заголовок на кнопке</summary>
			public String Title { get; set; }
			/// <summary>Текст, выводимый в каждой строке</summary>
			public String Text { get; set; }

			/// <summary>Создание экземпляра класса элемента списка с передачей всех данных для создания</summary>
			/// <param name="key">Ключ элемента</param>
			/// <param name="title">Заголовок элемента</param>
			/// <param name="text">Текс элемента в списке</param>
			public ListRow(String key, String title, String text)
			{
				this.Key = key;
				this.Title = title;
				this.Text = text;
			}
		}

		private readonly String _rowClickedMethodName;
		private readonly String _pagingMethodName;

		/// <summary>Максимальное количество элементов на одной странице</summary>
		public Int32 MaxPageSize { get; }

		/// <summary>Создание экземпляра класса со списком</summary>
		/// <param name="rowClickedMethodName">Наименование метода при нажатии на элемент списка</param>
		/// <param name="pagingMethodName">Наименование метода вызываемого при постраничном просмотре</param>
		/// <param name="maxPageSize">Максимальный азмер страницы</param>
		public ListCtrl(String rowClickedMethodName, String pagingMethodName, Int32 maxPageSize)
		{
			this._rowClickedMethodName = rowClickedMethodName;
			this._pagingMethodName = pagingMethodName;
			this.MaxPageSize = maxPageSize;
		}

		/// <summary>Создание ответа с массивом кнопок</summary>
		/// <param name="pagingKey">Ключ для постраничного просмотра (Передаётся в качестве первого аргумента)</param>
		/// <param name="skipIndex">Кол-во элементов которы пропустить с начала массива</param>
		/// <param name="rows">Поток с рядами кнопок для отображения</param>
		/// <returns>Ответ с кнопками</returns>
		public Reply Construct(String pagingKey, Int32 skipIndex, IEnumerable<ButtonRow> rows)
		{
			Int32 index = skipIndex;
			rows = rows.Skip(skipIndex);

			InlineKeyboardMarkup markup = new InlineKeyboardMarkup();
			Int32 iterator = 0;
			foreach(ButtonRow row in rows.Take(this.MaxPageSize+1))
			{
				if(iterator <= this.MaxPageSize)
				{
					InlineButton rowClickedButton = this.CreateRowClickedButton(row.Text, row.Key);
					if(rowClickedButton == null)
						throw new ArgumentNullException("Необходимо указать Callback метод для нажатия на конпку");

					markup.Keyboard.Add(new InlineButton[] { rowClickedButton, });
					index++;
				}
				iterator++;
			}

			InlineButton[] paging = this.CreatePageButtons(pagingKey, skipIndex, iterator > this.MaxPageSize);
			if(paging.Length > 0)
				markup.Keyboard.Add(paging);

			return new Reply() { Markup = markup, };
		}

		/// <summary>Создание ответа со списком и массивом кнопок</summary>
		/// <param name="pagingKey">Ключ постраничного просмотра, передаётся в качестве первого аргумента</param>
		/// <param name="skipIndex">Кол-во элементов сколько пропустить сначала списка</param>
		/// <param name="rows">Ряды</param>
		/// <returns>Подготовленный ответ с текстом и кнопками</returns>
		public Reply Construct(String pagingKey, Int32 skipIndex, IEnumerable<ListRow> rows)
		{
			Int32 index = skipIndex;
			rows = rows.Skip(skipIndex);

			String prevTitle = null;

			StringBuilder body = new StringBuilder();
			InlineKeyboardMarkup markup = new InlineKeyboardMarkup();
			List<InlineButton> buttonRows = new List<InlineButton>();

			Int32 iterator = 0;
			foreach(ListRow row in rows.Take(this.MaxPageSize + 1))
			{
				if(iterator == this.MaxPageSize)
					break;

				if(row.Title != prevTitle)
				{
					body.AppendLine(row.Title);
					prevTitle = row.Title;
				}

				InlineButton rowClickedButton = this.CreateRowClickedButton(index.ToString(), row.Key);
				if(rowClickedButton == null)
					body.AppendLine(row.Text);
				else
				{
					body.AppendLine(index + ".\t " + row.Text);
					buttonRows.Add(rowClickedButton);
					if(buttonRows.Count == 5)
					{
						markup.Keyboard.Add(buttonRows.ToArray());
						buttonRows.Clear();
					}
				}
				index++;
				iterator++;
			}

			if(buttonRows.Count > 0)
			{
				markup.Keyboard.Add(buttonRows.ToArray());
				buttonRows.Clear();
			}

			InlineButton[] pagingButtons = this.CreatePageButtons(pagingKey, skipIndex, iterator == this.MaxPageSize);
			if(pagingButtons.Length > 0)
				markup.Keyboard.Add(pagingButtons);

			return new Reply()
			{
				Title = String.Join(Environment.NewLine, body.ToString()),
				Markup = markup,
			};
		}

		private InlineButton[] CreatePageButtons(String pagingKey, Int32 skipIndex, Boolean hasMoreRows)
		{
			List<InlineButton> buttons = new List<InlineButton>();
			if(skipIndex >= this.MaxPageSize)
				buttons.Add(this.CreatePagingButton("⬅", pagingKey, skipIndex - this.MaxPageSize));
			if(hasMoreRows)//Есть ещё данные
				buttons.Add(this.CreatePagingButton("➡", pagingKey, skipIndex + this.MaxPageSize));

			return buttons.ToArray();
		}

		/// <summary>Метод для создания кнопок постраничного просмотра</summary>
		/// <param name="title">Текст на кнопке</param>
		/// <param name="pagingKey">Ключ постраничного просмотра</param>
		/// <param name="skipIndex">Кол-во элементов для пропуска с начала массива</param>
		/// <returns>Кнопка для постраничного просмотра</returns>
		protected virtual InlineButton CreatePagingButton(String title, String pagingKey, Int32 skipIndex)
		{
			return pagingKey == null
				? MethodInvoker.CreateInlineButton(title, this._pagingMethodName, skipIndex)
				: MethodInvoker.CreateInlineButton(title, this._pagingMethodName, pagingKey, skipIndex);
		}

		/// <summary>Метод для создания кнопки нажатия на ряд</summary>
		/// <param name="title">Текст на кнопке</param>
		/// <param name="key">Ключ ряда из списка</param>
		/// <returns>Кнопка для вызова метода нажатия на ряд</returns>
		protected virtual InlineButton CreateRowClickedButton(String title, String key)
		{
			return String.IsNullOrEmpty(this._rowClickedMethodName)
				? null
				: MethodInvoker.CreateInlineButton(title, this._rowClickedMethodName, key);
		}
	}
}