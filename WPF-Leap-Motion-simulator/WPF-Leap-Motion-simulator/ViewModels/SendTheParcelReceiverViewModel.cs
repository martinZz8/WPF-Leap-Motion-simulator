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
    class SendTheParcelReceiverViewModel : Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleWindowWidth>, IHandle<HandleWindowHeight>, IHandle<HandleKeyClick>, IHandle<HandleCrusorMove>
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
        private bool _isFirstView = true;
        private List<InputTypes> _listOfFirstPageErrorInputsTypes = new List<InputTypes>();
        private List<InputTypes> _listOfSecondPageErrorInputsTypes = new List<InputTypes>();

        private readonly double titleLabelMarginTop = 12;
        private readonly double titleLabelHeight = 23;
        private readonly double standardLabelHeight = 15;
        private readonly double standardLabelMarginTop = 4;
        private readonly double standardInputWidth = 200;
        private readonly double standardInputHeight = 50;
        private readonly double standardInputMarginTop = 13;
        private readonly double standardInputMarginLeft = 20;
        private readonly double standardButtonWidth = 200;
        private readonly double standardButtonHeight = 100;
        private readonly double standardButtonMarginLeft = 20;

        public SendTheParcelReceiverViewModel(IEventAggregator eventAggregator, TDOWindowSize windowSize, TDOWindowPadding windowPadding, TDOSendTheParcelReceiverInputValues inputValues)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            mainWindowPadding = windowPadding;
            _gridColumnMultipliers = new double[] { 1.5, 5, 1.5 };
            _focusedInput = InputTypes.NO_INPUT;

            double titlePaddingLeftX = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardInputWidth) / 2;
            double basicPaddingLeftX = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardInputWidth * 2 + standardInputMarginLeft)) / 2;
            double smallBasicPaddingLeftX = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardInputWidth * 3 + standardInputMarginLeft*2)) / 2;
            double basicPaddingTopY = (windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom) * 0.04;

            double inputPagePaddingTopY = (windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom) * 0.04 + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop + standardInputHeight + standardInputMarginTop / 2;

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
                    Type = LabelTypes.SEND_RECEIVER_TITLE,
                    Value = "Dane odbiorcy"
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
                    Type = LabelTypes.SEND_RECEIVER_FIRST_NAME,
                    Value = "Imię",
                    IsVisible = true
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
                    Type = LabelTypes.SEND_RECEIVER_ERROR_FIRST_NAME,
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
                    Type = LabelTypes.SEND_RECEIVER_LAST_NAME,
                    Value = "Nazwisko",
                    IsVisible = true
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
                    Type = LabelTypes.SEND_RECEIVER_ERROR_LAST_NAME,
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
                    Type = LabelTypes.SEND_RECEIVER_PHONE_NUMBER,
                    Value = "Numer telefonu",
                    IsVisible = true
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
                    Type = LabelTypes.SEND_RECEIVER_ERROR_PHONE_NUMBER,
                    Value = "Zły format numeru telefonu",
                    IsVisible = false
                },// second view labels
                new Label
                {
                    Width = standardInputWidth,
                    Height = titleLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_RECEIVER_CITY,
                    Value = "Miasto",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop*2 + standardInputHeight,
                    FontSize = 11,
                    FontWeight = "Bold",
                    TextColor = "#f22929",
                    Type = LabelTypes.SEND_RECEIVER_ERROR_CITY,
                    Value = "Pole nie może być puste",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = titleLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_RECEIVER_POST_CODE,
                    Value = "Kod pocztowy",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop*2 + standardInputHeight,
                    FontSize = 11,
                    FontWeight = "Bold",
                    TextColor = "#f22929",
                    Type = LabelTypes.SEND_RECEIVER_ERROR_POST_CODE,
                    Value = "Zły format kodu pocztowego",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = titleLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_RECEIVER_STREET,
                    Value = "Ulica",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop*2 + standardInputHeight,
                    FontSize = 11,
                    FontWeight = "Bold",
                    TextColor = "#f22929",
                    Type = LabelTypes.SEND_RECEIVER_ERROR_STREET,
                    Value = "Pole nie może być puste",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = titleLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*2 + standardInputHeight + standardInputMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_RECEIVER_HOUSE_NUMBER,
                    Value = "Numer domu",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight*3 + standardLabelMarginTop*4 + standardInputHeight*2 + standardInputMarginTop,
                    FontSize = 11,
                    FontWeight = "Bold",
                    TextColor = "#f22929",
                    Type = LabelTypes.SEND_RECEIVER_ERROR_HOUSE_NUMBER,
                    Value = "Pole nie może być puste",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = titleLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*2 + standardInputHeight + standardInputMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_RECEIVER_APARTMENT_NUMBER,
                    Value = "Numer mieszkania",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = titleLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*2 + standardInputHeight + standardInputMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_RECEIVER_HOUSE_LETTER,
                    Value = "Litera domu",
                    IsVisible = false
                },
                new Label
                {
                    Width = standardInputWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallBasicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*2 + standardInputHeight + standardInputMarginTop,
                    FontSize = 11,
                    FontWeight = "Bold",
                    TextColor = "#f22929",
                    Type = LabelTypes.SEND_RECEIVER_ERROR_HOUSE_LETTER,
                    Value = "Niepoprawa litera domu",
                    IsVisible = false
                },
            };

            _inputs = new List<Input>
            {
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop,
                    Type = InputTypes.SEND_RECEIVER_FIRST_NAME,
                    Value = inputValues.FirstName,
                    IsFocused = false,
                    IsVisible = true
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop,
                    Type = InputTypes.SEND_RECEIVER_LAST_NAME,
                    Value = inputValues.LastName,
                    IsFocused = false,
                    IsVisible = true
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*3 + standardInputMarginTop + standardInputHeight,
                    Type = InputTypes.SEND_RECEIVER_PHONE_NUMBER,
                    Value = inputValues.PhoneNumber,
                    IsFocused = false,
                    IsVisible = true
                },// second view inputs
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop,
                    Type = InputTypes.SEND_RECEIVER_CITY,
                    Value = inputValues.City,
                    IsFocused = false,
                    IsVisible = false
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop,
                    Type = InputTypes.SEND_RECEIVER_POST_CODE,
                    Value = inputValues.PostCode,
                    IsFocused = false,
                    IsVisible = false
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop,
                    Type = InputTypes.SEND_RECEIVER_STREET,
                    Value = inputValues.Street,
                    IsFocused = false,
                    IsVisible = false
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*3 + standardInputHeight + standardInputMarginTop,
                    Type = InputTypes.SEND_RECEIVER_HOUSE_NUMBER,
                    Value = inputValues.HouseNumber,
                    IsFocused = false,
                    IsVisible = false
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*3 + standardInputHeight + standardInputMarginTop,
                    Type = InputTypes.SEND_RECEIVER_APARTMENT_NUMBER,
                    Value = inputValues.ApartmentNumber,
                    IsFocused = false,
                    IsVisible = false
                },
                new Input
                {
                    Width = standardInputWidth,
                    Height = standardInputHeight,
                    PaddingLeftX = basicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2,
                    PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop)*3 + standardInputHeight + standardInputMarginTop,
                    Type = InputTypes.SEND_RECEIVER_HOUSE_LETTER,
                    Value = inputValues.HouseLetter,
                    IsFocused = false,
                    IsVisible = false
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
                    Type = ButtonTypes.VIEW_SEND_THE_PARCEL_SUMMARY,
                    Title = "Dalej",
                    IsHovered = false
                },
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = basicButtonPaddingLeft + standardButtonWidth + standardButtonMarginLeft,
                    PaddingTopY = buttonPaddingTop,
                    Type = ButtonTypes.VIEW_SEND_THE_PARCEL_SENDER,
                    Title = "Wróć",
                    IsHovered = false
                },
                new Button
                {
                    Width = 40,
                    Height = 40,
                    PaddingLeftX = 20,
                    PaddingTopY = inputPagePaddingTopY,
                    Type = ButtonTypes.ACTION_CHANGE_BACKWARD_INPUT_PAGE,
                    Title = "<",
                    IsHovered = false,
                    IsVisible = false,
                    IsErrorStyle = false
                },
                new Button
                {
                    Width = 40,
                    Height = 40,
                    PaddingLeftX = ((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - 60,
                    PaddingTopY = inputPagePaddingTopY,
                    Type = ButtonTypes.ACTION_CHANGE_FORWARD_INPUT_PAGE,
                    Title = ">",
                    IsHovered = false,
                    IsVisible = true,
                    IsErrorStyle = false
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
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_TITLE);
            }
        }

        public Label GetFirstNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_FIRST_NAME);
            }
        }

        public Label GetErrorFirstNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_ERROR_FIRST_NAME);
            }
        }

        public Label GetLastNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_LAST_NAME);
            }
        }

        public Label GetErrorLastNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_ERROR_LAST_NAME);
            }
        }

        public Label GetPhoneNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_PHONE_NUMBER);
            }
        }

        public Label GetErrorPhoneNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_ERROR_PHONE_NUMBER);
            }
        }

        public Label GetCityLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_CITY);
            }
        }

        public Label GetErrorCityLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_ERROR_CITY);
            }
        }

        public Label GetPostCodeLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_POST_CODE);
            }
        }

        public Label GetErrorPostCodeLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_ERROR_POST_CODE);
            }
        }

        public Label GetStreetLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_STREET);
            }
        }

        public Label GetErrorStreetLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_ERROR_STREET);
            }
        }

        public Label GetHouseNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_HOUSE_NUMBER);
            }
        }

        public Label GetErrorHouseNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_ERROR_HOUSE_NUMBER);
            }
        }

        public Label GetApartmentNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_APARTMENT_NUMBER);
            }
        }

        public Label GetHouseLetterLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_HOUSE_LETTER);
            }
        }

        public Label GetErrorHouseLetterLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_RECEIVER_ERROR_HOUSE_LETTER);
            }
        }

        public Input GetFirstNameInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_RECEIVER_FIRST_NAME);
            }
        }

        public Input GetLastNameInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_RECEIVER_LAST_NAME);
            }
        }

        public Input GetPhoneNumberInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_RECEIVER_PHONE_NUMBER);
            }
        }

        public Input GetCityInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_RECEIVER_CITY);
            }
        }

        public Input GetPostCodeInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_RECEIVER_POST_CODE);
            }
        }

        public Input GetStreetInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_RECEIVER_STREET);
            }
        }

        public Input GetHouseNumberInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_RECEIVER_HOUSE_NUMBER);
            }
        }

        public Input GetApartmentNumberInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_RECEIVER_APARTMENT_NUMBER);
            }
        }

        public Input GetHouseLetterInput
        {
            get
            {
                return _inputs.Find(input => input.Type == InputTypes.SEND_RECEIVER_HOUSE_LETTER);
            }
        }

        public Button GetSendTheParcelSummaryButton
        {
            get
            {
                return _buttons.Find(button => button.Type == ButtonTypes.VIEW_SEND_THE_PARCEL_SUMMARY);
            }
        }

        public Button GetSendTheParcelSenderButton
        {
            get
            {
                return _buttons.Find(button => button.Type == ButtonTypes.VIEW_SEND_THE_PARCEL_SENDER);
            }
        }

        public Button GetChangeBackwardInputPageButton
        {
            get
            {
                return _buttons.Find(button => button.Type == ButtonTypes.ACTION_CHANGE_BACKWARD_INPUT_PAGE);
            }
        }

        public Button GetChangeForwardInputPageButton
        {
            get
            {
                return _buttons.Find(button => button.Type == ButtonTypes.ACTION_CHANGE_FORWARD_INPUT_PAGE);
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
                    Type = InputTypes.SEND_RECEIVER_FIRST_NAME,
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
                        NotifyOfPropertyChange(() => GetErrorFirstNameLabel);
                        RemoveUniqueInputTypeToList(_listOfFirstPageErrorInputsTypes, GetFirstNameInput.Type);
                    }
                    else
                    {
                        if (_isFirstView)
                        {
                            GetErrorFirstNameLabel.IsVisible = true;
                            NotifyOfPropertyChange(() => GetErrorFirstNameLabel);
                        }
                        AddUniqueInputTypeToList(_listOfFirstPageErrorInputsTypes, GetFirstNameInput.Type);
                    }
                    ChangeErrorStyleOfButton(GetChangeBackwardInputPageButton, _listOfFirstPageErrorInputsTypes, () => NotifyOfPropertyChange(() => GetChangeBackwardInputPageButton));
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
                    Type = InputTypes.SEND_RECEIVER_LAST_NAME,
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
                        NotifyOfPropertyChange(() => GetErrorLastNameLabel);
                        RemoveUniqueInputTypeToList(_listOfFirstPageErrorInputsTypes, GetLastNameInput.Type);
                    }
                    else
                    {
                        if (_isFirstView)
                        {
                            GetErrorLastNameLabel.IsVisible = true;
                            NotifyOfPropertyChange(() => GetErrorLastNameLabel);
                        }
                        AddUniqueInputTypeToList(_listOfFirstPageErrorInputsTypes, GetLastNameInput.Type);
                    }
                    ChangeErrorStyleOfButton(GetChangeBackwardInputPageButton, _listOfFirstPageErrorInputsTypes, () => NotifyOfPropertyChange(() => GetChangeBackwardInputPageButton));
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
                    Type = InputTypes.SEND_RECEIVER_PHONE_NUMBER,
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
                        NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);
                        RemoveUniqueInputTypeToList(_listOfFirstPageErrorInputsTypes, GetPhoneNumberInput.Type);
                    }
                    else
                    {
                        if (_isFirstView)
                        {
                            GetErrorPhoneNumberLabel.IsVisible = true;
                            NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);
                        }
                        AddUniqueInputTypeToList(_listOfFirstPageErrorInputsTypes, GetPhoneNumberInput.Type);
                    }
                    ChangeErrorStyleOfButton(GetChangeBackwardInputPageButton, _listOfFirstPageErrorInputsTypes, () => NotifyOfPropertyChange(() => GetChangeBackwardInputPageButton));
                }
            }
        }
        // props for the second view
        public string PropCityInput
        {
            get
            {
                return GetCityInput.Value;
            }
            set
            {
                GetCityInput.Value = StringFormat.ConvertToNameFormat(value);
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_RECEIVER_CITY,
                    Value = GetCityInput.Value
                });
                NotifyOfPropertyChange(() => GetCityInput);
                NotifyOfPropertyChange(() => PropCityInput);

                // Live validate if validation is turned on
                if (_isLiveValidation)
                {
                    if (GetCityInput.Value.Length > 0)
                    {
                        GetErrorCityLabel.IsVisible = false;
                        NotifyOfPropertyChange(() => GetErrorCityLabel);
                        RemoveUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetCityInput.Type);
                    }
                    else
                    {
                        if (!_isFirstView)
                        {
                            GetErrorCityLabel.IsVisible = true;
                            NotifyOfPropertyChange(() => GetErrorCityLabel);
                        }
                        AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetCityInput.Type);
                    }
                    ChangeErrorStyleOfButton(GetChangeForwardInputPageButton, _listOfSecondPageErrorInputsTypes, () => NotifyOfPropertyChange(() => GetChangeForwardInputPageButton));
                }
            }
        }

        public string PropPostCodeInput
        {
            get
            {
                // return post code with format xx-xxx
                return StringFormat.ConvertToPostCodeFormat(GetPostCodeInput.Value);
            }
            set
            {
                // Replace the dashes with blank character - when we type from keyboard
                value.Replace("-", "");
                GetPostCodeInput.Value = value;
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_RECEIVER_POST_CODE,
                    Value = GetPostCodeInput.Value
                });
                NotifyOfPropertyChange(() => GetPostCodeInput);
                NotifyOfPropertyChange(() => PropPostCodeInput);

                // Live validate if validation is turned on
                if (_isLiveValidation)
                {
                    if (GetPostCodeInput.Value.Length == 5)
                    {
                        GetErrorPostCodeLabel.IsVisible = false;
                        NotifyOfPropertyChange(() => GetErrorPostCodeLabel);
                        RemoveUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetPostCodeInput.Type);
                    }
                    else
                    {
                        if (!_isFirstView)
                        {
                            GetErrorPostCodeLabel.IsVisible = true;
                            NotifyOfPropertyChange(() => GetErrorPostCodeLabel);
                        }
                        AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetPostCodeInput.Type);
                    }
                    ChangeErrorStyleOfButton(GetChangeForwardInputPageButton, _listOfSecondPageErrorInputsTypes, () => NotifyOfPropertyChange(() => GetChangeForwardInputPageButton));
                }
            }
        }

        public string PropStreetInput
        {
            get
            {
                return GetStreetInput.Value;
            }
            set
            {
                GetStreetInput.Value = StringFormat.ConvertToNameFormat(value);
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_RECEIVER_STREET,
                    Value = GetStreetInput.Value
                });
                NotifyOfPropertyChange(() => GetStreetInput);
                NotifyOfPropertyChange(() => PropStreetInput);

                // Live validate if validation is turned on
                if (_isLiveValidation)
                {
                    if (GetStreetInput.Value.Length > 0)
                    {
                        GetErrorStreetLabel.IsVisible = false;
                        NotifyOfPropertyChange(() => GetErrorStreetLabel);
                        RemoveUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetStreetInput.Type);
                    }
                    else
                    {
                        if (!_isFirstView)
                        {
                            GetErrorStreetLabel.IsVisible = true;
                            NotifyOfPropertyChange(() => GetErrorStreetLabel);
                        }
                        AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetStreetInput.Type);
                    }
                    ChangeErrorStyleOfButton(GetChangeForwardInputPageButton, _listOfSecondPageErrorInputsTypes, () => NotifyOfPropertyChange(() => GetChangeForwardInputPageButton));
                }
            }
        }

        public string PropHouseNumberInput
        {
            get
            {
                return GetHouseNumberInput.Value;
            }
            set
            {
                GetHouseNumberInput.Value = value;
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_RECEIVER_HOUSE_NUMBER,
                    Value = GetHouseNumberInput.Value
                });
                NotifyOfPropertyChange(() => GetHouseNumberInput);
                NotifyOfPropertyChange(() => PropHouseNumberInput);

                // Live validate if validation is turned on
                if (_isLiveValidation)
                {
                    if (GetHouseNumberInput.Value.Length > 0)
                    {
                        GetErrorHouseNumberLabel.IsVisible = false;
                        NotifyOfPropertyChange(() => GetErrorHouseNumberLabel);
                        RemoveUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetHouseNumberInput.Type);
                    }
                    else
                    {
                        if (!_isFirstView)
                        {
                            GetErrorHouseNumberLabel.IsVisible = true;
                            NotifyOfPropertyChange(() => GetErrorHouseNumberLabel);
                        }
                        AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetHouseNumberInput.Type);
                    }
                    ChangeErrorStyleOfButton(GetChangeForwardInputPageButton, _listOfSecondPageErrorInputsTypes, () => NotifyOfPropertyChange(() => GetChangeForwardInputPageButton));
                }
            }
        }

        public string PropApartmentNumberInput
        {
            get
            {
                return GetApartmentNumberInput.Value;
            }
            set
            {
                GetApartmentNumberInput.Value = value;
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_RECEIVER_APARTMENT_NUMBER,
                    Value = GetApartmentNumberInput.Value
                });
                NotifyOfPropertyChange(() => GetApartmentNumberInput);
                NotifyOfPropertyChange(() => PropApartmentNumberInput);
            }
        }

        public string PropHouseLetterInput
        {
            get
            {
                return GetHouseLetterInput.Value;
            }
            set
            {
                GetHouseLetterInput.Value = value;
                _eventAggregator.PublishOnUIThread(new HandleInputField
                {
                    Type = InputTypes.SEND_RECEIVER_HOUSE_LETTER,
                    Value = GetHouseNumberInput.Value
                });
                NotifyOfPropertyChange(() => GetHouseLetterInput);
                NotifyOfPropertyChange(() => PropHouseLetterInput);

                // Live validate if validation is turned on
                if (_isLiveValidation)
                {
                    if (GetHouseLetterInput.Value.Length == 0 || GetHouseLetterInput.Value.Length == 1)
                    {
                        GetErrorHouseLetterLabel.IsVisible = false;
                        NotifyOfPropertyChange(() => GetErrorHouseLetterLabel);
                        RemoveUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetHouseLetterInput.Type);
                    }
                    else
                    {
                        if (!_isFirstView)
                        {
                            GetErrorHouseLetterLabel.IsVisible = true;
                            NotifyOfPropertyChange(() => GetErrorHouseLetterLabel);
                        }
                        AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetHouseLetterInput.Type);
                    }
                    ChangeErrorStyleOfButton(GetChangeForwardInputPageButton, _listOfSecondPageErrorInputsTypes, () => NotifyOfPropertyChange(() => GetChangeForwardInputPageButton));
                }
            }
        }

        // -- Methods --
        public void LoadSendTheParcelSenderView()
        {
            HideKeyboard();

            _eventAggregator.PublishOnUIThread(new HandleSendTheParcelButtonClick
            {
                Type = SendTheParcelButtonClickTypes.SEND_THE_PARCEL_SENDER
            });
        }

        public void LoadSendTheParcelSummaryView()
        {
            // Trun on live validation of the inputs
            _isLiveValidation = true;

            // If validation is successful, hide the keyboard and change the view
            if (ValidateInputs())
            {
                HideKeyboard();

                _eventAggregator.PublishOnUIThread(new HandleSendTheParcelButtonClick
                {
                    Type = SendTheParcelButtonClickTypes.SEND_THE_PARCEL_SUMMARY
                });
            }
        }

        public void LoadPreviousPage()
        {
            LoadPage(false);
        }

        public void LoadNextPage()
        {
            LoadPage(true);
        }

        // Handle window width change
        public void Handle(HandleWindowWidth message)
        {
            // labels
            GetTitleLabel.PaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - standardInputWidth) / 2;
            NotifyOfPropertyChange(() => GetTitleLabel);

            double basicPaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardInputWidth * 2 + standardInputMarginLeft)) / 2;
            double smallBasicPaddingLeftX = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardInputWidth * 3 + standardInputMarginLeft * 2)) / 2;
            GetFirstNameLabel.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetFirstNameLabel);

            GetErrorFirstNameLabel.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetErrorFirstNameLabel);

            GetLastNameLabel.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetLastNameLabel);

            GetErrorLastNameLabel.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetErrorLastNameLabel);

            GetPhoneNumberLabel.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetPhoneNumberLabel);

            GetErrorPhoneNumberLabel.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);

            GetCityLabel.PaddingLeftX = smallBasicPaddingLeftX;
            NotifyOfPropertyChange(() => GetCityLabel);

            GetErrorCityLabel.PaddingLeftX = smallBasicPaddingLeftX;
            NotifyOfPropertyChange(() => GetErrorCityLabel);

            GetPostCodeLabel.PaddingLeftX = smallBasicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetPostCodeLabel);

            GetErrorPostCodeLabel.PaddingLeftX = smallBasicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetErrorPostCodeLabel);

            GetStreetLabel.PaddingLeftX = smallBasicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2;
            NotifyOfPropertyChange(() => GetStreetLabel);

            GetErrorStreetLabel.PaddingLeftX = smallBasicPaddingLeftX + (standardInputWidth + standardInputMarginLeft) * 2;
            NotifyOfPropertyChange(() => GetErrorStreetLabel);

            GetHouseNumberLabel.PaddingLeftX = smallBasicPaddingLeftX;
            NotifyOfPropertyChange(() => GetHouseNumberLabel);

            GetErrorHouseNumberLabel.PaddingLeftX = smallBasicPaddingLeftX;
            NotifyOfPropertyChange(() => GetErrorHouseNumberLabel);

            GetApartmentNumberLabel.PaddingLeftX = smallBasicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetApartmentNumberLabel);

            GetHouseLetterLabel.PaddingLeftX = smallBasicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2;
            NotifyOfPropertyChange(() => GetHouseLetterLabel);

            GetErrorHouseLetterLabel.PaddingLeftX = smallBasicPaddingLeftX + (standardInputWidth + standardInputMarginLeft) * 2;
            NotifyOfPropertyChange(() => GetErrorHouseLetterLabel);

            // inputs
            GetFirstNameInput.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetFirstNameInput);

            GetLastNameInput.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetLastNameInput);

            GetPhoneNumberInput.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetPhoneNumberInput);

            GetCityInput.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetCityInput);

            GetPostCodeInput.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetPostCodeInput);

            GetStreetInput.PaddingLeftX = basicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2;
            NotifyOfPropertyChange(() => GetStreetInput);

            GetHouseNumberInput.PaddingLeftX = basicPaddingLeftX;
            NotifyOfPropertyChange(() => GetHouseNumberInput);

            GetApartmentNumberInput.PaddingLeftX = basicPaddingLeftX + standardInputWidth + standardInputMarginLeft;
            NotifyOfPropertyChange(() => GetApartmentNumberInput);

            GetHouseLetterInput.PaddingLeftX = basicPaddingLeftX + (standardInputWidth + standardInputMarginLeft)*2;
            NotifyOfPropertyChange(() => GetHouseLetterInput);

            // buttons
            double buttonRowSize = standardButtonMarginLeft + GetSendTheParcelSenderButton.Width + GetSendTheParcelSummaryButton.Width;
            double basicButtonPaddingLeft = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - buttonRowSize) / 2;

            GetSendTheParcelSummaryButton.PaddingLeftX = basicButtonPaddingLeft;
            NotifyOfPropertyChange(() => GetSendTheParcelSummaryButton);

            GetSendTheParcelSenderButton.PaddingLeftX = basicButtonPaddingLeft + GetSendTheParcelSummaryButton.Width + standardButtonMarginLeft;
            NotifyOfPropertyChange(() => GetSendTheParcelSenderButton);

            GetChangeForwardInputPageButton.PaddingLeftX = ((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - 60;
            NotifyOfPropertyChange(() => GetChangeForwardInputPageButton);
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

            GetPhoneNumberLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight + standardInputMarginTop + (standardLabelHeight + standardLabelMarginTop) * 2;
            NotifyOfPropertyChange(() => GetPhoneNumberLabel);

            GetErrorPhoneNumberLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardInputHeight * 2 + standardInputMarginTop + standardLabelHeight * 3 + standardLabelMarginTop * 4;
            NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);

            GetCityLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop;
            NotifyOfPropertyChange(() => GetCityLabel);

            GetErrorCityLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop * 2 + standardInputHeight;
            NotifyOfPropertyChange(() => GetErrorCityLabel);

            GetPostCodeLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop;
            NotifyOfPropertyChange(() => GetPostCodeLabel);

            GetErrorPostCodeLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop * 2 + standardInputHeight;
            NotifyOfPropertyChange(() => GetErrorPostCodeLabel);

            GetStreetLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop;
            NotifyOfPropertyChange(() => GetStreetLabel);

            GetErrorStreetLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop * 2 + standardInputHeight;
            NotifyOfPropertyChange(() => GetErrorStreetLabel);

            GetHouseNumberLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 2 + standardInputHeight + standardInputMarginTop;
            NotifyOfPropertyChange(() => GetHouseNumberLabel);

            GetErrorHouseNumberLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight * 3 + standardLabelMarginTop * 4 + standardInputHeight * 2 + standardInputMarginTop;
            NotifyOfPropertyChange(() => GetErrorHouseNumberLabel);

            GetApartmentNumberLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 2 + standardInputHeight + standardInputMarginTop;
            NotifyOfPropertyChange(() => GetApartmentNumberLabel);

            GetHouseLetterLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 2 + standardInputHeight + standardInputMarginTop;
            NotifyOfPropertyChange(() => GetHouseLetterLabel);

            GetErrorHouseLetterLabel.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 2 + standardInputHeight + standardInputMarginTop;
            NotifyOfPropertyChange(() => GetErrorHouseLetterLabel);

            // inputs
            GetFirstNameInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetFirstNameInput);

            GetLastNameInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetLastNameInput);

            GetPhoneNumberInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 3 + standardInputMarginTop + standardInputHeight;
            NotifyOfPropertyChange(() => GetPhoneNumberInput);

            GetCityInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetCityInput);

            GetPostCodeInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetPostCodeInput);

            GetStreetInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetStreetInput);

            GetHouseNumberInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 3 + standardInputHeight + standardInputMarginTop;
            NotifyOfPropertyChange(() => GetHouseNumberInput);

            GetApartmentNumberInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 3 + standardInputHeight + standardInputMarginTop;
            NotifyOfPropertyChange(() => GetApartmentNumberInput);

            GetHouseLetterInput.PaddingTopY = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardLabelHeight + standardLabelMarginTop) * 3 + standardInputHeight + standardInputMarginTop;
            NotifyOfPropertyChange(() => GetHouseLetterInput);

            // buttons
            double buttonPaddingTop = basicPaddingTopY + titleLabelHeight + titleLabelMarginTop + (standardInputHeight + standardInputMarginTop) * 2 + (standardLabelHeight + standardLabelMarginTop) * 4;
            GetSendTheParcelSummaryButton.PaddingTopY = buttonPaddingTop;
            NotifyOfPropertyChange(() => GetSendTheParcelSummaryButton);

            GetSendTheParcelSenderButton.PaddingTopY = buttonPaddingTop;
            NotifyOfPropertyChange(() => GetSendTheParcelSenderButton);

            double inputPagePaddingTopY = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.04 + titleLabelHeight + titleLabelMarginTop + standardLabelHeight + standardLabelMarginTop + standardInputHeight + standardInputMarginTop / 2;
            GetChangeBackwardInputPageButton.PaddingTopY = inputPagePaddingTopY;
            NotifyOfPropertyChange(() => GetChangeBackwardInputPageButton);

            GetChangeForwardInputPageButton.PaddingTopY = inputPagePaddingTopY;
            NotifyOfPropertyChange(() => GetChangeForwardInputPageButton);
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
                if (GetSendTheParcelSenderButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadSendTheParcelSenderView();
                }
                else if (GetSendTheParcelSummaryButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadSendTheParcelSummaryView();
                }
                else if (!_isFirstView && GetChangeBackwardInputPageButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadPreviousPage();
                }
                else if (_isFirstView && GetChangeForwardInputPageButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadNextPage();
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
            if (_focusedInput == InputTypes.SEND_RECEIVER_FIRST_NAME)
            {
                inputToChange = GetFirstNameInput;
            }
            else if (_focusedInput == InputTypes.SEND_RECEIVER_LAST_NAME)
            {
                inputToChange = GetLastNameInput;
            }
            else if (_focusedInput == InputTypes.SEND_RECEIVER_PHONE_NUMBER)
            {
                inputToChange = GetPhoneNumberInput;
            }
            else if (_focusedInput == InputTypes.SEND_RECEIVER_CITY)
            {
                inputToChange = GetCityInput;
            }
            else if (_focusedInput == InputTypes.SEND_RECEIVER_POST_CODE)
            {
                inputToChange = GetPostCodeInput;
            }
            else if (_focusedInput == InputTypes.SEND_RECEIVER_STREET)
            {
                inputToChange = GetStreetInput;
            }
            else if (_focusedInput == InputTypes.SEND_RECEIVER_HOUSE_NUMBER)
            {
                inputToChange = GetHouseNumberInput;
            }
            else if (_focusedInput == InputTypes.SEND_RECEIVER_APARTMENT_NUMBER)
            {
                inputToChange = GetApartmentNumberInput;
            }
            else if (_focusedInput == InputTypes.SEND_RECEIVER_HOUSE_LETTER)
            {
                inputToChange = GetHouseLetterInput;
            }

            Console.WriteLine("Focused input type: " + _focusedInput);
            // Check which key is clicked and manage appropriate action, if there's a focus on any input
            if (inputToChange != null)
            {
                if (message.KeyType != KeyTypes.DELETE)
                {
                    bool canAddKey = false;
                    // Check whether we can add key or not (validate the input)
                    if (inputToChange.Type == InputTypes.SEND_RECEIVER_PHONE_NUMBER)
                    {
                        if (inputToChange.Value.Length < 9)
                        {
                            canAddKey = true;
                        }
                    }
                    else if (inputToChange.Type == InputTypes.SEND_RECEIVER_POST_CODE)
                    {
                        if (inputToChange.Value.Length < 5)
                        {
                            canAddKey = true;
                        }
                    }
                    else if (inputToChange.Type == InputTypes.SEND_RECEIVER_HOUSE_LETTER)
                    {
                        if (inputToChange.Value.Length < 1)
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
                if (inputToChange.Type == InputTypes.SEND_RECEIVER_FIRST_NAME)
                {
                    PropFirstNameInput = newValue;
                    NotifyOfPropertyChange(() => PropFirstNameInput);
                    Console.WriteLine("Change of first name input: " + GetFirstNameInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_RECEIVER_LAST_NAME)
                {
                    PropLastNameInput = newValue;
                    NotifyOfPropertyChange(() => PropLastNameInput);
                    Console.WriteLine("Change of last name input: " + GetLastNameInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_RECEIVER_PHONE_NUMBER)
                {
                    PropPhoneNumberInput = newValue;
                    NotifyOfPropertyChange(() => PropPhoneNumberInput);
                    Console.WriteLine("Change of phone input: " + GetPhoneNumberInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_RECEIVER_CITY)
                {
                    PropCityInput = newValue;
                    NotifyOfPropertyChange(() => PropCityInput);
                    Console.WriteLine("Change of city input: " + GetCityInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_RECEIVER_POST_CODE)
                {
                    PropPostCodeInput = newValue;
                    NotifyOfPropertyChange(() => PropPostCodeInput);
                    Console.WriteLine("Change of city input: " + GetPostCodeInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_RECEIVER_STREET)
                {
                    PropStreetInput = newValue;
                    NotifyOfPropertyChange(() => PropStreetInput);
                    Console.WriteLine("Change of city input: " + GetStreetInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_RECEIVER_HOUSE_NUMBER)
                {
                    PropHouseNumberInput = newValue;
                    NotifyOfPropertyChange(() => PropHouseNumberInput);
                    Console.WriteLine("Change of city input: " + GetHouseNumberInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_RECEIVER_APARTMENT_NUMBER)
                {
                    PropApartmentNumberInput = newValue;
                    NotifyOfPropertyChange(() => PropApartmentNumberInput);
                    Console.WriteLine("Change of city input: " + GetApartmentNumberInput.Value);
                }
                else if (inputToChange.Type == InputTypes.SEND_RECEIVER_HOUSE_LETTER)
                {
                    PropHouseLetterInput = newValue;
                    NotifyOfPropertyChange(() => PropHouseLetterInput);
                    Console.WriteLine("Change of city input: " + GetHouseLetterInput.Value);
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

            NotifyOfPropertyChange(() => GetSendTheParcelSenderButton);
            NotifyOfPropertyChange(() => GetSendTheParcelSummaryButton);
            NotifyOfPropertyChange(() => GetChangeBackwardInputPageButton);
            NotifyOfPropertyChange(() => GetChangeForwardInputPageButton);
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
                if (input.Type == InputTypes.SEND_RECEIVER_FIRST_NAME)
                {
                    keyboardTypeToSet = KeyboardTypes.LETTER;
                    NotifyOfPropertyChange(() => GetFirstNameInput);
                }
                else if (input.Type == InputTypes.SEND_RECEIVER_LAST_NAME)
                {
                    keyboardTypeToSet = KeyboardTypes.LETTER;
                    NotifyOfPropertyChange(() => GetLastNameInput);
                }
                else if (input.Type == InputTypes.SEND_RECEIVER_PHONE_NUMBER)
                {
                    keyboardTypeToSet = KeyboardTypes.NUMERIC;
                    NotifyOfPropertyChange(() => GetPhoneNumberInput);
                }
                else if (input.Type == InputTypes.SEND_RECEIVER_CITY)
                {
                    keyboardTypeToSet = KeyboardTypes.LETTER;
                    NotifyOfPropertyChange(() => GetCityInput);
                }
                else if (input.Type == InputTypes.SEND_RECEIVER_POST_CODE)
                {
                    keyboardTypeToSet = KeyboardTypes.NUMERIC;
                    NotifyOfPropertyChange(() => GetPostCodeInput);
                }
                else if (input.Type == InputTypes.SEND_RECEIVER_STREET)
                {
                    keyboardTypeToSet = KeyboardTypes.LETTER;
                    NotifyOfPropertyChange(() => GetStreetInput);
                }
                else if (input.Type == InputTypes.SEND_RECEIVER_HOUSE_NUMBER)
                {
                    keyboardTypeToSet = KeyboardTypes.NUMERIC;
                    NotifyOfPropertyChange(() => GetHouseNumberInput);
                }
                else if (input.Type == InputTypes.SEND_RECEIVER_APARTMENT_NUMBER)
                {
                    keyboardTypeToSet = KeyboardTypes.NUMERIC;
                    NotifyOfPropertyChange(() => GetApartmentNumberInput);
                }
                else if (input.Type == InputTypes.SEND_RECEIVER_HOUSE_LETTER)
                {
                    keyboardTypeToSet = KeyboardTypes.LETTER;
                    NotifyOfPropertyChange(() => GetHouseLetterInput);
                }

                // Check the previous and actual type of focused input
                if (_focusedInput != input.Type)
                {
                    //Toggle the provious focused input
                    if (_focusedInput != InputTypes.NO_INPUT)
                    {
                        Input prevFocusedInput = _inputs.Find(fInput => fInput.Type == _focusedInput);
                        prevFocusedInput.IsFocused = !prevFocusedInput.IsFocused;
                        if (_focusedInput == InputTypes.SEND_RECEIVER_FIRST_NAME)
                        {
                            NotifyOfPropertyChange(() => GetFirstNameInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_RECEIVER_LAST_NAME)
                        {
                            NotifyOfPropertyChange(() => GetLastNameInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_RECEIVER_PHONE_NUMBER)
                        {
                            NotifyOfPropertyChange(() => GetPhoneNumberInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_RECEIVER_CITY)
                        {
                            NotifyOfPropertyChange(() => GetCityInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_RECEIVER_POST_CODE)
                        {
                            NotifyOfPropertyChange(() => GetPostCodeInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_RECEIVER_STREET)
                        {
                            NotifyOfPropertyChange(() => GetStreetInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_RECEIVER_HOUSE_NUMBER)
                        {
                            NotifyOfPropertyChange(() => GetHouseNumberInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_RECEIVER_APARTMENT_NUMBER)
                        {
                            NotifyOfPropertyChange(() => GetApartmentNumberInput);
                        }
                        else if (_focusedInput == InputTypes.SEND_RECEIVER_HOUSE_LETTER)
                        {
                            NotifyOfPropertyChange(() => GetHouseLetterInput);
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

        private void AddUniqueInputTypeToList(List<InputTypes> listToChange, InputTypes inputTypeToAdd)
        {
            if (!listToChange.Contains(inputTypeToAdd))
            {
                listToChange.Add(inputTypeToAdd);
            }
        }

        private void RemoveUniqueInputTypeToList(List<InputTypes> listToChange, InputTypes inputTypeToRemove)
        {
            listToChange.Remove(inputTypeToRemove);
        }

        private void ChangeErrorStyleOfButton(Button buttonToChange, List<InputTypes> listOfErrors, System.Action notifyFunc)
        {
            if (listOfErrors.Count == 0)
            {
                // No error
                buttonToChange.IsErrorStyle = false;
            }
            else
            {
                // Error
                buttonToChange.IsErrorStyle = true;
            }
            notifyFunc();
        }

        private bool ValidateInputs()
        {
            // Validate inputs
            bool canSubmit = true;
            // Validate First Name
            if (GetFirstNameInput.Value.Length == 0)
            {
                canSubmit = false;
                GetChangeBackwardInputPageButton.IsErrorStyle = true;
                AddUniqueInputTypeToList(_listOfFirstPageErrorInputsTypes, GetFirstNameInput.Type);
                if (_isFirstView)
                {
                    GetErrorFirstNameLabel.IsVisible = true;
                    NotifyOfPropertyChange(() => GetErrorFirstNameLabel);
                }
            }

            // Validate Last Name
            if (GetLastNameInput.Value.Length == 0)
            {
                canSubmit = false;
                GetChangeBackwardInputPageButton.IsErrorStyle = true;
                AddUniqueInputTypeToList(_listOfFirstPageErrorInputsTypes, GetLastNameInput.Type);
                if (_isFirstView)
                {
                    GetErrorLastNameLabel.IsVisible = true;
                    NotifyOfPropertyChange(() => GetErrorLastNameLabel);
                }
            }

            // Validate Phone Number
            if (GetPhoneNumberInput.Value.Length != 9)
            {
                canSubmit = false;
                GetChangeBackwardInputPageButton.IsErrorStyle = true;
                AddUniqueInputTypeToList(_listOfFirstPageErrorInputsTypes, GetPhoneNumberInput.Type);
                if (_isFirstView)
                {
                    GetErrorPhoneNumberLabel.IsVisible = true;
                    NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);
                }
            }

            // Second view validation
            // Validate City
            if (GetCityInput.Value.Length == 0)
            {
                canSubmit = false;
                GetChangeForwardInputPageButton.IsErrorStyle = true;
                AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetCityInput.Type);
                if (!_isFirstView)
                {
                    GetErrorCityLabel.IsVisible = true;
                    NotifyOfPropertyChange(() => GetErrorCityLabel);
                }
            }

            // Validate Post Code
            if (GetPostCodeInput.Value.Length != 5)
            {
                canSubmit = false;
                GetChangeForwardInputPageButton.IsErrorStyle = true;
                AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetPostCodeInput.Type);
                if (!_isFirstView)
                {
                    GetErrorPostCodeLabel.IsVisible = true;
                    NotifyOfPropertyChange(() => GetErrorPostCodeLabel);
                }
            }

            // Validate Street
            if (GetStreetInput.Value.Length == 0)
            {
                canSubmit = false;
                GetChangeForwardInputPageButton.IsErrorStyle = true;
                AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetStreetInput.Type);
                if (!_isFirstView)
                {
                    GetErrorStreetLabel.IsVisible = true;
                    NotifyOfPropertyChange(() => GetErrorStreetLabel);
                }
            }

            // Validate House Number
            if (GetHouseNumberInput.Value.Length == 0)
            {
                canSubmit = false;
                GetChangeForwardInputPageButton.IsErrorStyle = true;
                AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetHouseNumberInput.Type);
                if (!_isFirstView)
                {
                    GetErrorHouseNumberLabel.IsVisible = true;
                    NotifyOfPropertyChange(() => GetErrorHouseNumberLabel);
                }
            }

            // Validate House Letter
            if (GetApartmentNumberInput.Value.Length > 1)
            {
                canSubmit = false;
                GetChangeForwardInputPageButton.IsErrorStyle = true;
                AddUniqueInputTypeToList(_listOfSecondPageErrorInputsTypes, GetApartmentNumberInput.Type);
                if (!_isFirstView)
                {
                    GetErrorHouseLetterLabel.IsVisible = true;
                    NotifyOfPropertyChange(() => GetErrorHouseLetterLabel);
                }
            }

            if (_isFirstView)
            {
                GetErrorCityLabel.IsVisible = false;
                GetErrorPostCodeLabel.IsVisible = false;
                GetErrorStreetLabel.IsVisible = false;
                GetErrorHouseNumberLabel.IsVisible = false;
                GetErrorHouseLetterLabel.IsVisible = false;

                NotifyOfPropertyChange(() => GetErrorCityLabel);
                NotifyOfPropertyChange(() => GetErrorPostCodeLabel);
                NotifyOfPropertyChange(() => GetErrorStreetLabel);
                NotifyOfPropertyChange(() => GetErrorHouseNumberLabel);
                NotifyOfPropertyChange(() => GetErrorHouseLetterLabel);
            }
            else
            {
                GetErrorFirstNameLabel.IsVisible = false;
                GetErrorLastNameLabel.IsVisible = false;
                GetErrorPhoneNumberLabel.IsVisible = false;

                NotifyOfPropertyChange(() => GetErrorFirstNameLabel);
                NotifyOfPropertyChange(() => GetErrorLastNameLabel);
                NotifyOfPropertyChange(() => GetErrorPhoneNumberLabel);
            }

            return canSubmit;
        }

        private void LoadPage(bool isNextPage)
        {
            // Changing the page view flag
            _isFirstView = !_isFirstView;

            // Changing the visibility of the inputs navigation buttons
            bool backwardButtonVisibility = false;
            bool forwardButtonVisibility = true;
            if (isNextPage)
            {
                backwardButtonVisibility = true;
                forwardButtonVisibility = false;
            }
            GetChangeBackwardInputPageButton.IsVisible = backwardButtonVisibility;
            GetChangeForwardInputPageButton.IsVisible = forwardButtonVisibility;

            // Validating the inputs
            ValidateInputs();

            // Changing the focus property of the previous focused input
            if (_focusedInput != InputTypes.NO_INPUT)
            {
                Input prevFocusedInput = _inputs.Find(fInput => fInput.Type == _focusedInput);
                prevFocusedInput.IsFocused = !prevFocusedInput.IsFocused;
                HideKeyboard();
            }

            // Changing visibility of the inputs
            foreach (Input input in _inputs)
            {
                input.IsVisible = !input.IsVisible;
            }

            NotifyOfPropertyChange(() => GetFirstNameInput);
            NotifyOfPropertyChange(() => GetLastNameInput);
            NotifyOfPropertyChange(() => GetPhoneNumberInput);
            NotifyOfPropertyChange(() => GetCityInput);
            NotifyOfPropertyChange(() => GetPostCodeInput);
            NotifyOfPropertyChange(() => GetStreetInput);
            NotifyOfPropertyChange(() => GetApartmentNumberInput);
            NotifyOfPropertyChange(() => GetHouseLetterInput);
        }
    }
}
