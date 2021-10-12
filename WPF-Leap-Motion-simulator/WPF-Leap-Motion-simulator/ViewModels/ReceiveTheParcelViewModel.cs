using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Caliburn Micro
using Caliburn.Micro;
using WPF_Leap_Motion_simulator.Models;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class ReceiveTheParcelViewModel: Screen
    {
        private IEventAggregator _eventAggregator;

        //-- Variables of this window --


        public ReceiveTheParcelViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        // -- Methods --
        public void LoadMenuView()
        {
            _eventAggregator.PublishOnUIThread(new MenuButtonClick
            {
                Name = "menu"
            });
        }

    }
}
