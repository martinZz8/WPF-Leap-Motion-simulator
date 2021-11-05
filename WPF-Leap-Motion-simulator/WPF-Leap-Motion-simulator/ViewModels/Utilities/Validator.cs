using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    abstract class Validator
    {
        private readonly static Regex emailRgx = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public static bool IsValidEmail(in string emailToCheck)
        {
            if (emailRgx.IsMatch(emailToCheck))
            {
                return true;
            }

            return false;
        }
    }
}
