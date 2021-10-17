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
        public double CoordX { get; set; }
        public double CoordZ { get; set; }
        public LeapGestureTypes GestrueType { get; set; }
    }
}
