using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class Button
    {
        // Fields
        private double _paddingLeftX = 0;
        private double _paddingTopY = 0;
        private double _width = 10;
        private double _height = 10;
        private ButtonTypes _type = ButtonTypes.NO_ACTION;
        private string _title = "";

        // Properties
        public double PaddingLeftX {
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
        public ButtonTypes Type
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
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

        // Methods
        public static Button SearchButtonByType(List<Button> ButtonList, ButtonTypes ButtonType)
        {
            return ButtonList.Find(button => button.Type == ButtonType);
        }

        public Boolean IsCursorInsideTheButton(Cursor relativeCursor)
        {
            double cursorCenterX = relativeCursor.PositionX + relativeCursor.CursorRadius;
            double cursorCenterY = relativeCursor.PositionY + relativeCursor.CursorRadius;

            //Console.WriteLine("CCY: "+ cursorCenterY+" PT: "+ PaddingTopY+" PB: "+ (PaddingTopY + Height));
            //Console.WriteLine("CCX: " + cursorCenterX + " PL: " + PaddingLeftX + " PR: " + (PaddingLeftX + Width));
            if (PaddingTopY <= cursorCenterY && PaddingTopY+Height >= cursorCenterY)
            {
                if (PaddingLeftX <= cursorCenterX && PaddingLeftX+Width >= cursorCenterX)
                {
                    //cursor is inside the button
                    return true;
                }
            }

            return false;
        }
    }
}
