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
    class OptionsViewModel: Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleWindowWidth>, IHandle<HandleWindowHeight>, IHandle<HandleCrusorMove>, IHandle<HandleOptionChange>
    {
        private IEventAggregator _eventAggregator;

        // -- Variables of the main window --
        private readonly TDOWindowPadding mainWindowPadding;

        //-- Variables of this window --
        private double[] _gridColumnMultipliers;
        private List<Button> _buttons;
        private List<Label> _labels;

        private readonly double standardLabelHeight = 30;
        private readonly double standardLabelWidth = 80;
        private readonly double extendedLabelWidth = 200;
        private readonly double standardLabelMarginTop = 30;
        private readonly double standardButtonWidth = 200;
        private readonly double standardButtonHeight = 100;
        private readonly double standardButtonMarginLeft = 20;

        private readonly double authorLabelWidth = 120;
        private readonly double additionnalAuthorLabelMarginRight = 18;
        private readonly double additionnalAuthorLabelMarginBottom = 35;

        public OptionsViewModel(IEventAggregator eventAggregator, TDOWindowSize windowSize, TDOWindowPadding windowPadding, TDOActualOptions actualOptions)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            mainWindowPadding = windowPadding;
            _gridColumnMultipliers = new double[] { 1.5, 5, 1.5 };

            double basicPaddingTop = (windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom) * 0.12;
            double titleLabelPaddingLeft = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardLabelWidth) / 2;
            double extendedLabelPaddingLeft = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - extendedLabelWidth - standardButtonWidth - standardButtonMarginLeft) / 2;
            double authorLabelPaddingLeft = ((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - authorLabelWidth - additionnalAuthorLabelMarginRight;
            double authorLabelPaddingTop = windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom - standardLabelHeight - additionnalAuthorLabelMarginBottom;

            _labels = new List<Label>
            {
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = titleLabelPaddingLeft,
                    PaddingTopY = basicPaddingTop,
                    FontSize = 20,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.OPTIONS_TITLE,
                    Value = "Opcje"
                },
                new Label
                {
                    Width = extendedLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = extendedLabelPaddingLeft,
                    PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop,
                    FontSize = 12,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.OPTIONS_SELECTED_HAND,
                    Value = $"Wybrana ręka: {GetHandName(actualOptions.IsRightHandSelected)}"
                },
                new Label
                {
                    Width = authorLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = authorLabelPaddingLeft,
                    PaddingTopY = authorLabelPaddingTop,
                    FontSize = 12,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.OPTIONS_AUTHOR,
                    Value = "Autor: Maciej Harbuz"
                }
            };

            double menuButtonPaddingLeft = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardButtonWidth) / 2;
            double extendedButtonPaddingLeft = extendedLabelPaddingLeft + extendedLabelWidth + standardButtonMarginLeft;
            _buttons = new List<Button> {
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = extendedButtonPaddingLeft,
                    PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop,
                    Type = ButtonTypes.ACTION_CHANGE_HAND,
                    Title = "Zmień rękę",
                    IsHovered = false
                },
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = menuButtonPaddingLeft,
                    PaddingTopY = basicPaddingTop + standardLabelHeight + standardButtonHeight + standardLabelMarginTop*2,
                    Type = ButtonTypes.VIEW_MENU,
                    Title = "Menu",
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

        public Label GetTitleLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.OPTIONS_TITLE);
            }
        }

        public Label GetSelectedHandLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.OPTIONS_SELECTED_HAND);
            }
        }

        public Label GetAuthorLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.OPTIONS_AUTHOR);
            }
        }

        public Button GetChangeHandButton
        {
            get
            {
                return Button.SearchButtonByType(_buttons, ButtonTypes.ACTION_CHANGE_HAND);
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
        public void ChangeHand()
        {
            _eventAggregator.PublishOnUIThread(new HandleOptionsButtonClick
            {
                Type = OptionsButtonClickTypes.CHANGE_HAND
            });
        }
        public void LoadMenuView()
        {
            _eventAggregator.PublishOnUIThread(new HandleOptionsButtonClick
            {
                Type = OptionsButtonClickTypes.MENU
            });
        }

        // Handle window width change
        public void Handle(HandleWindowWidth message)
        {
            double titleLabelPaddingLeft = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardLabelWidth) / 2;
            GetTitleLabel.PaddingLeftX = titleLabelPaddingLeft;
            NotifyOfPropertyChange(() => GetTitleLabel);

            double extendedLabelPaddingLeft = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - extendedLabelWidth - standardButtonWidth - standardButtonMarginLeft) / 2;
            GetSelectedHandLabel.PaddingLeftX = extendedLabelPaddingLeft;
            NotifyOfPropertyChange(() => GetSelectedHandLabel);

            double extendedButtonPaddingLeft = extendedLabelPaddingLeft + extendedLabelWidth + standardButtonMarginLeft;
            GetChangeHandButton.PaddingLeftX = extendedButtonPaddingLeft;
            NotifyOfPropertyChange(() => GetChangeHandButton);

            double menuButtonPaddingLeft = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardButtonWidth) / 2;
            GetMenuButton.PaddingLeftX = menuButtonPaddingLeft;
            NotifyOfPropertyChange(() => GetMenuButton);

            double authorLabelPaddingLeft = ((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - authorLabelWidth - additionnalAuthorLabelMarginRight;
            GetAuthorLabel.PaddingLeftX = authorLabelPaddingLeft;
            NotifyOfPropertyChange(() => GetAuthorLabel);
        }

        // Handle window height change
        public void Handle(HandleWindowHeight message)
        {
            double basicPaddingTop = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.12;

            GetTitleLabel.PaddingTopY = basicPaddingTop;
            NotifyOfPropertyChange(() => GetTitleLabel);

            GetSelectedHandLabel.PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetSelectedHandLabel);

            GetChangeHandButton.PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetChangeHandButton);

            GetMenuButton.PaddingTopY = basicPaddingTop + standardLabelHeight +  standardButtonHeight + standardLabelMarginTop * 2;
            NotifyOfPropertyChange(() => GetMenuButton);

            double authorLabelPaddingTop = message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom - standardLabelHeight - additionnalAuthorLabelMarginBottom;
            GetAuthorLabel.PaddingTopY = authorLabelPaddingTop;
            NotifyOfPropertyChange(() => GetAuthorLabel);
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
                if (GetChangeHandButton.IsCursorInsideTheButton(relativeCursor))
                {
                    ChangeHand();
                }
                else if (GetMenuButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadMenuView();
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

            NotifyOfPropertyChange(() => GetChangeHandButton);
            NotifyOfPropertyChange(() => GetMenuButton);
        }

        // Handle option change (from another class)
        public void Handle(HandleOptionChange message)
        {
            if (message.Type == OptionTypes.SELECTED_HAND)
            {
                GetSelectedHandLabel.Value = $"Wybrana ręka: {GetHandName(message.BoolValue)}";
                NotifyOfPropertyChange(() => GetSelectedHandLabel);
            }
        }

        private string GetHandName(bool isRightHand)
        {
            if (isRightHand)
            {
                return "Prawa";
            }

            return "Lewa";
        }
    }
}
