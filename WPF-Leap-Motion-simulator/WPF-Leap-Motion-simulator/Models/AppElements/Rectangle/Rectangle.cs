using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class Rectangle
    {
        // Fields
        private double _paddingLeftX = 0;
        private double _paddingTopY = 0;
        private double _width = 10;
        private double _height = 10;
        private string _backgroundColor = "#ffffff";
        private double _radius = 0;
        private double _opacity = 1;

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

        public string BackgroudColor
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

        public double Radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = value;
            }
        }

        public double Opacity
        {
            get
            {
                return _opacity;
            }

            set
            {
                _opacity = value;
            }
        }
    }
}
