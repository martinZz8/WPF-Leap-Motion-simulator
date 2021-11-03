using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class Label
    {
        // Fields
        private double _paddingLeftX = 0;
        private double _paddingTopY = 0;
        private double _width = 0;
        private double _height = 10;
        private string _value = "";
        private double _fontSize = 10;
        private string _fontWeight = "Normal";
        private string _textColor = "Black";
        private LabelTypes _type = LabelTypes.NO_LABEL;

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
        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
            }
        }
        public string FontWeight
        {
            get
            {
                return _fontWeight;
            }

            set
            {
                _fontWeight = value;
            }
        }
        public string TextColor
        {
            get
            {
                return _textColor;
            }

            set
            {
                _textColor = value;
            }
        }
        public LabelTypes Type
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

        // Methods

    }
}
