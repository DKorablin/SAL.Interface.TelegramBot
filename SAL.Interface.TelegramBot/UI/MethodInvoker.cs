using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Class for creating callbacks to methods from a cart</summary>
	public static class MethodInvoker
	{
		/// <summary>Create a callback to a method with one argument and a response stream</summary>
		/// <typeparam name="T">Argument type</typeparam>
		/// <param name="title">Button label</param>
		/// <param name="callback">Method reflection</param>
		/// <param name="argument">Single argument accepted by the method</param>
		/// <returns>Button to render in the response</returns>
		public static InlineButton CreateInlineButton<T>(String title, Func<T, IEnumerable<Reply>> callback, T argument)
			=> CreateInlineButton(title, callback.Method, argument);

		/// <summary>Create a callback to a method with two arguments and a stream of replies</summary>
		/// <typeparam name="T1">Type of the first argument</typeparam>
		/// <typeparam name="T2">Type of the second argument</typeparam>
		/// <param name="title">Button label</param>
		/// <param name="callback">Method reflection</param>
		/// <param name="argument1">First argument</param>
		/// <param name="argument2">Second argument</param>
		/// <returns>Button to render in the response</returns>
		public static InlineButton CreateInlineButton<T1,T2>(String title,Func<T1,T2,IEnumerable<Reply>> callback,T1 argument1,T2 argument2)
			=> CreateInlineButton(title, callback.Method, argument1, argument2);

		/// <summary>Create a callback to a method with three arguments and a stream of replies</summary>
		/// <typeparam name="T1">Type of the first argument</typeparam>
		/// <typeparam name="T2">Type of the second argument</typeparam>
		/// <typeparam name="T3">Type of the third argument</typeparam>
		/// <param name="title">Button label</param>
		/// <param name="callback">Method reflection</param>
		/// <param name="argument1">First argument</param>
		/// <param name="argument2">Second argument</param>
		/// <param name="argument3">Third argument</param>
		/// <returns>Button to render in the response</returns>
		public static InlineButton CreateInlineButton<T1, T2, T3>(String title, Func<T1, T2, T3, IEnumerable<Reply>> callback, T1 argument1, T2 argument2, T3 argument3)
			=> CreateInlineButton(title, callback.Method, argument1, argument2, argument3);

		/// <summary>Create a callback to a method with four arguments and a stream of replies</summary>
		/// <typeparam name="T1">Type of the first argument</typeparam>
		/// <typeparam name="T2">Type of the second argument</typeparam>
		/// <typeparam name="T3">Type of the third argument</typeparam>
		/// <typeparam name="T4">Type of the fourth argument</typeparam>
		/// <param name="title">Button label</param>
		/// <param name="callback">Method reflection</param>
		/// <param name="argument1">First argument</param>
		/// <param name="argument2">Second argument</param>
		/// <param name="argument3">Third argument</param>
		/// <param name="argument4">Fourth argument</param>
		/// <returns>Button to render in the response</returns>
		public static InlineButton CreateInlineButton<T1, T2, T3, T4>(String title, Func<T1, T2, T3, T4, IEnumerable<Reply>> callback, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
			=> CreateInlineButton(title, callback.Method, argument1, argument2, argument3, argument4);

		/// <summary>Create a button specifying text and method reflection for callback</summary>
		/// <param name="title">Text on the button</param>
		/// <param name="method">Method reflection</param>
		/// <param name="values">Arguments passed to the method</param>
		/// <returns>Button for the response</returns>
		public static InlineButton CreateInlineButton(String title, MethodInfo method, params Object[] values)
			=> new InlineButton(title == null ? "null" : title, CreateReference(method, values));

		/// <summary>Create a button with an arbitrary command to be executed by an unknown method (for example, in another plugin)</summary>
		/// <param name="title">Text on the button</param>
		/// <param name="callbackMethod">Callback command text</param>
		/// <param name="values">Arguments passed to the method</param>
		/// <returns>Button for the response</returns>
		public static InlineButton CreateInlineButton(String title, String callbackMethod, params Object[] values)
			=> new InlineButton(title == null ? "null" : title, callbackMethod + "_" + String.Join("_", FormatParameters(values)));

		/// <summary>Get a list of all supported methods via reflection</summary>
		/// <param name="instanceType">Class type where supported methods are searched</param>
		/// <returns>Array of supported methods</returns>
		public static MethodInfo[] GetSupportedMethods(Type instanceType)
			=> instanceType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
				.Where(p => p.ReturnType == typeof(IEnumerable<Reply>) || p.ReturnType == typeof(Reply[]) || p.ReturnType == typeof(Reply))
				.ToArray();

		/// <summary>Get a unique key of the method based on reflection</summary>
		/// <param name="method">Method reflection</param>
		/// <returns>Unique method key</returns>
		public static Int32 GetMethodKey(MethodInfo method)
		{
			String methodKey = method.DeclaringType.FullName + '.' + method.Name;
			return methodKey.GetHashCode();
		}

		/// <summary>Create a full command to invoke a specific method</summary>
		/// <param name="method">Method reflection</param>
		/// <param name="values">Values passed to the method when the button is pressed</param>
		/// <returns>Command to send to Telegram by which the method will be invoked and arguments passed back</returns>
		public static String CreateReference(MethodInfo method, params Object[] values)
			=> "/" + GetMethodKey(method) + ":" + String.Join("&", FormatParameters(values));

		/// <summary>Convert parameters to string representation</summary>
		/// <param name="values">Method arguments</param>
		/// <returns>Sequence of formatted objects</returns>
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