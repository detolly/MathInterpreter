using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    static class Extensions
    {
        public static string repl(this string str, char one, char two)
        {
            int first = str.IndexOf(one);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if (i == first)
                {
                    builder.Append(two);
                    continue;
                }
                builder.Append(str[i]);
            }
            return builder.ToString();
        }
        
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string inv(this string str)
        {
            var returnString = new StringBuilder();
            for (int i = str.Length-1; i >= 0; i--)
            {
                returnString.Append(str[i]);
            }
            return returnString.ToString();
        }

        public static bool isDigit(this string str)
        {
            try {
                int.Parse(str);
            } catch {
                return false;
            }
            return true;
        }


    }
}
