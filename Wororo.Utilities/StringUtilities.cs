﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wororo.Utilities
{
    public static class StringUtilities
    {
        private const string Space = " ";
        private static readonly Regex ToDigitsOnlyRegex = new Regex("[\\D]", RegexOptions.Compiled);
        private static readonly Regex ToIntRegex = new Regex("[^0-9+-.,]", RegexOptions.Compiled);
        private static readonly Regex ToDoubleRegex = new Regex("[^0-9+-.,eE]", RegexOptions.Compiled);
        private static readonly Regex ToSingleLineRegex = new Regex(@"[\r\n]+", RegexOptions.Compiled);
        private static readonly Regex ToLettersOnlyRegex = new Regex(@"[^a-zA-Z]", RegexOptions.Compiled);
        private static readonly Regex RemoveSymbolsRegex = new Regex("[^0-9a-zA-Z\\s]", RegexOptions.Compiled);
        private static readonly Regex RemoveSpacesRegex = new Regex("[ ]{2,}", RegexOptions.Compiled);

        private static readonly Regex ToSentenceCaseRegex =
            new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        public static string GetRegexMatchGroup(this string input, string regexPattern, int groupIndex)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var r = new Regex(regexPattern);
            var m = r.Match(input);
            return m.Success ? m.Groups.Count > groupIndex ? m.Groups[groupIndex].Value : string.Empty : string.Empty;
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string Remove2PlusSpaces(this string text)
        {
            return RemoveSpacesRegex.Replace(text, Space).Trim();
        }

        public static string RemoveSymbols(this string text)
        {
            return RemoveSymbolsRegex.Replace(text, string.Empty).Remove2PlusSpaces();
        }

        public static string TabToSpaces(this string text)
        {
            return text.Replace('\t', ' ');
        }

        public static string ToDigitsOnly(this string input)
        {
            return ToDigitsOnlyRegex.Replace(input, string.Empty);
        }

        public static double ToDouble(this string input)
        {
            var value = ToDoubleRegex.Replace(input, string.Empty);
            return value.IsEmpty() ? 0 : double.Parse(value);
        }

        public static int ToInt(this string input)
        {
            var value = ToIntRegex.Replace(input, string.Empty);
            return value.IsEmpty() ? 0 : (int)double.Parse(value);
        }

        public static string ToLetters(this string text)
        {
            return ToLettersOnlyRegex.Replace(text, string.Empty);
        }

        public static string ToNormalized(this string text)
        {
            return new string(text.Normalize(NormalizationForm.FormD)
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    .ToArray())
                .Normalize(NormalizationForm.FormC);
        }

        public static int ToNumbers(this string text)
        {
            var tryInt = -1;
            if (text.IsEmpty()) return tryInt;

            var replaced = ToDigitsOnlyRegex.Replace(text, string.Empty);
            if (!string.IsNullOrWhiteSpace(replaced))
                int.TryParse(replaced, out tryInt);
            return tryInt;
        }

        public static string ToSentenceCase(this string lowerCaseString)
        {
            return ToSentenceCaseRegex.Replace(lowerCaseString, s => s.Value.ToUpper());
        }

        public static string ToSingleLineText(this string text)
        {
            return text.IsEmpty()
                ? string.Empty
                : ToSingleLineRegex.Replace(text.Replace(Environment.NewLine, Space), Space).Remove2PlusSpaces();
        }

        public static string ToTitleCase(this string lowerCaseString)
        {
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(lowerCaseString);
        }
    }
}