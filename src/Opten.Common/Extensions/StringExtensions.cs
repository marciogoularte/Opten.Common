﻿using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Opten.Common.Extensions
{
	/// <summary>
	/// The String Extensions.
	/// </summary>
	public static class StringExtensions
	{

		/// <summary>
		/// Uppers the first char.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string UpperFirst(this string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return string.Empty;

			return char.ToUpper(input[0]) + input.Substring(1);
		}

		/// <summary>
		/// Lowers the first char.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string LowerFirst(this string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return string.Empty;

			return char.ToLower(input[0]) + input.Substring(1);
		}

		/// <summary>
		/// Uppers the first char (invariant).
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string UpperFirstInvariant(this string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return string.Empty;

			return char.ToUpperInvariant(input[0]) + input.Substring(1);
		}

		/// <summary>
		/// Lowers the first char (invariant).
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string LowerFirstInvariant(this string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return string.Empty;

			return char.ToLowerInvariant(input[0]) + input.Substring(1);
		}

		/// <summary>
		/// Trims the string if not null.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string NullCheckTrim(this string input)
		{
			return input.NullCheckTrim(returnNullIfEmpty: false);
		}

		/// <summary>
		/// Trims the string if not null.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="returnNullIfEmpty">if set to <c>true</c> [return null if empty].</param>
		/// <returns></returns>
		public static string NullCheckTrim(this string input, bool returnNullIfEmpty)
		{
			if (input == null) return null;
			if (string.IsNullOrWhiteSpace(input)) return returnNullIfEmpty ? null : string.Empty;
			else return input.Trim();
		}

		/// <summary>
		/// Converts the comma separated string to string array.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string[] ConvertCommaSeparatedToStringArray(this string input)
		{
			return input.ConvertCommaSeparatedToStringArray(
				stringSplitOptions: StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// Converts the comma separated string to string array.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="stringSplitOptions">The string split options.</param>
		/// <returns></returns>
		public static string[] ConvertCommaSeparatedToStringArray(this string input, StringSplitOptions stringSplitOptions)
		{
			if (string.IsNullOrWhiteSpace(input)) return new string[0];
			input = input.Replace("\"", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty); // Helper for javascript...
			return (from splitted in input.Split(new char[] { ',' }, stringSplitOptions)
					select splitted.NullCheckTrim()).ToArray();
		}

		/// <summary>
		/// Converts the comma separated string to int array.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static int[] ConvertCommaSeparatedToIntArray(this string input)
		{
			return input.ConvertCommaSeparatedToStringArray().ConvertStringArrayToIntArray();
		}

		/// <summary>
		/// Determines whether this string contains descending text.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static bool IsDescending(this string input)
		{
			input = input.NullCheckTrim();
			if (string.IsNullOrWhiteSpace(input)) return false;
			if (input.Equals("1")) return true;
			if (input.Equals("d", StringComparison.OrdinalIgnoreCase)) return true;
			if (input.Equals("des", StringComparison.OrdinalIgnoreCase)) return true;
			if (input.Equals("desc", StringComparison.OrdinalIgnoreCase)) return true;
			if (input.Equals("descending", StringComparison.OrdinalIgnoreCase)) return true;
			return false;
		}

		/// <summary>
		/// Removes something between a text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="start">The start.</param>
		/// <param name="end">The end.</param>
		/// <returns></returns>
		public static string RemoveBetween(this string text, string start, string end)
		{
			string pattern = string.Format("(\\{0}.*\\{1})", start, end);
			return Regex.Replace(text, pattern, string.Empty).Trim();
		}

		/// <summary>
		/// Removes the leading and trailing slash.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string RemoveLeadingAndTrailingSlash(this string text)
		{
			return text.Trim("/".ToCharArray());
		}

		/// <summary>
		/// Removes the leading and trailing slash and backslash.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string RemoveLeadingAndTrailingSlashAndBackslash(this string text)
		{
			return text.Trim(new char[] { '/', '\\' });
		}

		/// <summary>
		/// Gets the index of the whitespace before the length.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public static int GetIndexOfWhiteSpaceBeforeLength(this string text, int length)
		{
			text = text.NullCheckTrim();
			if (string.IsNullOrWhiteSpace(text)) return 0;

			if (text.Length <= length) return text.Length;

			int index = length;

			if (Char.IsWhiteSpace(text[length]) == false)
				index = GetIndexOfWhiteSpaceBeforeLength(text, length - 1);

			return index;
		}

		/// <summary>
		/// Trims all string properties of the object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objectToTrim">The object to trim.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">objectToTrim</exception>
		public static T TrimAllStringProperties<T>(this T objectToTrim)
		{
			if (objectToTrim == null)
				throw new ArgumentNullException("objectToTrim");

			foreach (PropertyInfo stringProperty in objectToTrim.GetType().GetReadAndWriteablePropertiesOfType<string>())
			{
				string currentValue = stringProperty.GetValue(objectToTrim) as string;

				if (currentValue == null) continue;

				stringProperty.SetValue(objectToTrim, currentValue.NullCheckTrim(), null);
			}

			return objectToTrim;
		}

		/// <summary>
		/// Determines whether the string [is digits only].
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static bool IsDigitsOnly(this string input)
		{
			foreach (char c in input)
			{
				if (c < '0' || c > '9')
					return false;
			}

			return true;
		}

		/// <summary>
		/// Removes special characters (everything except numbers and letters) and replace them with your choice.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="replacement">choose your replacement</param>
		/// <returns>
		/// cleaned up string
		/// </returns>
		public static string RemoveSpecialCharacters(this string input, string replacement = "-")
		{
			input = input.Replace(" ", replacement);
			input = input.Replace("ä", "ae");
			input = input.Replace("ö", "oe");
			input = input.Replace("ü", "ue");
			input = input.Replace("Ä", "Ae");
			input = input.Replace("Ö", "Oe");
			input = input.Replace("Ü", "Ue");

			return Regex.Replace(input, "[^0-9a-zA-Z]+", replacement);
		}
	}
}