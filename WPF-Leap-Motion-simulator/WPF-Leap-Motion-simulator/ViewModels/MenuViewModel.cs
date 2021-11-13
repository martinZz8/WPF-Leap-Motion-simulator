using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows;

// Caliburn Micro
using Caliburn.Micro;

// Models
using WPF_Leap_Motion_simulator.Models;

// Leap Tracker
using WPF_Leap_Motion_simulator.LeapTracker;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class MenuViewModel: Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleWindowWidth>, IHandle<HandleWindowHeight>, IHandle<HandleCrusorMove>
    {
        private IEventAggregator _eventAggregator;

        // -- Variables of the main window --
        private readonly TDOWindowPadding mainWindowPadding;

        //-- Variables of this window --
        private double[] _gridColumnMultipliers;
        private List<Button> _buttons;

        private readonly double standardButtonWidth = 200;
        private readonly double standardButtonHeight = 100;
        private readonly double standardButtonMarginTop = 20;

        private readonly double optionsButtonWidth = 60;
        private readonly double optionsButtonHeight = 60;
        private readonly double optionsButtonMarginTop = 20;
        private readonly double optionsButtonMarginRight = 20;

        public MenuViewModel(IEventAggregator eventAggregator, TDOWindowSize windowSize, TDOWindowPadding windowPadding)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            mainWindowPadding = windowPadding;
            _gridColumnMultipliers = new double[] { 1.5, 5, 1.5 };

            double basicPaddingTopY = (windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom) * 0.25;

            _buttons = new List<Button> {
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = (((windowSize.WindowWidth-windowPadding.PaddingLeft-windowPadding.PaddingRight)*_gridColumnMultipliers[1]/GridColumnTotalDenominator)-standardButtonWidth)/2,
                    PaddingTopY = basicPaddingTopY,
                    Type = ButtonTypes.VIEW_RECEIVE_THE_PARCEL,
                    Title = "Odbierz przesyłkę",
                    IsHovered = false
                },
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = (((windowSize.WindowWidth-windowPadding.PaddingLeft-windowPadding.PaddingRight)*_gridColumnMultipliers[1]/GridColumnTotalDenominator)-standardButtonWidth)/2,
                    PaddingTopY = basicPaddingTopY + standardButtonHeight + standardButtonMarginTop,
                    Type = ButtonTypes.VIEW_SEND_THE_PARCEL_SENDER,
                    Title = "Nadaj przesyłkę",
                    IsHovered = false
                },
                new Button
                {
                    Width = optionsButtonWidth,
                    Height = optionsButtonHeight,
                    PaddingLeftX = ((windowSize.WindowWidth-windowPadding.PaddingLeft-windowPadding.PaddingRight)*_gridColumnMultipliers[1]/GridColumnTotalDenominator) - optionsButtonWidth - optionsButtonMarginRight,
                    PaddingTopY = optionsButtonMarginTop,
                    Type = ButtonTypes.VIEW_OPTIONS,
                    Title = "",
                    IsHovered = false
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
                return Button.SearchButtonByType(_buttons, ButtonTypes.VIEW_SEND_THE_PARCEL_SENDER);
            }
        }

        public Button GetOptionsButton
        {
            get
            {
                return Button.SearchButtonByType(_buttons, ButtonTypes.VIEW_OPTIONS);
            }
        }

        // -- Methods --
        public void LoadReceiveTheParcelView()
        {
            _eventAggregator.PublishOnUIThread(new HandleMenuButtonClick
            {
                Type = MenuButtonClickTypes.RECEIVE_THE_PARCEL
            });
        }

        public void LoadSendTheParcelView()
        {
            _eventAggregator.PublishOnUIThread(new HandleMenuButtonClick
            {
                Type = MenuButtonClickTypes.SEND_THE_PARCEL
            });
        }

        public void LoadOptionsView()
        {
            _eventAggregator.PublishOnUIThread(new HandleMenuButtonClick
            {
                Type = MenuButtonClickTypes.OPTIONS
            });
        }

        // Handle window width change
        public void Handle(HandleWindowWidth message)
        {
            foreach (Button button in _buttons)
            {
                if (button.Type != ButtonTypes.VIEW_OPTIONS)
                {
                    button.PaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - button.Width) / 2;
                }
            }

            NotifyOfPropertyChange(() => GetReceiveTheParcelButton);
            NotifyOfPropertyChange(() => GetSendTheParcelButton);

            GetOptionsButton.PaddingLeftX = ((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - optionsButtonWidth - optionsButtonMarginRight;
            NotifyOfPropertyChange(() => GetOptionsButton);
        }

        // Handle window height change
        public void Handle(HandleWindowHeight message)
        {
            double basicPaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.25;

            Button buttonReceiveTheParcel = _buttons.Find(button => button.Type == ButtonTypes.VIEW_RECEIVE_THE_PARCEL);
            buttonReceiveTheParcel.PaddingTopY = basicPaddingTopY;
            NotifyOfPropertyChange(() => GetReceiveTheParcelButton);

            Button buttonSendTheParcel = _buttons.Find(button => button.Type == ButtonTypes.VIEW_SEND_THE_PARCEL_SENDER);
            buttonSendTheParcel.PaddingTopY = basicPaddingTopY + buttonReceiveTheParcel.Height + standardButtonMarginTop;
            NotifyOfPropertyChange(() => GetSendTheParcelButton);

            GetOptionsButton.PaddingTopY = optionsButtonMarginTop;
            NotifyOfPropertyChange(() => GetOptionsButton);
        }

        // Handle cursor hand gesture
        public void Handle(HandleCursorHandGesture message)
        {
            if (message.GestrueType == LeapGestureTypes.KeyTap)
            {
                // Creting cursor object, that has values relative to the content bar (the black box)
                Cursor relativeCursor = new Cursor
                {
                    CursorRadius = message.CursorRadius,
                    PositionX = message.CursorPositionX - message.PaddingLeft - ((message.WindowWidth - message.PaddingLeft - message.PaddingRight) * _gridColumnMultipliers[0] / GridColumnTotalDenominator),
                    PositionY = message.CursorPositionY - message.PaddingTop
                };

                // Checking if relativeCursor is inside any button in this view
                if (GetReceiveTheParcelButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadReceiveTheParcelView();
                }
                else if (GetSendTheParcelButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadSendTheParcelView();
                }
                else if (GetOptionsButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadOptionsView();
                }
            }
        }

        // Handle cursor move
        public void Handle(HandleCrusorMove message)
        {
            // Creting cursor object, that has values relative to the content bar (the black box)
            Cursor relativeCursor = new Cursor
            {
                CursorRadius = message.CursorRadius,
                PositionX = message.CursorPositionX - message.PaddingLeft - ((message.WindowWidth - message.PaddingLeft - message.PaddingRight) * _gridColumnMultipliers[0] / GridColumnTotalDenominator),
                PositionY = message.CursorPositionY - message.PaddingTop
            };

            // Checking if relativeCursor is inside any button in this view
            foreach(Button button in _buttons)
            {
                if (button.IsCursorInsideTheButton(relativeCursor))
                {
                    button.IsHovered = true;
                }
                else
                {
                    button.IsHovered = false;
                }
            }

            NotifyOfPropertyChange(() => GetReceiveTheParcelButton);
            NotifyOfPropertyChange(() => GetSendTheParcelButton);
            NotifyOfPropertyChange(() => GetOptionsButton);
        }
    }
}
