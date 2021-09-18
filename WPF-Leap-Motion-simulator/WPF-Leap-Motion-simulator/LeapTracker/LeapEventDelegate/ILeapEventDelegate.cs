using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.LeapTracker.LeapEventDelegate
{
    public interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }
}
