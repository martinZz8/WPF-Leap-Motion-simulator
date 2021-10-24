using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class Input
    {
        // Fields
        private double _paddingLeftX = 0;
        private double _paddingTopY = 0;
        private double _width = 10;
        private double _height = 10;
        private InputTypes _type = InputTypes.NO_INPUT;
        private string _value = "";

        // Properties
        public double PaddingLeftX
        {
            get
            {
                return _paddingLeftX;
            }

            set
            {
                _paddingLeftX = value;
            }
        }
        public double PaddingTopY
        {
            get
            {
                return _paddingTopY;
            }

            set
            {
                _paddingTopY = value;
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
        public InputTypes Type
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

        // Methods
        public static Input SearchInputByType(List<Input> InputList, InputTypes InputType)
        {
            return InputList.Find(input => input.Type == InputType);
        }

        public Boolean IsCursorInsideTheInput(Cursor relativeCursor)
        {
            double cursorCenterX = relativeCursor.PositionX + relativeCursor.CursorRadius;
            double cursorCenterY = relativeCursor.PositionY + relativeCursor.CursorRadius;

            //Console.WriteLine("CCY: " + cursorCenterY + " PT: " + PaddingTopY + " PB: " + (PaddingTopY + Height));
            //Console.WriteLine("CCX: " + cursorCenterX + " PL: " + PaddingLeftX + " PR: " + (PaddingLeftX + Width));
            if (PaddingTopY <= cursorCenterY && PaddingTopY + Height >= cursorCenterY)
            {
                if (PaddingLeftX <= cursorCenterX && PaddingLeftX + Width >= cursorCenterX)
                {
                    //cursor is inside the cursor
                    return true;
                }
            }

            return false;
        }
    }
}
