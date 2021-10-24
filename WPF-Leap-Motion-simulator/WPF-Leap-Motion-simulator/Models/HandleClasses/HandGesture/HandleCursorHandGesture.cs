using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Leap Tracker
using WPF_Leap_Motion_simulator.LeapTracker;

namespace WPF_Leap_Motion_simulator.Models
{
    class HandleCursorHandGesture
    {
        public double CursorPositionX { get; set; }
        public double CursorPositionY { get; set; }
        public double CursorRadius { get; set; }
        public LeapGestureTypes GestrueType { get; set; }
        public double PaddingTop { get; set; }
        public double  PaddingRight { get; set; }
        public double  PaddingBottom { get; set; }
        public double PaddingLeft { get; set; }
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public Boolean IsKeyboardVisible { get; set; }
    }
}
