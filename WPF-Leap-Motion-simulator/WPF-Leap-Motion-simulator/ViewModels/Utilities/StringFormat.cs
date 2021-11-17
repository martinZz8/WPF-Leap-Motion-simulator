using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    abstract class StringFormat
    {
        public static string ConvertToCodeFormat(string strToConvert)
        {
            string convertedStr = "";
            int strToConvertLength = strToConvert.Length;

            for (int i = 1; i <= strToConvertLength; i++)
            {
                convertedStr += strToConvert[i - 1];

                if ((i % 2 == 0) && (i != strToConvertLength))
                {
                    convertedStr += " ";
                }
            }

            return convertedStr;
        }

        public static string ConvertToPhoneFormat(string strToConvert)
        {
            string convertedStr = "";
            int strToConvertLength = strToConvert.Length;

            for (int i = 1; i <= strToConvertLength; i++)
            {
                convertedStr += strToConvert[i - 1];

                if ((i % 3 == 0) && (i != strToConvertLength))
                {
                    convertedStr += "-";
                }
            }

            return convertedStr;
        }

        public static string ConvertToPostCodeFormat(string strToConvert)
        {
            string convertedStr = "";
            int strToConvertLength = strToConvert.Length;

            for (int i = 1; i <= strToConvertLength; i++)
            {
                convertedStr += strToConvert[i - 1];

                if (i == 2)
                {
                    convertedStr += "-";
                }
            }

            return convertedStr;
        }

        public static string ConvertToNameFormat(string strToConvert)
        {
            if (strToConvert.Length > 0)
            {
                return char.ToUpper(strToConvert[0]) + strToConvert.Substring(1).ToLower();
            }

            return strToConvert;
        }

        public static string ConvertToFirstUpperCaseNameFormat(string strToConvert)
        {
            if (strToConvert.Length > 0)
            {
                string[] splittedStrings = strToConvert.Split(new char[] { Convert.ToChar(" ") });

                if (splittedStrings.Length > 1)
                {
                    string[] upperCaseSplittedStrings = new string[splittedStrings.Length];
                    for (int i = 0; i < splittedStrings.Length; i++)
                    {
                        upperCaseSplittedStrings[i] = ConvertToNameFormat(splittedStrings[i]);
                    }

                    return String.Join(" ", upperCaseSplittedStrings);
                }
                else
                {
                    return ConvertToNameFormat(strToConvert);
                }
            }

            return strToConvert;
        }
    }
}
