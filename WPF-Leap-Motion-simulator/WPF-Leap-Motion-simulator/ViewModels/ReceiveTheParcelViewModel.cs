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
            double standardInputHeight = 50;

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
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = (((windowSize.WindowWidth-windowPadding.PaddingLeft-windowPadding.PaddingRight)*_gridColumnMultipliers[1]/GridColumnTotalDenominator)-standardInputWidth)/2,
                    PaddingTopY = (windowSize.WindowHeight-windowPadding.PaddingTop-windowPadding.PaddingBottom)*0.15 + standardInputHeight + 20,
                    Type = InputTypes.RECEIVE_PHONE_NUMBER,
                    Value = ""
                },
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
                    PaddingTopY = (windowSize.WindowHeight-windowPadding.PaddingTop-windowPadding.PaddingBottom)*0.15 + (standardInputHeight + 20)*2,
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
                // Display SMSCode in 'xx xx xx' format
                return StringFormat.CovertToCodeFormat(GetSMSCodeInput.Value);
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

        public string PropPhoneNumberInput
        {
            get
            {
                // Display PhoneNumber in 'xxx-xxx-xxx' format
                return StringFormat.CovertToPhoneFormat(GetPhoneNumberInput.Value);
            }
            set
            {
                GetPhoneNumberInput.Value = value;
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.RECEIVE_PHONE_NUMBER,
                    Value = GetPhoneNumberInput.Value
                });
                NotifyOfPropertyChange(() => GetPhoneNumberInput);
                NotifyOfPropertyChange(() => PropPhoneNumberInput);
            }
        }

        public Input GetSMSCodeInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.RECEIVE_SMS_CODE);
            }
        }

        public Input GetPhoneNumberInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.RECEIVE_PHONE_NUMBER);
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
            NotifyOfPropertyChange(() => GetPhoneNumberInput);

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

            Input inputPhoneNumber = GetPhoneNumberInput;
            inputPhoneNumber.PaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.15 + inputSMSCode.Height + 20;
            NotifyOfPropertyChange(() => GetPhoneNumberInput);

            Button buttonMenu = GetMenuButton;
            buttonMenu.PaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.15 + inputSMSCode.Height + inputPhoneNumber.Height + 20 * 2;
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
                foreach (Input input in _inputs)
                {
                    CheckInputClick(input, relativeCursor);
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

            // Checking which input is focused
            if (_focusedInput == InputTypes.RECEIVE_SMS_CODE)
            {
                inputToChange = GetSMSCodeInput;
            }
            else if (_focusedInput == InputTypes.RECEIVE_PHONE_NUMBER)
            {
                inputToChange = GetPhoneNumberInput;
            }

            Console.WriteLine("Focused input type: "+ _focusedInput);
            // Check which key is clicked and manage appropriate action, if there's a focus on any input
            if(inputToChange != null)
            {
                if (message.KeyType != KeyTypes.DELETE)
                {
                    bool canAddKey = false;
                    // Check whether we can add key or not (validate the input)
                    if (inputToChange.Type == InputTypes.RECEIVE_SMS_CODE)
                    {
                        if (inputToChange.Value.Length < 6)
                        {
                            canAddKey = true;
                        }
                    }
                    else if (inputToChange.Type == InputTypes.RECEIVE_PHONE_NUMBER)
                    {
                        if (inputToChange.Value.Length < 9)
                        {
                            canAddKey = true;
                        }
                    }

                    // Add key if validation is appropraiate
                    if(canAddKey)
                    {
                        inputToChange.Value += message.keyToAdd;
                        hasValueChanged = true;
                    }
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

            // Update the UI if any input has changed
            if (hasValueChanged)
            {
                if(inputToChange.Type == InputTypes.RECEIVE_SMS_CODE)
                {
                    NotifyOfPropertyChange(() => GetSMSCodeInput);
                    NotifyOfPropertyChange(() => PropSMSCodeInput);
                    Console.WriteLine("Change of sms input: " + GetSMSCodeInput.Value);
                }
                else if (inputToChange.Type == InputTypes.RECEIVE_PHONE_NUMBER)
                {
                    NotifyOfPropertyChange(() => GetPhoneNumberInput);
                    NotifyOfPropertyChange(() => PropPhoneNumberInput);
                    Console.WriteLine("Change of phone input: " + GetPhoneNumberInput.Value);
                }
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

        private void CheckInputClick(Input input, Cursor relativeCursor)
        {
            bool isInputChange = false;
            KeyboardTypes keyboardTypeToSet = KeyboardTypes.NUMERIC;

            // Check if cursor is inside the input
            if (input.IsCursorInsideTheInput(relativeCursor))
            {
                // Check the type of input and set appropriate value to variable keyboardTypeToSet
                if(input.Type == InputTypes.RECEIVE_SMS_CODE)
                {
                    keyboardTypeToSet = KeyboardTypes.NUMERIC;
                }
                else if (input.Type == InputTypes.RECEIVE_PHONE_NUMBER)
                {
                    keyboardTypeToSet = KeyboardTypes.NUMERIC; // TO CHANGE - TO NUMERIC
                }
                
                // Check the previous and actual type of focused input
                if (_focusedInput != input.Type)
                {
                    isInputChange = true;
                    _focusedInput = input.Type;
                }
                else
                {
                    _focusedInput = InputTypes.NO_INPUT;
                }

                // Publishing message via event aggregator - the message body depends on the isInputChange flag
                if (isInputChange)
                {
                    _eventAggregator.PublishOnUIThread(new HandleKeyboardChange
                    {
                        Type = keyboardTypeToSet,
                        IsInputChange = true
                    });
                }
                else
                {
                    _eventAggregator.PublishOnUIThread(new HandleKeyboardChange
                    {
                        Type = keyboardTypeToSet,
                        IsToggle = true
                    });
                }
            }
        }
    }
}
