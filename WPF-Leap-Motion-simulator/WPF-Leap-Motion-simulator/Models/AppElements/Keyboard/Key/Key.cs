using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class Key
    {
        //-- Fields --
        private double _positionX;
        private double _positionY;
        private double _width;
        private double _height;
        private KeyTypes _type;
        private string _value;
        private bool _isHovered;
        private string _borderColor = "#000000";
        private string _hoveredBorderColor = "#f1b938";

        //-- Properties --
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

        public double PositionY
        {
            get
            {
                return _positionY;
            }

            set
            {
                _positionY = value;
            }
        }

        public double Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        public double Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
            }
        }

        public KeyTypes Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public bool IsHovered
        {
            get
            {
                return _isHovered;
            }

            set
            {
                _isHovered = value;
            }
        }

        public string BorderColor
        {
            get
            {
                return _borderColor;
            }

            set
            {
                _borderColor = value;
            }
        }

        public string HoveredBorderColor
        {
            get
            {
                return _hoveredBorderColor;
            }

            set
            {
                _hoveredBorderColor = value;
            }
        }

        public string GetBorderColor
        {
            get
            {
                if (!_isHovered)
                {
                    return _borderColor;
                }
                else
                {
                    return _hoveredBorderColor;
                }
            }
        }

        public double SmallWidth
        {
            get
            {
                double smallWidth = _width - 10;
                if (smallWidth <= 0)
                {
                    smallWidth = 1;
                }

                return smallWidth;
            }
        }

        public double SmallHeight
        {
            get
            {
                double smallHeight = _height - 10;
                if(smallHeight <= 0)
                {
                    smallHeight = 1;
                }

                return smallHeight;
            }
        }

        //-- Methods --
        public Boolean IsCursorInsideTheKey(Cursor relativeCursor)
        {
            double cursorCenterX = relativeCursor.PositionX + relativeCursor.CursorRadius;
            double cursorCenterY = relativeCursor.PositionY + relativeCursor.CursorRadius;

            //Console.WriteLine("CCY: " + cursorCenterY + " PT: " + PositionY + " PB: " + (PositionY + Height));
            //Console.WriteLine("CCX: " + cursorCenterX + " PL: " + PositionX + " PR: " + (PositionX + Width));
            if (PositionY <= cursorCenterY && PositionY + Height >= cursorCenterY)
            {
                if (PositionX <= cursorCenterX && PositionX + Width >= cursorCenterX)
                {
                    //cursor is inside the key
                    return true;
                }
            }

            return false;
        }
    }
}
