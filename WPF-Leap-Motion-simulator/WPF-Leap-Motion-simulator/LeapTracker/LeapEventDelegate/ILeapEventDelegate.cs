using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.LeapTracker
{
    public interface ILeapEventDelegate
    {
        void LeapEventNotification(LeapEventTypes EventType);
    }
}
