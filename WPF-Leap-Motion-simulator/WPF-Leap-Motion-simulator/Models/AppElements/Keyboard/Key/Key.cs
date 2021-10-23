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

        //-- Methods --
        public Boolean IsCursorInsideTheKey(Cursor relativeCursor)
        {
            double cursorCenterX = relativeCursor.PositionX + relativeCursor.CursorRadius;
            double cursorCenterY = relativeCursor.PositionY + relativeCursor.CursorRadius;

            Console.WriteLine("CCY: " + cursorCenterY + " PT: " + PositionY + " PB: " + (PositionY + Height));
            Console.WriteLine("CCX: " + cursorCenterX + " PL: " + PositionX + " PR: " + (PositionX + Width));
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
