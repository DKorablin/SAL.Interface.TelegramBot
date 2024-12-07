using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Класс создания callback'ов на методы из телеги</summary>
	public class MethodInvoker
	{
		/// <summary>Создать callback на метод с одним аргументом и потоком ответов</summary>
		/// <typeparam name="T">Тип аргумента</typeparam>
		/// <param name="title">Заголовок на кнопке</param>
		/// <param name="callback">Рефлексия на метод</param>
		/// <param name="argument">Один аргумент принимаемый в метод</param>
		/// <returns>Кнопка для рисования в ответе</returns>
		public static InlineButton CreateInlineButton<T>(String title, Func<T, IEnumerable<Reply>> callback, T argument)
			=> CreateInlineButton(title, callback.Method, argument);

		/// <summary>Создать callback на метод с двумя аргументами и потоком ответов</summary>
		/// <typeparam name="T1">Тип первого аргумента</typeparam>
		/// <typeparam name="T2">Тип второго аргумента</typeparam>
		/// <param name="title">Заголовок на кнопке</param>
		/// <param name="callback">Рефлексия на метод</param>
		/// <param name="argument1">Первый аргумент</param>
		/// <param name="argument2">Второй аргумент</param>
		/// <returns>Кнопка для рисования в ответе</returns>
		public static InlineButton CreateInlineButton<T1,T2>(String title,Func<T1,T2,IEnumerable<Reply>> callback,T1 argument1,T2 argument2)
			=> CreateInlineButton(title, callback.Method, argument1, argument2);

		/// <summary>Создать callback на метод с трёмя аргументами и потоком ответов</summary>
		/// <typeparam name="T1">Тип первого аргумента</typeparam>
		/// <typeparam name="T2">Тип второго аргумента</typeparam>
		/// <typeparam name="T3">Тип третьего аргумента</typeparam>
		/// <param name="title">Заголовок на кнопке</param>
		/// <param name="callback">Рефлексия на метод</param>
		/// <param name="argument1">Первый аргумент</param>
		/// <param name="argument2">Второй аргумент</param>
		/// <param name="argument3">Третий аргумент</param>
		/// <returns>Кнопка для рисования в ответе</returns>
		public static InlineButton CreateInlineButton<T1, T2, T3>(String title, Func<T1, T2, T3, IEnumerable<Reply>> callback, T1 argument1, T2 argument2, T3 argument3)
			=> CreateInlineButton(title, callback.Method, argument1, argument2, argument3);

		/// <summary>Создать callback на метод с четырьмя аргументами и потоком ответов</summary>
		/// <typeparam name="T1">Тип первого аргумента</typeparam>
		/// <typeparam name="T2">Тип второго аргумента</typeparam>
		/// <typeparam name="T3">Тип третьего аргумента</typeparam>
		/// <typeparam name="T4">Тип четвёртого аргумента</typeparam>
		/// <param name="title">Заголовок на кнопке</param>
		/// <param name="callback">Рефлексия на метод</param>
		/// <param name="argument1">Первый аргумент</param>
		/// <param name="argument2">Второй аргумент</param>
		/// <param name="argument3">Третий аргумент</param>
		/// <param name="argument4">Четвёртый аргумент</param>
		/// <returns>Кнопка для рисования в ответе</returns>
		public static InlineButton CreateInlineButton<T1, T2, T3, T4>(String title, Func<T1, T2, T3, T4, IEnumerable<Reply>> callback, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
			=> CreateInlineButton(title, callback.Method, argument1, argument2, argument3, argument4);

		/// <summary>Получить список всех поддерживаемых методов в вефлексии</summary>
		/// <param name="instanceType">Рефлексия класса, где подразумевается поиск поддерживаемых методов</param>
		/// <returns>Массив поддерживаемых методов</returns>
		public static MethodInfo[] GetSupportedMethods(Type instanceType)
			=> instanceType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
				.Where(p => p.ReturnType == typeof(IEnumerable<Reply>) || p.ReturnType == typeof(Reply[]) || p.ReturnType == typeof(Reply))
				.ToArray();

		/// <summary>Получить уникальный ключ метода исходя из рефлексии</summary>
		/// <param name="method">Рефлексия метода</param>
		/// <returns>Уникальный ключ метода</returns>
		public static Int32 GetMethodKey(MethodInfo method)
		{
			String methodKey = method.DeclaringType.FullName + '.' + method.Name;
			return methodKey.GetHashCode();
		}

		/// <summary>Создание кнопки с указанием текста и рефлексией метода для обратного вызова</summary>
		/// <param name="title">Текст на кнопке</param>
		/// <param name="method">Рефлексия метода</param>
		/// <param name="values">Аргументы, передаваемые в метод</param>
		/// <returns>Кнопка для ответа</returns>
		public static InlineButton CreateInlineButton(String title, MethodInfo method, params Object[] values)
			=> new InlineButton(title == null ? "null" : title, CreateReference(method, values));

		/// <summary>Создание кнопки с указанием произвольной коммандой для выполнения неизвестным методом (к примеру, в другом плагине)</summary>
		/// <param name="title">Текст на кнопке</param>
		/// <param name="callbackMethod">Текстовая комманда обратного вызова</param>
		/// <param name="values">Аргументы, передаваемые в метод</param>
		/// <returns>Кнопка для ответа</returns>
		public static InlineButton CreateInlineButton(String title,String callbackMethod, params Object[] values)
			=> new InlineButton(title == null ? "null" : title, callbackMethod + "_" + String.Join("_", FormatParameters(values)));

		/// <summary>Создать полноценную команду для вызова конкретного метода</summary>
		/// <param name="method">Рефлексия метода</param>
		/// <param name="values">Значения передаваемые в метод при нажатии на кнопку</param>
		/// <returns>Комманда для пердачи в телеграм, по которой обратно будет вызван метода и переданы аргументы</returns>
		public static String CreateReference(MethodInfo method, params Object[] values)
			=> "/" + GetMethodKey(method) + ":" + String.Join("&", FormatParameters(values));

		/// <summary>Преобразвать параметры в строковое представление</summary>
		/// <param name="values">Аргументы метода</param>
		/// <returns>Поток отформатированных объектов</returns>
		private static IEnumerable<Object> FormatParameters(Object[] values)
		{
			foreach(Object value in values)
				if(value == null)
					yield return String.Empty;
				else if(value is Guid g)
					yield return g.ToString("N");
				else if(value is Message)
					continue;
				else
					yield return value;
		}
	}
}