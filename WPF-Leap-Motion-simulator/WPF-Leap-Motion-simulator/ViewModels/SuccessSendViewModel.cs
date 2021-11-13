using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

// Caliburn Micro
using Caliburn.Micro;

// Models
using WPF_Leap_Motion_simulator.Models;

// Leap Tracker
using WPF_Leap_Motion_simulator.LeapTracker;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class SuccessSendViewModel: Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleWindowWidth>, IHandle<HandleWindowHeight>, IHandle<HandleCrusorMove>
    {
        private IEventAggregator _eventAggregator;

        // -- Variables of the main window --
        private readonly TDOWindowPadding mainWindowPadding;

        //-- Variables of this window --
        private double[] _gridColumnMultipliers;
        private List<Button> _buttons;
        private List<Label> _labels;

        private readonly double standardLabelHeight = 40;
        private readonly double standardLabelWidth = 600;
        private readonly double standardLabelMarginTop = 30;
        private readonly double standardButtonWidth = 200;
        private readonly double standardButtonHeight = 100;

        public SuccessSendViewModel(IEventAggregator eventAggregator, TDOWindowSize windowSize, TDOWindowPadding windowPadding)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            mainWindowPadding = windowPadding;
            _gridColumnMultipliers = new double[] { 1.5, 5, 1.5 };

            double basicPaddingTop = (windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom) * 0.30;
            double labelPaddingLeft = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardLabelWidth) / 2;

            _labels = new List<Label>
            {
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = labelPaddingLeft,
                    PaddingTopY = basicPaddingTop,
                    FontSize = 20,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SUCCESS_SEND_MESSAGE,
                    Value = "Włóż paczkę do otworzonej skrytki, zapłać i potwierdź nadanie."
                }
            };

            _buttons = new List<Button> {
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = (((windowSize.WindowWidth-windowPadding.PaddingLeft-windowPadding.PaddingRight)*_gridColumnMultipliers[1]/GridColumnTotalDenominator)-standardButtonWidth)/2,
                    PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop,
                    Type = ButtonTypes.VIEW_MENU,
                    Title = "Kończymy na dziś",
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
                if (_gridColumnMultipliers.Length >= 1)
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

        public Label GetMessageLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SUCCESS_SEND_MESSAGE);
            }
        }

        public Button GetMenuButton
        {
            get
            {
                return Button.SearchButtonByType(_buttons, ButtonTypes.VIEW_MENU);
            }
        }

        // -- Methods --
        public void LoadMenuView()
        {
            _eventAggregator.PublishOnUIThread(new HandleSendTheParcelButtonClick
            {
                Type = SendTheParcelButtonClickTypes.MENU
            });
        }

        // Handle window width change
        public void Handle(HandleWindowWidth message)
        {
            foreach (Label label in _labels)
            {
                label.PaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - label.Width) / 2;
            }
            NotifyOfPropertyChange(() => GetMessageLabel);

            GetMenuButton.PaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardButtonWidth) / 2;
            NotifyOfPropertyChange(() => GetMenuButton);
        }

        // Handle window height change
        public void Handle(HandleWindowHeight message)
        {
            double basicPaddingTop = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.30;

            GetMessageLabel.PaddingTopY = basicPaddingTop;
            NotifyOfPropertyChange(() => GetMessageLabel);

            GetMenuButton.PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetMenuButton);
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
                Button buttonMenu = GetMenuButton;
                if (buttonMenu.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadMenuView();
                }
            }
            else if (message.GestrueType == LeapGestureTypes.Swipe)
            {
                LoadMenuView();
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
            foreach (Button button in _buttons)
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

            NotifyOfPropertyChange(() => GetMenuButton);
        }
    }
}
