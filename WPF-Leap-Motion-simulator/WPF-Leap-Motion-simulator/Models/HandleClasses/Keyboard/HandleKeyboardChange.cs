using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class HandleKeyboardChange
    {
        //-- Fields --
        private Boolean _isVisible = false;
        private Boolean _isToggle = false;
        private Boolean _isRestoreDefault = false;

        //-- Properties --
        public KeyboardTypes Type { get; set; }
        public Boolean IsVisible {
            get
            {
                return _isVisible;
            }

            set
            {
                _isVisible = value;
            }
        }
        public Boolean IsToggle {
            get
            {
                return _isToggle;
            }

            set
            {
                _isToggle = value;
            } 
        }

        public Boolean IsRestoreDefault
        {
            get
            {
                return _isRestoreDefault;
            }

            set
            {
                _isRestoreDefault = value;
            }
        }
    }
}
