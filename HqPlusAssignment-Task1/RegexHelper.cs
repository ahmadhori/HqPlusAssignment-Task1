using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HqPlusAssignment_Task1
{
    class RegexHelper
    {
        public static string getFirstMatchValueOrNull(string text, string regexString) 
        {
            Regex regex = new Regex(regexString, RegexOptions.None);
            MatchCollection Matches = regex.Matches(text);
            if (Matches.Count > 0)
            {
                string Val = Matches[0].Value;
                return Val;
            }
            else
            {
                return null;
            }
        }
    }
}
