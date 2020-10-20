using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omilab.Extensions
{
    public static class CharExtension
    {
        public static bool IsDigit(this char chr)
        {
            if (chr > 47 && chr < 58) //0-9
                return true;
            else
                return false;
        }

        public static bool IsLetter(this char chr)
        {
            if (chr > 64 && chr < 91) //A-Z
                return true;
            else if (chr > 96 && chr < 123) //a-z
                return true;
            else
                return false;
        }

        public static bool IsOperator(this char chr)
        {
            if (chr == '+' || chr == '-' || chr == '*' || chr == '/'
                || chr == '^' || chr == '=' || chr == '!' || chr == '>'
                || chr == '<' || chr == '&' || chr == '|' || chr == '%')
                return true;
            else
                return false;
        }


        public static bool IsSeparator(char chr)
        {
            if (chr == '(' || chr == ')' || chr == '{' || chr == '}'
              || chr == ',' || chr == ';' || chr == '[' || chr == ']'
              || chr == '<' || chr == '&' || chr == '|' || chr == '%')
                return true;
            else
                return false;
        }


    }//end class
}//end namespace
