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
        private bool _isFocused = false;
        private bool _isVisible = true;
        private string _backgroundColor = "#fdf7e7";
        private string _foregroundColor = "#000000";
        private string _borderColor = "#4d4d4d";
        private string _focusedBorderColor = "#f1b938";

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

        public bool IsFocused
        {
            get
            {
                return _isFocused;
            }

            set
            {
                _isFocused = value;
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

        public string BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }

            set
            {
                _backgroundColor = value;
            }
        }

        public string ForegroundColor
        {
            get
            {
                return _foregroundColor;
            }

            set
            {
                _foregroundColor = value;
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

        public string FocusedBorderColor
        {
            get
            {
                return _focusedBorderColor;
            }

            set
            {
                _focusedBorderColor = value;
            }
        }

        public string GetBackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
        }

        public string GetForegroundColor
        {
            get
            {
                return _foregroundColor;
            }
        }

        public string GetBorderColor
        {
            get
            {
                if (!_isFocused)
                {
                    return _borderColor;
                }
                else
                {
                    return _focusedBorderColor;
                }
                
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
