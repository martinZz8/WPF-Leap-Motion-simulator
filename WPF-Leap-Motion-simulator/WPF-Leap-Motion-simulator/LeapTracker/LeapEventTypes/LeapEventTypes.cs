using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.LeapTracker
{
    public enum LeapEventTypes
    {
        onInit,
        onConnect,
        onFrame,
        onExit,
        onDisconnect,
        onCircleGestureDetected,
        onSwipeGestureDetected,
        onScreenTapGestureDetected,
        onKeyTapGestureDetected,
        onNoGestureDetected
    }
}