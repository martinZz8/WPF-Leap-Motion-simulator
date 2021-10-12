using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Caliburn Micro
using Caliburn.Micro;
using WPF_Leap_Motion_simulator.Models;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    public class MenuViewModel: Screen
    {
        private IEventAggregator _eventAggregator;

        //-- Variables of this window --
        private string _testInput;

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        //-- Properties --
        public string TestInput
        {
            get
            {
                return _testInput;
            }
            set
            {
                Console.WriteLine("ICH: " + value);
                _testInput = value;
                _eventAggregator.PublishOnUIThread(new InputField
                {
                    Name = "testInput",
                    Value = _testInput
                });
                NotifyOfPropertyChange(() => TestInput);
            }
        }

        // -- Methods --
        public void LoadReceiveTheParcelView()
        {
            _eventAggregator.PublishOnUIThread(new MenuButtonClick
            {
                Name = "receiveTheParcel"
            });
        }
    }
}
