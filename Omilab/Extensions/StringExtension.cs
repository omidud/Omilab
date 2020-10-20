using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omilab.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Return the first character of the string into Uppercase.
        /// </summary>
        /// <param name="input">example: "john"</param>
        /// <returns>example: return "John"</returns>
        public static string Capitalize(this string input)
        {
            string output = input;
            char[] charArray = output.ToCharArray();
            char firstLetter = charArray[0];

            if (firstLetter >= 97 && firstLetter <= 122) //si es minuscula
            {
                string upperLetter = firstLetter.ToString().ToUpper();
                output = output.Substring(1);
                output = upperLetter + output;
            }

            return output;
        }

        /// <summary>
        /// return true is the input string is numeric
        /// </summary>
        /// <param name="input"></param>
        /// <returns>true it is numeric</returns>
        public static bool IsNumeric(this string input)
        {
            double outDouble = 0;
            return double.TryParse(input, out outDouble);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">example:  "<title>my title</title>" </param>
        /// <param name="startString">Example: "<title"</param>
        /// <param name="endString">Example: "</title>"</param>
        /// <returns>Example:  "my title"</returns>
        public static string StringBetween(this String str, string startString, string endString)
        {
            string retorna = "";
            int start = str.IndexOf(startString);

            if (start > -1)
            {
                start += startString.Length;
                int end = str.LastIndexOf(endString);
                if (end > -1)
                {
                    retorna = str.Substring(start, end - start);
                }
            }

            return retorna;

        }

    }//end class
}//end namespace
