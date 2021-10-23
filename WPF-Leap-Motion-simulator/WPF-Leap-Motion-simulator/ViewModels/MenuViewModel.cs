using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows;

//Caliburn Micro
using Caliburn.Micro;

// Models
using WPF_Leap_Motion_simulator.Models;

// Leap Tracker
using WPF_Leap_Motion_simulator.LeapTracker;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class MenuViewModel: Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleWindowWidth>, IHandle<HandleWindowHeight>
    {
        private IEventAggregator _eventAggregator;

        // -- Variables of the main window --
        private readonly TDOWindowPadding mainWindowPadding;

        //-- Variables of this window --
        private double[] _gridColumnMultipliers;
        private List<Button> _buttons;
        private string _testInput;

        public MenuViewModel(IEventAggregator eventAggregator, TDOWindowSize windowSize, TDOWindowPadding windowPadding)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            mainWindowPadding = windowPadding;
            _gridColumnMultipliers = new double[] { 1.5, 5, 1.5 };

            double standardButtonWidth = 140;
            _buttons = new List<Button> {
                new Button
                {
                    Width = standardButtonWidth,
                    Height = 30,
                    PaddingLeftX = (((windowSize.WindowWidth-windowPadding.PaddingLeft-windowPadding.PaddingRight)*_gridColumnMultipliers[1]/GridColumnTotalDenominator)-standardButtonWidth)/2,
                    PaddingTopY = (windowSize.WindowHeight-windowPadding.PaddingTop)*0.3,
                    Type = ButtonTypes.VIEW_RECEIVE_THE_PARCEL,
                    Title = "Odbierz przesyłkę"
                },
                new Button
                {
                    Width = standardButtonWidth,
                    Height = 30,
                    PaddingLeftX = (((windowSize.WindowWidth-windowPadding.PaddingLeft-windowPadding.PaddingRight)*_gridColumnMultipliers[1]/GridColumnTotalDenominator)-standardButtonWidth)/2,
                    PaddingTopY = (windowSize.WindowHeight-windowPadding.PaddingTop)*0.3 + 30 + 20,
                    Type = ButtonTypes.VIEW_RECEIVE_THE_PARCEL,
                    Title = "Nadaj przesyłkę"
                }
            };
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        //-- Properties --
        public string GetGridFirstColumnMultiplier
        {
            get
            {
                if(_gridColumnMultipliers.Length >= 1)
                {
                    return _gridColumnMultipliers[0].ToString(CultureInfo.GetCultureInfo("en-US")) + "*";
                }

                return "1*";
            }
        }
        public string GetGridSecondColumnMultiplier
        {
            get
            {
                if (_gridColumnMultipliers.Length >= 2)
                {
                    return _gridColumnMultipliers[1].ToString(CultureInfo.GetCultureInfo("en-US")) + "*";
                }

                return "1*";
            }
        }
        public string GetGridThirdColumnMultiplier
        {
            get
            {
                if (_gridColumnMultipliers.Length >= 3)
                {
                    return _gridColumnMultipliers[2].ToString(CultureInfo.GetCultureInfo("en-US")) + "*";
                }

                return "1*";
            }
        }
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

        public double GridColumnTotalDenominator
        {
            get
            {
                return _gridColumnMultipliers.Aggregate(0d, (total, next) => total + next); ;
            }
        }

        public Button GetReceiveTheParcelButton
        {
            get
            {
                return Button.SearchButtonByType(_buttons, ButtonTypes.VIEW_RECEIVE_THE_PARCEL);
            }
        }

        public Button GetSendTheParcelButton
        {
            get
            {
                return Button.SearchButtonByType(_buttons, ButtonTypes.VIEW_SEND_THE_PARCEL);
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

        public void LoadSendTheParcelView()
        {
            // TO DO - use publish in event aggrgator to change window to send parcle view

        }

        // Handle cursor hand gesture
        public void Handle(HandleCursorHandGesture message)
        {
            if(message.GestrueType == LeapGestureTypes.KeyTap)
            {
                // Creting cursor object, that has values relative to the content bar (the black box)
                Cursor relativeCursor = new Cursor
                {
                    CursorRadius = message.CursorRadius,
                    PositionX = message.CursorPositionX - message.PaddingLeft - ((message.WindowWidth - message.PaddingLeft - message.PaddingRight) * _gridColumnMultipliers[0] / GridColumnTotalDenominator),
                    PositionY = message.CursorPositionY - message.PaddingTop
                };

                // Checking if relativeCursor is inside any button in this view
                Button buttonReceiveTheParcel = _buttons.Find(button => button.Type == ButtonTypes.VIEW_RECEIVE_THE_PARCEL);
                if (buttonReceiveTheParcel.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadReceiveTheParcelView();
                }

                // TO DO - check other buttons in this view
                
            }
        }

        // Handle window width change
        public void Handle(HandleWindowWidth message)
        {
            foreach (Button button in _buttons)
            {
                button.PaddingLeftX = (((message.WindowWidth- mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - button.Width) / 2;
            }

            NotifyOfPropertyChange(() => GetReceiveTheParcelButton);
            NotifyOfPropertyChange(() => GetSendTheParcelButton);
        }

        // Handle window height change
        public void Handle(HandleWindowHeight message)
        {
            Button buttonReceiveTheParcel = _buttons.Find(button => button.Type == ButtonTypes.VIEW_RECEIVE_THE_PARCEL);
            buttonReceiveTheParcel.PaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop) * 0.3;

            Button buttonSendTheParcel = _buttons.Find(button => button.Type == ButtonTypes.VIEW_SEND_THE_PARCEL);
            buttonSendTheParcel.PaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop) * 0.3 + buttonReceiveTheParcel.Height + 20;

            NotifyOfPropertyChange(() => GetReceiveTheParcelButton);
            NotifyOfPropertyChange(() => GetSendTheParcelButton);
        }
    }
}
