using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Caliburn Micro
using Caliburn.Micro;

// Models
using WPF_Leap_Motion_simulator.Models;

// Leap Tracker
using WPF_Leap_Motion_simulator.LeapTracker;

// Static Classes
using WPF_Leap_Motion_simulator.StaticClasses;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class MenuViewModel: Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleSendButtonData>
    {
        private IEventAggregator _eventAggregator;

        //-- Variables of this window --
        private string _testInput;

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            //DialogEventAggregatorProvider.eventAggregator.Subscribe(this);
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
                _testInput = value;
                _eventAggregator.PublishOnUIThread(new HandleInputField
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
            _eventAggregator.PublishOnUIThread(new HandleMenuButtonClick
            {
                Name = "receiveTheParcel"
            });
        }

        // Handle cursor hand gesture
        public void Handle(HandleCursorHandGesture message)
        {
            if(message.GestrueType == LeapGestureTypes.KeyTap)
            {
                Console.WriteLine("I'm here!!");
                DialogEventAggregatorProvider.eventAggregator.Publish(
                    new HandleRequestButtonData { ButtonType = ButtonTypes.RECEIVE_THE_PARCEL },
                    action => { }
                );
                //LoadReceiveTheParcelView();
            }
        }

        // Handle SendButtonData
        public void Handle(HandleSendButtonData message)
        {
            List<ButtonData> buttonList = message.ButtonList;
            if(buttonList.Count > 0)
            {
                Console.WriteLine("First element's X in buttonList: " + buttonList[0].RelativePositionX);
            }
        }
    }
}
