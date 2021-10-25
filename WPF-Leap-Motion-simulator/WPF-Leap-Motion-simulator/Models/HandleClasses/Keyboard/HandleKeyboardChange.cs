using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class HandleKeyboardChange
    {
        //-- Fields --
        private bool _isVisible = false; //3rd place
        private bool _isToggle = false; //3rd place
        private bool _isInputChange = false; //2nd place
        private bool _isRestoreDefault = false; //1st place

        //-- Properties --
        public KeyboardTypes Type { get; set; }
        public bool IsVisible {
            get
            {
                return _isVisible;
            }

            set
            {
                _isVisible = value;
            }
        }
        public bool IsToggle {
            get
            {
                return _isToggle;
            }

            set
            {
                _isToggle = value;
            } 
        }

        public bool IsInputChange
        {
            get
            {
                return _isInputChange;
            }

            set
            {
                _isInputChange = value;
            }
        }

        public bool IsRestoreDefault
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
