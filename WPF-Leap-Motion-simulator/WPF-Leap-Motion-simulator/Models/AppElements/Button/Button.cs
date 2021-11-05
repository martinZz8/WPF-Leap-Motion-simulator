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
        private bool _isHovered = false;
        private string _backgroundColor = "#4d4d4d";
        private string _foregroundColor = "#f1b938";
        private string _borderColor = "#999999";
        private string _hoveredBackgroundColor = "#f1b938";
        private string _hoveredForegroundColor = "#4d4d4d";
        private string _hoveredBorderColor = "#f8dea0";
        private bool _isVisible = true;

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

        public string HoveredBackgroundColor
        {
            get
            {
                return _hoveredBackgroundColor;
            }

            set
            {
                _hoveredBackgroundColor = value;
            }
        }

        public string HoveredForegroundColor
        {
            get
            {
                return _hoveredForegroundColor;
            }

            set
            {
                _hoveredForegroundColor = value;
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

        public string GetBackgroundColor
        {
            get
            {
                if (!_isHovered)
                {
                    return _backgroundColor;
                }
                else
                {
                    return _hoveredBackgroundColor;
                }
            }
        }

        public string GetForegroundColor
        {
            get
            {
                if (!_isHovered)
                {
                    return _foregroundColor;
                }
                else
                {
                    return _hoveredForegroundColor;
                }
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
