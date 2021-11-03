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
    class ReceiveTheParcelViewModel: Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleWindowWidth>, IHandle<HandleWindowHeight>, IHandle<HandleKeyClick>, IHandle<HandleCrusorMove>
    {
        private IEventAggregator _eventAggregator;

        // -- Variables of the main window --
        private readonly TDOWindowPadding mainWindowPadding;

        //-- Variables of this window --
        private double[] _gridColumnMultipliers;
        private List<Label> _labels;
        private List<Button> _buttons;
        private List<Input> _inputs;
        private InputTypes _focusedInput;

        private readonly double standardLabelHeight = 14;
        private readonly double standardLabelMarginTop = 5;
        private readonly double standardInputWidth = 200;
        private readonly double standardInputHeight = 50;
        private readonly double standardInputMarginTop = 20;
        private readonly double standardButtonWidth = 200;
        private readonly double standardButtonHeight = 100;
        private readonly double standardButtonMarginLeft = 20;

        public ReceiveTheParcelViewModel(IEventAggregator eventAggregator, TDOWindowSize windowSize, TDOWindowPadding windowPadding)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            mainWindowPadding = windowPadding;
            _gridColumnMultipliers = new double[] { 1.5, 5, 1.5 };
            _focusedInput = InputTypes.NO_INPUT;

            double paddingLeftX = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardInputWidth) / 2;
            double basicPaddingTopY = (windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom) * 0.08;

            _labels = new List<Label>
            {
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = paddingLeftX,
                    PaddingTopY = basicPaddingTopY,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.RECEIVE_SMS_CODE,
                    Value = "Kod SMS"
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = paddingLeftX,
                    PaddingTopY = basicPaddingTopY + standardInputHeight + standardInputMarginTop + standardLabelHeight + standardLabelMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.RECEIVE_PHONE_NUMBER,
                    Value = "Numer telefonu"
                }
            };

            _inputs = new List<Input>
            {
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = paddingLeftX,
                    PaddingTopY = basicPaddingTopY + standardLabelHeight + standardLabelMarginTop,
                    Type = InputTypes.RECEIVE_SMS_CODE,
                    Value = "",
                    IsFocused = false
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = paddingLeftX,
                    PaddingTopY = basicPaddingTopY + standardInputHeight + standardInputMarginTop + (standardLabelHeight + standardLabelMarginTop)*2,
                    Type = InputTypes.RECEIVE_PHONE_NUMBER,
                    Value = "",
                    IsFocused = false
                },
            };

            double buttonRowSize = standardInputWidth * 2 + standardButtonMarginLeft;
            double basicButtonPaddingLeft = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - buttonRowSize) / 2;

            _buttons = new List<Button>
            {
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = basicButtonPaddingLeft,
                    PaddingTopY = basicPaddingTopY + (standardInputHeight + standardInputMarginTop + standardLabelHeight + standardLabelMarginTop)*2,
                    Type = ButtonTypes.VIEW_SUCCESS_RECEIVE,
                    Title = "Wyślij",
                    IsHovered = false
                },
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = basicButtonPaddingLeft + standardButtonWidth + standardButtonMarginLeft,
                    PaddingTopY = basicPaddingTopY + (standardInputHeight + standardInputMarginTop + standardLabelHeight + standardLabelMarginTop)*2,
                    Type = ButtonTypes.VIEW_MENU,
                    Title = "Wróć",
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
                //Remove spaces from input (in case it's written from keyboard)
                string valToSet = value.Replace(" ", "");
                GetSMSCodeInput.Value = valToSet;
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
                //Remove dashes from input (in case it's written from keyboard)
                string valToSet = value.Replace("-", "");
                GetPhoneNumberInput.Value = valToSet;
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.RECEIVE_PHONE_NUMBER,
                    Value = GetPhoneNumberInput.Value
                });
                NotifyOfPropertyChange(() => GetPhoneNumberInput);
                NotifyOfPropertyChange(() => PropPhoneNumberInput);
            }
        }

        public Label GetSMSCodeLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.RECEIVE_SMS_CODE);
            }
        }

        public Label GetPhoneNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.RECEIVE_PHONE_NUMBER);
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

        public Button GetSuccessReceiveButton
        {
            get
            {
                return _buttons.Find(button => button.Type == ButtonTypes.VIEW_SUCCESS_RECEIVE);
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
                Type = ReceiveTheParcelButtonClickTypes.MENU
            });
        }

        public void LoadSuccessReceiveView()
        {
            HideKeyboard();

            _eventAggregator.PublishOnUIThread(new HandleReceiveTheParcelButtonClick
            {
                Type = ReceiveTheParcelButtonClickTypes.SUCCESS_RECEIVE
            });
        }

        // Handle window width change
        public void Handle(HandleWindowWidth message)
        {
            foreach(Label label in _labels)
            {
                label.PaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - label.Width) / 2;
            }
            NotifyOfPropertyChange(() => GetSMSCodeLabel);
            NotifyOfPropertyChange(() => GetPhoneNumberLabel);

            foreach (Input input in _inputs)
            {
                input.PaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - input.Width) / 2;
            }
            NotifyOfPropertyChange(() => GetSMSCodeInput);
            NotifyOfPropertyChange(() => GetPhoneNumberInput);

            double buttonRowSize = standardButtonMarginLeft;
            foreach (Button button in _buttons)
            {
                buttonRowSize += button.Width;
            }
            double basicButtonPaddingLeft = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - buttonRowSize) / 2;

            Button SuccessReceiveButton = GetSuccessReceiveButton;
            SuccessReceiveButton.PaddingLeftX = basicButtonPaddingLeft;

            Button MenuButton = GetMenuButton;
            MenuButton.PaddingLeftX = basicButtonPaddingLeft + SuccessReceiveButton.Width + standardButtonMarginLeft;
            NotifyOfPropertyChange(() => GetSuccessReceiveButton);
            NotifyOfPropertyChange(() => GetMenuButton);
        }

        // Handle window height change
        public void Handle(HandleWindowHeight message)
        {
            double basicPaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.08;

            Label labelSMSCode = GetSMSCodeLabel;
            labelSMSCode.PaddingTopY = basicPaddingTopY;
            NotifyOfPropertyChange(() => GetSMSCodeLabel);

            Label labelPhoneNumber = GetPhoneNumberLabel;
            labelPhoneNumber.PaddingTopY = basicPaddingTopY + standardInputHeight + standardInputMarginTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetPhoneNumberLabel);

            Input inputSMSCode = GetSMSCodeInput;
            inputSMSCode.PaddingTopY = basicPaddingTopY + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetSMSCodeInput);

            Input inputPhoneNumber = GetPhoneNumberInput;
            inputPhoneNumber.PaddingTopY = basicPaddingTopY + standardInputHeight + standardInputMarginTop + (standardLabelHeight + standardLabelMarginTop) * 2;
            NotifyOfPropertyChange(() => GetPhoneNumberInput);

            Button buttonMenu = GetMenuButton;
            buttonMenu.PaddingTopY = basicPaddingTopY + (standardInputHeight + standardInputMarginTop + standardLabelHeight + standardLabelMarginTop) * 2;
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
                Button buttonSuccessReceive = GetSuccessReceiveButton;
                if (buttonMenu.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadMenuView();
                }
                else if (buttonSuccessReceive.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadSuccessReceiveView();
                }

                // TO DO - check other buttons and inputs in this view

            }
        }

        // Handle keyboard key click
        public void Handle(HandleKeyClick message)
        {
            Input inputToChange = null;
            bool hasValueToBeChanged = false;
            string newValue = "";

            // Checking which input is focused
            if (_focusedInput == InputTypes.RECEIVE_SMS_CODE)
            {
                inputToChange = GetSMSCodeInput;
            }
            else if (_focusedInput == InputTypes.RECEIVE_PHONE_NUMBER)
            {
                inputToChange = GetPhoneNumberInput;
            }

            Console.WriteLine("Focused input type: " + _focusedInput);
            // Check which key is clicked and manage appropriate action, if there's a focus on any input
            if (inputToChange != null)
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
                    if (canAddKey)
                    {
                        // We don't change here inputToChange, because we want also to update value of the input in tje ShellViewModel
                        // So we change the appropriate property of the input (wchich also changes the input value)
                        //OLD - inputToChange.Value += message.keyToAdd;

                        newValue = inputToChange.Value + message.keyToAdd;
                        hasValueToBeChanged = true;
                    }
                }
                else
                {
                    if (inputToChange.Value.Length > 0)
                    {
                        //OLD - inputToChange.Value = inputToChange.Value.Substring(0, inputToChange.Value.Length - 1);

                        newValue = inputToChange.Value.Substring(0, inputToChange.Value.Length - 1);
                        hasValueToBeChanged = true;
                    }
                }
            }

            // Update the UI if any input has changed
            if (hasValueToBeChanged)
            {
                // Change the values and notify the proper properties
                if (inputToChange.Type == InputTypes.RECEIVE_SMS_CODE)
                {
                    PropSMSCodeInput = newValue;
                    NotifyOfPropertyChange(() => PropSMSCodeInput);
                    Console.WriteLine("Change of sms input: " + GetSMSCodeInput.Value);
                }
                else if (inputToChange.Type == InputTypes.RECEIVE_PHONE_NUMBER)
                {
                    PropPhoneNumberInput = newValue;
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
                // Toggle focus of the input
                input.IsFocused = !input.IsFocused;

                // Check the type of input and set appropriate value to variable keyboardTypeToSet
                if(input.Type == InputTypes.RECEIVE_SMS_CODE)
                {
                    keyboardTypeToSet = KeyboardTypes.NUMERIC;
                    NotifyOfPropertyChange(() => GetSMSCodeInput);
                }
                else if (input.Type == InputTypes.RECEIVE_PHONE_NUMBER)
                {
                    keyboardTypeToSet = KeyboardTypes.NUMERIC; // TO CHANGE - TO NUMERIC
                    NotifyOfPropertyChange(() => GetPhoneNumberInput);
                }
                
                // Check the previous and actual type of focused input
                if (_focusedInput != input.Type)
                {
                    //Toggle the provious focused input
                    if (_focusedInput != InputTypes.NO_INPUT)
                    {
                        Input prevFocusedInput = _inputs.Find(fInput => fInput.Type == _focusedInput);
                        prevFocusedInput.IsFocused = !prevFocusedInput.IsFocused;
                        if (_focusedInput == InputTypes.RECEIVE_SMS_CODE)
                        {
                            NotifyOfPropertyChange(() => GetSMSCodeInput);
                        }
                        else if (_focusedInput == InputTypes.RECEIVE_PHONE_NUMBER)
                        {
                            NotifyOfPropertyChange(() => GetPhoneNumberInput);
                        }
                    }

                    // Set flag to change the input and set new focused input
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

            NotifyOfPropertyChange(() => GetSuccessReceiveButton);
            NotifyOfPropertyChange(() => GetMenuButton);
        }
    }
}
