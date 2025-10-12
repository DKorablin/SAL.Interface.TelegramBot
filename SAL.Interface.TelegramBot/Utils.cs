using System;
using System.ComponentModel;
using System.Reflection;

namespace SAL.Interface.TelegramBot
{
	internal static class Utils
	{
		public static Object TryChangeValue(String text, TypeCode requiredType)
		{
			try
			{
				return Convert.ChangeType(text, requiredType);
			} catch(InvalidCastException)
			{
				return null;
			} catch(FormatException)
			{
				return null;
			} catch(OverflowException)
			{
				return null;
			}
		}

		public static Boolean TryChangeValue(String text, ParameterInfo parameter, out Object value)
		{
			value = null;
			Boolean result = false;
			TypeConverter converter = TypeDescriptor.GetConverter(parameter.ParameterType);
			try
			{
				value = converter.ConvertFromString(text);

				// value = Convert.ChangeType(text, parameter.ParameterType)
				result = true;
			} catch(IndexOutOfRangeException)
			{
			} catch(NotSupportedException)
			{
			} catch(Exception exc)
			{
				if(!(exc.InnerException is IndexOutOfRangeException))
					throw;
			}

			if(!result && parameter.HasDefaultValue)
			{
				value = parameter.DefaultValue;
				result = true;
			}
			return result;
		}
	}
}