using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class Cursor
    {
        // Fields
        private double _positionX;
        private double _positionZ;
        private bool _isVisible;
        private double _cursorRadius = 9;

        // Properties
        public double CursorRadius
        {
            get
            {
                return _cursorRadius;
            }
        }

        public double PositionX
        {
            get
            {
                return _positionX;
            }

            set
            {
                _positionX = value;
            }
        }

        public double PositionZ
        {
            get
            {
                return _positionZ;
            }

            set
            {
                _positionZ = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }

            set
            {
                _isVisible = value;
            }
        }

        public string getVisibilityType
        {
            get
            {
                if (IsVisible)
                    return "Visible";
                return "Collapsed";
            }
        }
    }
}
