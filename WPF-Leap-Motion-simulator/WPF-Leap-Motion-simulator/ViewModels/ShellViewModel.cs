using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Caliburn Micro
using Caliburn.Micro;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class ShellViewModel: Caliburn.Micro.Screen
    {
        private string _FPSCounter;

        public string FPSCounter
        {
            get
            {
                return _FPSCounter;
            }

            set
            {
                _FPSCounter = value;
                Console.WriteLine("Changing to: " + value);
                NotifyOfPropertyChange(() => FPSCounter);
            }
        }
    }
}
