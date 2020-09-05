using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Houseplants.Common
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string str) where T : struct//, IConvertible
        {
            return str.ToEnum(default(T));
        }

        public static T ToEnum<T>(this string str, T withDefault) where T : struct//, IConvertible
        {
            //if (!typeof(T).IsEnum)
            //{
            //    throw new ArgumentException("T must be an enumerated type");
            //}

            T x;
            if (Enum.TryParse<T>(str, true, out x))
                return x;
            return withDefault;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static IList<string> SplitByStringFormat(this string str, string template)
        {
            // replace all the special regex characters
            template = Regex.Replace(template, @"[\\\^\$\.\|\?\*\+\(\)]", x => "\\" + x.Value);
            string pattern = "^" + Regex.Replace(template, @"\{[0-9]+\}", "(.*?)") + "$";

            Regex r = new Regex(pattern);
            Match m = r.Match(str);

            List<string> ret = new List<string>();

            for (int i = 1; i < m.Groups.Count; i++)
            {
                ret.Add(m.Groups[i].Value);
            }

            return ret;
        }

        public static decimal? ToDecimal(this string sourceString)
        {
            sourceString = sourceString
               .Replace(".", CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator)
               .Replace(",", CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator).Trim();

            var numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator;
            decimal result;

            if (!Decimal.TryParse(sourceString, NumberStyles.Any, numberFormatInfo, out result))
            {
                return null;
            }

            return result;
        }

        public static double? ToDouble(this string sourceString)
        {
            sourceString = sourceString
               .Replace(".", CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator)
               .Replace(",", CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator).Trim();

            var numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator;
            double result;

            if (!Double.TryParse(sourceString, NumberStyles.Any, numberFormatInfo, out result))
            {
                return null;
            }

            return result;
        }

        public static float? ToFloat(this string sourceString)
        {
            sourceString = sourceString
               .Replace(".", CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator)
               .Replace(",", CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator).Trim();

            var numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator;
            float result;

            if (!float.TryParse(sourceString, NumberStyles.Any, numberFormatInfo, out result))
            {
                return null;
            }

            return result;
        }

        public static string FormatStr(this string str, params object[] args)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            return string.Format(str, args);
        }

        public static int LevenshteinDistance(this string s, string t)
        {
            int[,] d = new int[s.Length + 1, t.Length + 1];
            for (int i = 0; i <= s.Length; i++)
                d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++)
                d[0, j] = j;
            for (int j = 1; j <= t.Length; j++)
                for (int i = 1; i <= s.Length; i++)
                    if (s[i - 1] == t[j - 1])
                        d[i, j] = d[i - 1, j - 1];  //no operation
                    else
                        d[i, j] = Math.Min(Math.Min(
                            d[i - 1, j] + 1,    //a deletion
                            d[i, j - 1] + 1),   //an insertion
                            d[i - 1, j - 1] + 1 //a substitution
                            );
            return d[s.Length, t.Length];
        }
    }
}