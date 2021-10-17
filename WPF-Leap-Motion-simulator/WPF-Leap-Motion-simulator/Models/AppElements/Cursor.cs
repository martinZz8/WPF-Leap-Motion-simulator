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
        private double _cursorSensibility = 1;
        private double _cursorTapSizeAddition = 5;

        // Properties

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

        public double CursorRadius
        {
            get
            {
                return _cursorRadius;
            }

            set
            {
                _cursorRadius = value;
            }
        }

        public double CursorSensibility
        {
            get
            {
                return _cursorSensibility;
            }

            set
            {
                _cursorSensibility = value;
            }
        }

        public string GetVisibilityType
        {
            get
            {
                if (IsVisible)
                    return "Visible";
                return "Collapsed";
            }
        }

        public double GetCursorSize
        {
            get
            {
                return _cursorRadius*2;
            }
        }

        public double GetCursorTapSize
        {
            get
            {
                return (_cursorRadius * 2) + _cursorTapSizeAddition;
            }
        }

        public double TapPositionX
        {
            get
            {
                return _positionX - (_cursorTapSizeAddition / 2);
            }
        }

        public double TapPositionZ
        {
            get
            {
                return _positionZ - (_cursorTapSizeAddition / 2);
            }
        }

        // Methods
        public bool areCoordsInCursor(double x, double z)
        {
            // Check if cursor is visible - if not, coords cannot be inside the cursor
            if (!_isVisible)
            {
                return false;
            }

            double centerX = _positionX + _cursorRadius;
            double centerZ = _positionZ + _cursorRadius;

            double absDiffX = Math.Abs(x - centerX);
            double absDiffZ = Math.Abs(z - centerZ);

            if ((absDiffX > _cursorRadius) && (absDiffZ > _cursorRadius))
            {
                return false;
            }

            return true;
        }
    }
}
