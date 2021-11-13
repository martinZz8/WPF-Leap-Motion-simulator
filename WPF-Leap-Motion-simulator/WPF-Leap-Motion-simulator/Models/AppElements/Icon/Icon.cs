using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class Icon
    {
        // Fields
        private double _paddingLeftX = 0;
        private double _paddingTopY = 0;
        private double _width = 30;
        private double _height = 50;
        private IconTypes _type = IconTypes.NO_ICON;
        private string _label = "";
        private double _labelHeight = 20;
        private double _labelMarginTop = 0;
        private bool _isVisible = false;

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
        public double LabelPaddingLeftX
        {
            get
            {
                return _paddingLeftX;
            }
        }
        public double LabelPaddingTopY
        {
            get
            {
                return _paddingTopY + Height - _labelHeight + _labelMarginTop;
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
        public IconTypes Type
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
        public string Label
        {
            get
            {
                return _label;
            }

            set
            {
                _label = value;
            }
        }
        public double IconWidth
        {
            get
            {
                return _width;
            }
        }
        public double IconHeight
        {
            get
            {
                return _height - _labelHeight;
            }
        }
        public double LabelWidth
        {
            get
            {
                return _width;
            }
        }
        public double LabelHeight
        {
            get
            {
                return _labelHeight;
            }
        }
        public double LabelMarginTop
        {
            get
            {
                return _labelMarginTop;
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
        public string GetVisibilityType
        {
            get
            {
                if (_isVisible)
                {
                    return "Visible";
                }

                return "Collapsed";
            }
        }
    }
}
