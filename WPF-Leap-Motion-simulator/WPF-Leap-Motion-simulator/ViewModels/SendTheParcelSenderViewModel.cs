using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

// Caliburn Micro
using Caliburn.Micro;

// Tracker
using WPF_Leap_Motion_simulator.LeapTracker;

// Models
using WPF_Leap_Motion_simulator.Models;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class SendTheParcelSenderViewModel: Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleWindowWidth>, IHandle<HandleWindowHeight>, IHandle<HandleKeyClick>, IHandle<HandleCrusorMove>
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
        private bool _isLiveValidation = false;
        private string _emailInputKeyboardType = "NONE";

        private readonly double titleLabelMarginTop = 12;
        private readonly double titleLabelHeight = 26;
        private readonly double standardLabelHeight = 15;
        private readonly double standardLabelMarginTop = 4;
        private readonly double standardInputWidth = 200;
        private readonly double standardInputHeight = 50;
        private readonly double standardInputMarginTop = 13;
        private readonly double standardInputMarginLeft = 20;
        private readonly double standardButtonWidth = 200;
        private readonly double standardButtonHeight = 100;
        private readonly double standardButtonMarginLeft = 20;

        public SendTheParcelSenderViewModel(IEventAggregator eventAggregator, TDOWindowSize windowSize, TDOWindowPadding windowPadding, TDOSendTheParcelSenderInputValues inputValues)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            mainWindowPadding = windowPadding;
            _gridColumnMultipliers = new double[] { 1.5, 5, 1.5 };
            _focusedInput = InputTypes.NO_INPUT;

            double titlePaddingLeftX = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardInputWidth) / 2;
            double basicPaddingLeftX = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardInputWidth*2 + standardInputMarginLeft)) / 2;
            double basicPaddingTopY = (windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom) * 0.04;

            _labels = new List<Label>
            {
                new Label
                {
                    Width = standardInputWidth,
                    Height = titleLabelHeight,
                    PaddingLeftX = titlePaddingLeftX,
                    PaddingTopY = basicPaddingTopY,
                    FontSize = 20,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SENDER_TITLE,
                    Value = "Dane nadawcy"
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SENDER_FIRST_NAME,
                    Value = "Imię"
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop*2 + standardInputHeight,
                    FontSize = 11,
                    FontWeight = "Bold",
                    TextColor = "#f22929",
                    Type = LabelTypes.SEND_SENDER_ERROR_FIRST_NAME,
                    Value = "Pole nie może być puste",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SENDER_LAST_NAME,
                    Value = "Nazwisko"
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop*2 + standardInputHeight,
                    FontSize = 11,
                    FontWeight = "Bold",
                    TextColor = "#f22929",
                    Type = LabelTypes.SEND_SENDER_ERROR_LAST_NAME,
                    Value = "Pole nie może być puste",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight + standardInputMarginTop + (standardLabelHeight + standardLabelMarginTop)*2,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SENDER_EMAIL,
                    Value = "Adres email"
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight*2 + standardInputMarginTop + standardLabelHeight*3 + standardLabelMarginTop*4,
                    FontSize = 11,
                    FontWeight = "Bold",
                    TextColor = "#f22929",
                    Type = LabelTypes.SEND_SENDER_ERROR_EMAIL,
                    Value = "Zły format adresu email",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight + standardInputMarginTop + (standardLabelHeight + standardLabelMarginTop)*2,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SENDER_PHONE_NUMBER,
                    Value = "Numer telefonu"
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight*2 + standardInputMarginTop + standardLabelHeight*3 + standardLabelMarginTop*4,
                    FontSize = 11,
                    FontWeight = "Bold",
                    TextColor = "#f22929",
                    Type = LabelTypes.SEND_SENDER_ERROR_PHONE_NUMBER,
                    Value = "Zły format numeru telefonu",
                    IsVisible = false
                }
            };

            _inputs = new List<Input>
            {
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop,
                    Type = InputTypes.SEND_SENDER_FIRST_NAME,
                    Value = inputValues.FirstName,
                    IsFocused = false
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop,
                    Type = InputTypes.SEND_SENDER_LAST_NAME,
                    Value = inputValues.LastName,
                    IsFocused = false
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*3 + standardInputMarginTop + standardInputHeight,
                    Type = InputTypes.SEND_SENDER_EMAIL,
                    Value = inputValues.Email,
                    IsFocused = false
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*3 + standardInputMarginTop + standardInputHeight,
                    Type = InputTypes.SEND_SENDER_PHONE_NUMBER,
                    Value = inputValues.PhoneNumber,
                    IsFocused = false
                }
            };

            double buttonRowSize = standardInputWidth * 2 + standardButtonMarginLeft;
            double basicButtonPaddingLeft = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - buttonRowSize) / 2;
            double buttonPaddingTop = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardInputHeight + standardInputMarginTop) * 2 + (standardLabelHeight + standardLabelMarginTop) * 4;

            _buttons = new List<Button>
            {
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = basicButtonPaddingLeft,
                    PaddingTopY = buttonPaddingTop,
                    Type = ButtonTypes.VIEW_SEND_THE_PARCEL_RECEIVER,
                    Title = "Dalej",
                    IsHovered = false
                },
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = basicButtonPaddingLeft + standardButtonWidth + standardButtonMarginLeft,
                    PaddingTopY = buttonPaddingTop,
                    Type = ButtonTypes.VIEW_MENU,
                    Title = "Wróć",
                    IsHovered = false
                },
                new Button
                {
                    Width = 50,
                    Height = 50,
                    PaddingLeftX = ((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - 50 - 20,
                    PaddingTopY = windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom - 320,
                    Type = ButtonTypes.ACTION_CHANGE_KEYBOARD_TYPE,
                    Title = "",
                    IsHovered = false,
                    IsVisible = false
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

        public Label GetTitleLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SENDER_TITLE);
            }
        }

        public Label GetFirstNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SENDER_FIRST_NAME);
            }
        }

        public Label GetErrorFirstNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SENDER_ERROR_FIRST_NAME);
            }
        }

        public Label GetLastNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SENDER_LAST_NAME);
            }
        }

        public Label GetErrorLastNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SENDER_ERROR_LAST_NAME);
            }
        }

        public Label GetEmailLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SENDER_EMAIL);
            }
        }

        public Label GetErrorEmailLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SENDER_ERROR_EMAIL);
            }
        }

        public Label GetPhoneNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SENDER_PHONE_NUMBER);
            }
        }

        public Label GetErrorPhoneNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SENDER_ERROR_PHONE_NUMBER);
            }
        }

        public Input GetFirstNameInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_SENDER_FIRST_NAME);
            }
        }

        public Input GetLastNameInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_SENDER_LAST_NAME);
            }
        }

        public Input GetEmailInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_SENDER_EMAIL);
            }
        }

        public Input GetPhoneNumberInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_SENDER_PHONE_NUMBER);
            }
        }

        public Button GetSendTheParcelReceiverButton
        {
            get
            {
                return _buttons.Find(button => button.Type == ButtonTypes.VIEW_SEND_THE_PARCEL_RECEIVER);
            }
        }

        public Button GetMenuButton
        {
            get
            {
                return _buttons.Find(button => button.Type == ButtonTypes.VIEW_MENU);
            }
        }

        public Button GetChangeKeyboardTypeButton
        {
            get
            {
                return _buttons.Find(button => button.Type == ButtonTypes.ACTION_CHANGE_KEYBOARD_TYPE);
            }
        }

        public string PropFirstNameInput
        {
            get
            {
                return GetFirstNameInput.Value;
            }

            set
            {
                GetFirstNameInput.Value = StringFormat.ConvertToNameFormat(value);
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_SENDER_FIRST_NAME,
                    Value = GetFirstNameInput.Value
                });
                NotifyOfPropertyChange(() => GetFirstNameInput);
                NotifyOfPropertyChange(() => PropFirstNameInput);

                // Live validate if validation is turned on
                if (_isLiveValidation)
                {
                    if (GetFirstNameInput.Value.Length > 0)
                    {
                        GetErrorFirstNameLabel.IsVisible = false;
                    }
                    else
                    {
                        GetErrorFirstNameLabel.IsVisible = true;
                    }
                    NotifyOfPropertyChange(() => GetErrorFirstNameLabel);
                }
            }
        }

        public string PropLastNameInput
        {
            get
            {
                return GetLastNameInput.Value;
            }

            set
            {
                GetLastNameInput.Value = StringFormat.ConvertToNameFormat(value);
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_SENDER_LAST_NAME,
                    Value = GetLastNameInput.Value
                });
                NotifyOfPropertyChange(() => GetLastNameInput);
                NotifyOfPropertyChange(() => PropLastNameInput);

                // Live validate if validation is turned on
                if (_isLiveValidation)
                {
                    if (GetLastNameInput.Value.Length > 0)
                    {
                        GetErrorLastNameLabel.IsVisible = false;
                    }
                    else
                    {
                        GetErrorLastNameLabel.IsVisible = true;
                    }
                    NotifyOfPropertyChange(() => GetErrorLastNameLabel);
                }
            }
        }

        public string PropEmailInput
        {
            get
            {
                return GetEmailInput.Value;
            }
            set
            {
                GetEmailInput.Value = value.ToLower();
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_SENDER_EMAIL,
                    Value = GetEmailInput.Value
                });
                NotifyOfPropertyChange(() => GetEmailInput);
                NotifyOfPropertyChange(() => PropEmailInput);

                // Live validate if validation is turned on
                if (_isLiveValidation)
                {
                    if (Validator.IsValidEmail(GetEmailInput.Value))
                    {
                        GetErrorEmailLabel.IsVisible = false;
                    }
                    else
                    {
                        GetErrorEmailLabel.IsVisible = true;
                    }
                    NotifyOfPropertyChange(() => GetErrorEmailLabel);
                }
            }
        }

        public string PropPhoneNumberInput
        {
            get
            {
                // Display PhoneNumber in 'xxx-xxx-xxx' format
                return StringFormat.ConvertToPhoneFormat(GetPhoneNumberInput.Value);
            }
            set
            {
                //Remove dashes from input (in case it's written from keyboard)
                string valToSet = value.Replace("-", "");
                GetPhoneNumberInput.Value = valToSet;
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_SENDER_PHONE_NUMBER,
                    Value = GetPhoneNumberInput.Value
                });
                NotifyOfPropertyChange(() => GetPhoneNumberInput);
                NotifyOfPropertyChange(() => PropPhoneNumberInput);

                // Live validate if validation is turned on
                if (_isLiveValidation)
                {
                    if (GetPhoneNumberInput.Value.Length == 9)
                    {
                        GetErrorPhoneNumberLabel.IsVisible = false;
                    }
                    else
                    {
                        GetErrorPhoneNumberLabel.IsVisible = true;
                    }
                    NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);
                }
            }
        }

        // -- Methods --
        public void LoadMenuView()
        {
            HideKeyboard();

            _eventAggregator.PublishOnUIThread(new HandleSendTheParcelButtonClick
            {
                Type = SendTheParcelButtonClickTypes.MENU
            });
        }

        public void LoadSendTheParcelReceiverView()
        {
            // Trun on live validation of the inputs
            _isLiveValidation = true;

            // Validate inputs
            bool canSubmit = true;
            // Validate First Name
            if (GetFirstNameInput.Value.Length == 0)
            {
                canSubmit = false;
                GetErrorFirstNameLabel.IsVisible = true;
                NotifyOfPropertyChange(() => GetErrorFirstNameLabel);
            }

            // Validate Last Name
            if (GetLastNameInput.Value.Length == 0)
            {
                canSubmit = false;
                GetErrorLastNameLabel.IsVisible = true;
                NotifyOfPropertyChange(() => GetErrorLastNameLabel);
            }

            // Validate Email
            if (!Validator.IsValidEmail(GetEmailInput.Value))
            {
                canSubmit = false;
                GetErrorEmailLabel.IsVisible = true;
                NotifyOfPropertyChange(() => GetErrorEmailLabel);
            }

            // Validate Phone Number
            if (GetPhoneNumberInput.Value.Length != 9)
            {
                canSubmit = false;
                GetErrorPhoneNumberLabel.IsVisible = true;
                NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);
            }

            // If validation is successful, hide the keyboard and change the view
            if (canSubmit)
            {
                HideKeyboard();

                _eventAggregator.PublishOnUIThread(new HandleSendTheParcelButtonClick
                {
                    Type = SendTheParcelButtonClickTypes.SEND_THE_PARCEL_RECEIVER
                });
            }
        }

        public void ToggleKeyboardType()
        {
            // Change the visible keyboard type
            if (_emailInputKeyboardType != "NONE")
            {
                KeyboardTypes keyboardType = KeyboardTypes.LETTER;
                if (_emailInputKeyboardType == "LETTER")
                {
                    _emailInputKeyboardType = "NUMERIC";
                    keyboardType = KeyboardTypes.NUMERIC;
                }
                else
                {
                    _emailInputKeyboardType = "LETTER";
                }

                _eventAggregator.PublishOnUIThread(new HandleKeyboardChange
                {
                    Type = keyboardType,
                    IsInputChange = true
                });
            }
        }

        // Handle window width change
        public void Handle(HandleWindowWidth message)
        {
            // labels
            GetTitleLabel.PaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardInputWidth) / 2;
            NotifyOfPropertyChange(() => GetTitleLabel);

            double basicPaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardInputWidth * 2 + standardInputMarginLeft)) / 2;
            GetFirstNameLabel.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetFirstNameLabel);

            GetErrorFirstNameLabel.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetErrorFirstNameLabel);

            GetLastNameLabel.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetLastNameLabel);

            GetErrorLastNameLabel.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetErrorLastNameLabel);

            GetEmailLabel.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetEmailLabel);

            GetErrorEmailLabel.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetErrorEmailLabel);

            GetPhoneNumberLabel.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetPhoneNumberLabel);

            GetErrorPhoneNumberLabel.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);

            // inputs
            GetFirstNameInput.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetFirstNameInput);

            GetLastNameInput.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetLastNameInput);

            GetEmailInput.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetEmailInput);

            GetPhoneNumberInput.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetPhoneNumberInput);

            // buttons
            double buttonRowSize = standardButtonMarginLeft + GetSendTheParcelReceiverButton.Width + GetMenuButton.Width;
            double basicButtonPaddingLeft = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - buttonRowSize) / 2;

            GetSendTheParcelReceiverButton.PaddingLeftX = basicButtonPaddingLeft;
            NotifyOfPropertyChange(() => GetSendTheParcelReceiverButton);

            GetMenuButton.PaddingLeftX = basicButtonPaddingLeft + GetSendTheParcelReceiverButton.Width + standardButtonMarginLeft;
            NotifyOfPropertyChange(() => GetMenuButton);

            GetChangeKeyboardTypeButton.PaddingLeftX = ((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - 50 - 20;
            NotifyOfPropertyChange(() => GetChangeKeyboardTypeButton);
        }

        // Handle window height change
        public void Handle(HandleWindowHeight message)
        {
            double basicPaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.04;

            // labels
            GetTitleLabel.PaddingTopY = basicPaddingTopY;
            NotifyOfPropertyChange(() => GetTitleLabel);

            GetFirstNameLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop;
            NotifyOfPropertyChange(() => GetFirstNameLabel);

            GetErrorFirstNameLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop * 2 + standardInputHeight;
            NotifyOfPropertyChange(() => GetErrorFirstNameLabel);

            GetLastNameLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop;
            NotifyOfPropertyChange(() => GetLastNameLabel);

            GetErrorLastNameLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop * 2 + standardInputHeight;
            NotifyOfPropertyChange(() => GetErrorLastNameLabel);

            GetEmailLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight + standardInputMarginTop + (standardLabelHeight + standardLabelMarginTop) * 2;
            NotifyOfPropertyChange(() => GetEmailLabel);

            GetErrorEmailLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight * 2 + standardInputMarginTop + standardLabelHeight * 3 + standardLabelMarginTop * 4;
            NotifyOfPropertyChange(() => GetErrorEmailLabel);

            GetPhoneNumberLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight + standardInputMarginTop + (standardLabelHeight + standardLabelMarginTop) * 2;
            NotifyOfPropertyChange(() => GetPhoneNumberLabel);

            GetErrorPhoneNumberLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight * 2 + standardInputMarginTop + standardLabelHeight * 3 + standardLabelMarginTop * 4;
            NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);

            // inputs
            GetFirstNameInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetFirstNameInput);

            GetLastNameInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetLastNameInput);

            GetEmailInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 3 + standardInputMarginTop + standardInputHeight;
            NotifyOfPropertyChange(() => GetEmailInput);

            GetPhoneNumberInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 3 + standardInputMarginTop + standardInputHeight;
            NotifyOfPropertyChange(() => GetPhoneNumberInput);

            // buttons
            double buttonPaddingTop = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardInputHeight + standardInputMarginTop) * 2 + (standardLabelHeight + standardLabelMarginTop) * 4;
            GetSendTheParcelReceiverButton.PaddingTopY = buttonPaddingTop;
            NotifyOfPropertyChange(() => GetSendTheParcelReceiverButton);

            GetMenuButton.PaddingTopY = buttonPaddingTop;
            NotifyOfPropertyChange(() => GetMenuButton);

            GetChangeKeyboardTypeButton.PaddingTopY = message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom - 320;
            NotifyOfPropertyChange(() => GetChangeKeyboardTypeButton);
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
                if (GetMenuButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadMenuView();
                }
                else if (GetSendTheParcelReceiverButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadSendTheParcelReceiverView();
                }
                else if (GetChangeKeyboardTypeButton.IsVisible && GetChangeKeyboardTypeButton.IsCursorInsideTheButton(relativeCursor))
                {
                    ToggleKeyboardType();
                }
            }
        }

        // Handle keyboard key click
        public void Handle(HandleKeyClick message)
        {
            Input inputToChange = null;
            bool hasValueToBeChanged = false;
            string newValue = "";

            // Checking which input is focused
            if (_focusedInput == InputTypes.SEND_SENDER_FIRST_NAME)
            {
                inputToChange = GetFirstNameInput;
            }
            else if (_focusedInput == InputTypes.SEND_SENDER_LAST_NAME)
            {
                inputToChange = GetLastNameInput;
            }
            else if (_focusedInput == InputTypes.SEND_SENDER_EMAIL)
            {
                inputToChange = GetEmailInput;
            }
            else if (_focusedInput == InputTypes.SEND_SENDER_PHONE_NUMBER)
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
                    if (inputToChange.Type == InputTypes.SEND_SENDER_PHONE_NUMBER)
                    {
                        if (inputToChange.Value.Length < 9)
                        {
                            canAddKey = true;
                        }
                    }
                    else
                    {
                        canAddKey = true;
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
                if (inputToChange.Type == InputTypes.SEND_SENDER_FIRST_NAME)
                {
                    PropFirstNameInput = newValue;
                    NotifyOfPropertyChange(() => PropFirstNameInput);
                    Console.WriteLine("Change of first name input: " + GetFirstNameInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_SENDER_LAST_NAME)
                {
                    PropLastNameInput = newValue;
                    NotifyOfPropertyChange(() => PropLastNameInput);
                    Console.WriteLine("Change of last name input: " + GetLastNameInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_SENDER_EMAIL)
                {
                    PropEmailInput = newValue;
                    NotifyOfPropertyChange(() => PropEmailInput);
                    Console.WriteLine("Change of email input: " + GetEmailInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_SENDER_PHONE_NUMBER)
                {
                    PropPhoneNumberInput = newValue;
                    NotifyOfPropertyChange(() => PropPhoneNumberInput);
                    Console.WriteLine("Change of phone input: " + GetPhoneNumberInput.Value);
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

            NotifyOfPropertyChange(() => GetSendTheParcelReceiverButton);
            NotifyOfPropertyChange(() => GetMenuButton);
            NotifyOfPropertyChange(() => GetChangeKeyboardTypeButton);
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
            string prevEmailInputKeyboardType = "NONE";

            // Check if cursor is inside the input
            if (input.IsCursorInsideTheInput(relativeCursor))
            {
                // Toggle focus of the input
                input.IsFocused = !input.IsFocused;

                // Check the type of input and set appropriate value to variable keyboardTypeToSet
                if (input.Type == InputTypes.SEND_SENDER_FIRST_NAME)
                {
                    keyboardTypeToSet = KeyboardTypes.LETTER;
                    NotifyOfPropertyChange(() => GetFirstNameInput);
                }
                else if (input.Type == InputTypes.SEND_SENDER_LAST_NAME)
                {
                    keyboardTypeToSet = KeyboardTypes.LETTER;
                    NotifyOfPropertyChange(() => GetLastNameInput);
                }
                else if (input.Type == InputTypes.SEND_SENDER_EMAIL)
                {
                    keyboardTypeToSet = KeyboardTypes.LETTER;
                    NotifyOfPropertyChange(() => GetEmailInput);
                }
                else if (input.Type == InputTypes.SEND_SENDER_PHONE_NUMBER)
                {
                    keyboardTypeToSet = KeyboardTypes.NUMERIC;
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
                        if (_focusedInput == InputTypes.SEND_SENDER_FIRST_NAME)
                        {
                            NotifyOfPropertyChange(() => GetFirstNameInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_SENDER_LAST_NAME)
                        {
                            NotifyOfPropertyChange(() => GetLastNameInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_SENDER_EMAIL)
                        {
                            NotifyOfPropertyChange(() => GetEmailInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_SENDER_PHONE_NUMBER)
                        {
                            NotifyOfPropertyChange(() => GetPhoneNumberInput);
                        }
                    }

                    // Set flag to change the input and set new focused input
                    isInputChange = true;
                    _focusedInput = input.Type;

                    if (_focusedInput == InputTypes.SEND_SENDER_EMAIL)
                    {
                        // Show button for changing the keyboard type
                        if (!GetChangeKeyboardTypeButton.IsVisible)
                        {
                            GetChangeKeyboardTypeButton.IsVisible = true;
                            NotifyOfPropertyChange(() => GetChangeKeyboardTypeButton);
                        }
                        _emailInputKeyboardType = "LETTER";
                    }
                    else
                    {
                        // Hide button for changing the keyboard type
                        GetChangeKeyboardTypeButton.IsVisible = false;
                        NotifyOfPropertyChange(() => GetChangeKeyboardTypeButton);
                        _emailInputKeyboardType = "NONE";
                    }
                }
                else
                {
                    _focusedInput = InputTypes.NO_INPUT;

                    // Hide button for changing the keyboard type
                    GetChangeKeyboardTypeButton.IsVisible = false;
                    NotifyOfPropertyChange(() => GetChangeKeyboardTypeButton);
                    prevEmailInputKeyboardType = _emailInputKeyboardType;
                    _emailInputKeyboardType = "NONE";
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
                    if (prevEmailInputKeyboardType != "NONE")
                    {
                        if (prevEmailInputKeyboardType == "LETTER")
                        {
                            keyboardTypeToSet = KeyboardTypes.LETTER;
                        }
                        else
                        {
                            keyboardTypeToSet = KeyboardTypes.NUMERIC;
                        }
                    }

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
