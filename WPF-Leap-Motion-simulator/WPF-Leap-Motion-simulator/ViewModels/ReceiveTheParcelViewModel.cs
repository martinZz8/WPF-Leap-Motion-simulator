using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

// Caliburn Micro
using Caliburn.Micro;

// Tracker
using WPF_Leap_Motion_simulator.LeapTracker;

// Models
using WPF_Leap_Motion_simulator.Models;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class ReceiveTheParcelViewModel: Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleWindowWidth>, IHandle<HandleWindowHeight>, IHandle<HandleKeyClick>
    {
        private IEventAggregator _eventAggregator;

        // -- Variables of the main window --
        private readonly TDOWindowPadding mainWindowPadding;

        //-- Variables of this window --
        private double[] _gridColumnMultipliers;
        private List<Button> _buttons;
        private List<Input> _inputs;
        private InputTypes _focusedInput;

        public ReceiveTheParcelViewModel(IEventAggregator eventAggregator, TDOWindowSize windowSize, TDOWindowPadding windowPadding)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            mainWindowPadding = windowPadding;
            _gridColumnMultipliers = new double[] { 1.5, 5, 1.5 };
            _focusedInput = InputTypes.NO_INPUT;

            double standardInputWidth = 200;
            double standardInputHeight = 100;

            _inputs = new List<Input>
            {
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = (((windowSize.WindowWidth-windowPadding.PaddingLeft-windowPadding.PaddingRight)*_gridColumnMultipliers[1]/GridColumnTotalDenominator)-standardInputWidth)/2,
                    PaddingTopY = (windowSize.WindowHeight-windowPadding.PaddingTop-windowPadding.PaddingBottom)*0.15,
                    Type = InputTypes.RECEIVE_SMS_CODE,
                    Value = ""
                }
            };

            double standardButtonWidth = 200;
            double standardButtonHeight = 100;

            _buttons = new List<Button>
            {
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = (((windowSize.WindowWidth-windowPadding.PaddingLeft-windowPadding.PaddingRight)*_gridColumnMultipliers[1]/GridColumnTotalDenominator)-standardButtonWidth)/2,
                    PaddingTopY = (windowSize.WindowHeight-windowPadding.PaddingTop-windowPadding.PaddingBottom)*0.15 + standardInputHeight + 20,
                    Type = ButtonTypes.VIEW_MENU,
                    Title = "Menu"
                }
            };
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        //-- Properties --
        public double GridColumnTotalDenominator
        {
            get
            {
                return _gridColumnMultipliers.Aggregate(0d, (total, next) => total + next); ;
            }
        }

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

        public string PropSMSCodeInput
        {
            get
            {
                return GetSMSCodeInput.Value;
            }
            set
            {
                GetSMSCodeInput.Value = value;
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.RECEIVE_SMS_CODE,
                    Value = GetSMSCodeInput.Value
                });
                NotifyOfPropertyChange(() => GetSMSCodeInput);
                NotifyOfPropertyChange(() => PropSMSCodeInput);
            }
        }

        public Input GetSMSCodeInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.RECEIVE_SMS_CODE);
            }
        }

        public Button GetMenuButton
        {
            get
            {
                return _buttons.Find(button => button.Type == ButtonTypes.VIEW_MENU);
            }
        }

        // -- Methods --
        public void LoadMenuView()
        {
            HideKeyboard();

            _eventAggregator.PublishOnUIThread(new HandleReceiveTheParcelButtonClick
            {
                Name = "menu"
            });
        }

        // Handle window width change
        public void Handle(HandleWindowWidth message)
        {
            foreach (Input input in _inputs)
            {
                input.PaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - input.Width) / 2;
            }
            NotifyOfPropertyChange(() => GetSMSCodeInput);

            foreach (Button button in _buttons)
            {
                button.PaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - button.Width) / 2;
            }
            NotifyOfPropertyChange(() => GetMenuButton);
        }

        // Handle window height change
        public void Handle(HandleWindowHeight message)
        {
            Input inputSMSCode = GetSMSCodeInput;
            inputSMSCode.PaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.15;
            NotifyOfPropertyChange(() => GetSMSCodeInput);

            Button buttonMenu = GetMenuButton;
            buttonMenu.PaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.15 + inputSMSCode.Height + 20;
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

                // Checking if relativeCursor is inside any input in this view
                Input inputSMSCode = GetSMSCodeInput;
                if (inputSMSCode.IsCursorInsideTheInput(relativeCursor))
                {
                    if(!message.IsKeyboardVisible)
                    {
                        _focusedInput = inputSMSCode.Type;
                    }
                    else
                    {
                        _focusedInput = InputTypes.NO_INPUT;
                    }
                    
                    _eventAggregator.PublishOnUIThread(new HandleKeyboardChange
                    {
                        Type = KeyboardTypes.NUMERIC,
                        IsToggle = true
                    });
                }

                // Checking if relativeCursor is inside any button in this view
                Button buttonMenu = GetMenuButton;
                if (buttonMenu.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadMenuView();
                }

                // TO DO - check other buttons and inputs in this view

            }
        }

        // Handle keyboard key click
        public void Handle(HandleKeyClick message)
        {
            Input inputToChange = null;
            bool hasValueChanged = false;

            if (_focusedInput == InputTypes.RECEIVE_SMS_CODE)
            {
                inputToChange = GetSMSCodeInput;

                if (message.KeyType != KeyTypes.DELETE)
                {
                    inputToChange.Value += message.keyToAdd;
                    hasValueChanged = true;
                }
                else
                {
                    if (inputToChange.Value.Length > 0)
                    {
                        inputToChange.Value = inputToChange.Value.Substring(0, inputToChange.Value.Length - 1);
                        hasValueChanged = true;
                    }
                }
            }
            // TO DO - handle other inputs in this view


            if (hasValueChanged)
            {
                if(inputToChange.Type == InputTypes.RECEIVE_SMS_CODE)
                {
                    NotifyOfPropertyChange(() => GetSMSCodeInput);
                    NotifyOfPropertyChange(() => PropSMSCodeInput);
                    Console.WriteLine("Change of sms input: " + GetSMSCodeInput.Value);
                }
                //other input types notifies
            }
        }

        private void HideKeyboard()
        {
            _focusedInput = InputTypes.NO_INPUT;

            _eventAggregator.PublishOnUIThread(new HandleKeyboardChange
            {
                IsRestoreDefault = true
            });
        }
    }
}
