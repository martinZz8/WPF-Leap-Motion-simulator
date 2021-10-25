using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    abstract class StringFormat
    {
        public static string CovertToCodeFormat(string strToConvert)
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

        public static string CovertToPhoneFormat(string strToConvert)
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
    }
}
