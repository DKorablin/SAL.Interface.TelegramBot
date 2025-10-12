using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Paged List Control</summary>
	public class ListCtrl
	{
		/// <summary>Row with a button</summary>
		public class ButtonRow
		{
			/// <summary>The key transmitted when the button is pressed</summary>
			public String Key { get; set; }
			/// <summary>Text displayed to the client</summary>
			public String Text { get; set; }

			/// <summary>Creating a DTO instance to create a button</summary>
			/// <param name="key">The key transmitted when the button is pressed</param>
			/// <param name="text">The text displayed on the button</param>
			public ButtonRow(String key, String text)
			{
				this.Key = key;
				this.Text = text;
			}
		}

		/// <summary>Row with list</summary>
		public class ListRow
		{
			/// <summary>Key when pressing the button</summary>
			public String Key { get; set; }
			/// <summary>Headline on the button</summary>
			public String Title { get; set; }
			/// <summary>Text to display on each line</summary>
			public String Text { get; set; }

			/// <summary>Create an instance of the list item class passing all the data for creation</summary>
			/// <param name="key">Element key</param>
			/// <param name="title">Element title</param>
			/// <param name="text">Text of an element in a list</param>
			public ListRow(String key, String title, String text)
			{
				this.Key = key;
				this.Title = title;
				this.Text = text;
			}
		}

		private readonly String _rowClickedMethodName;
		private readonly String _pagingMethodName;

		/// <summary>Maximum number of elements on one page</summary>
		public Int32 MaxPageSize { get; }

		/// <summary>Creating an instance of a class with a list</summary>
		/// <param name="rowClickedMethodName">Name of the method when clicking on a list item</param>
		/// <param name="pagingMethodName">Name of the method called during paging</param>
		/// <param name="maxPageSize">Maximum page size</param>
		public ListCtrl(String rowClickedMethodName, String pagingMethodName, Int32 maxPageSize)
		{
			this._rowClickedMethodName = rowClickedMethodName;
			this._pagingMethodName = pagingMethodName;
			this.MaxPageSize = maxPageSize;
		}

		/// <summary>Creating a response with an array of buttons</summary>
		/// <param name="pagingKey">Key for pagination (Passed as the first argument)</param>
		/// <param name="skipIndex">Number of elements to skip from the beginning of the array</param>
		/// <param name="rows">Stream with rows of buttons for display</param>
		/// <returns>Answer with buttons</returns>
		public Reply Construct(String pagingKey, Int32 skipIndex, IEnumerable<ButtonRow> rows)
		{
			Int32 index = skipIndex;
			rows = rows.Skip(skipIndex);

			InlineKeyboardMarkup markup = new InlineKeyboardMarkup();
			Int32 iterator = 0;
			foreach(ButtonRow row in rows.Take(this.MaxPageSize + 1))
			{
				if(iterator <= this.MaxPageSize)
				{
					InlineButton rowClickedButton = this.CreateRowClickedButton(row.Text, row.Key)
						?? throw new ArgumentNullException(nameof(rows), "You must specify a Callback method for clicking the button.");

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

		/// <summary>Creating a response with a list and an array of buttons</summary>
		/// <param name="pagingKey">The paging key, passed as the first argument</param>
		/// <param name="skipIndex">Number of elements to skip from the beginning of the list</param>
		/// <param name="rows">The rows</param>
		/// <returns>Prepared response with text and buttons</returns>
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
			if(hasMoreRows)//There is more data
				buttons.Add(this.CreatePagingButton("➡", pagingKey, skipIndex + this.MaxPageSize));

			return buttons.ToArray();
		}

		/// <summary>Method for creating pagination buttons</summary>
		/// <param name="title">Text on the button</param>
		/// <param name="pagingKey">Pagination key</param>
		/// <param name="skipIndex">Number of elements to skip from the beginning of the array</param>
		/// <returns>Button for pagination</returns>
		protected virtual InlineButton CreatePagingButton(String title, String pagingKey, Int32 skipIndex)
			=> pagingKey == null
				? MethodInvoker.CreateInlineButton(title, this._pagingMethodName, skipIndex)
				: MethodInvoker.CreateInlineButton(title, this._pagingMethodName, pagingKey, skipIndex);

		/// <summary>Method to create a click button on a row</summary>
		/// <param name="title">Text on the button</param>
		/// <param name="key">Row key from list</param>
		/// <returns>Button to call the method of clicking on a row</returns>
		protected virtual InlineButton CreateRowClickedButton(String title, String key)
			=> String.IsNullOrEmpty(this._rowClickedMethodName)
				? null
				: MethodInvoker.CreateInlineButton(title, this._rowClickedMethodName, key);
	}
}