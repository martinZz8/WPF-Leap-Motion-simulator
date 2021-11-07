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
    class SendTheParcelSummaryViewModel: Screen, IHandle<HandleCursorHandGesture>, IHandle<HandleWindowWidth>, IHandle<HandleWindowHeight>, IHandle<HandleCrusorMove>
    {
        private IEventAggregator _eventAggregator;

        // -- Variables of the main window --
        private readonly TDOWindowPadding mainWindowPadding;

        //-- Variables of this window --
        private double[] _gridColumnMultipliers;
        private List<Button> _buttons;
        private List<Label> _labels;

        private readonly double standardLabelHeight = 30;
        private readonly double standardLabelWidth = 220;
        private readonly double standardLabelMarginTop = 20;
        private readonly double standardLabelMarginLeft = 10;
        private readonly double standardButtonWidth = 200;
        private readonly double standardButtonHeight = 100;
        private readonly double standardButtonMarginLeft = 20;

        public SendTheParcelSummaryViewModel(IEventAggregator eventAggregator, TDOWindowSize windowSize, TDOWindowPadding windowPadding, TDOSendTheParcelSenderInputValues senderValues, TDOSendTheParcelReceiverInputValues receiverValues)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            mainWindowPadding = windowPadding;
            _gridColumnMultipliers = new double[] { 1.5, 5, 1.5 };

            double basicPaddingTop = (windowSize.WindowHeight - windowPadding.PaddingTop - windowPadding.PaddingBottom) * 0.08;
            double labelPaddingLeft = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardLabelWidth*2 + standardLabelMarginLeft)) / 2;
            double smallLabelPaddingLeft = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardLabelWidth*3 + standardLabelMarginLeft*2)) / 2;

            double basicButtonPaddingLeft = (((windowSize.WindowWidth - windowPadding.PaddingLeft - windowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardButtonWidth*2 + standardButtonMarginLeft)) / 2;

            _labels = new List<Label>
            {
                new Label
                {
                    Width = standardLabelWidth*2+standardLabelMarginLeft,
                    Height = standardLabelHeight,
                    PaddingLeftX = labelPaddingLeft,
                    PaddingTopY = basicPaddingTop,
                    FontSize = 18,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_SENDER_TITLE,
                    Value = "Dane nadawcy"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = labelPaddingLeft,
                    PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_SENDER_FIRST_NAME,
                    Value = $"Imię: {senderValues.FirstName}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = labelPaddingLeft + standardLabelWidth + standardLabelMarginLeft,
                    PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_SENDER_LAST_NAME,
                    Value = $"Nazwisko: {senderValues.LastName}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = labelPaddingLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*2,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_SENDER_EMAIL,
                    Value = $"Adres email: {senderValues.Email}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = labelPaddingLeft + standardLabelWidth + standardLabelMarginLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*2,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_SENDER_PHONE_NUMBER,
                    Value = $"Numer telefonu: {StringFormat.ConvertToPhoneFormat(senderValues.PhoneNumber)}"
                },
                new Label
                {
                    Width = standardLabelWidth*3+standardLabelMarginLeft*2,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*3,
                    FontSize = 18,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_TITLE,
                    Value = "Dane odbiorcy"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*4,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_FIRST_NAME,
                    Value = $"Imię: {receiverValues.FirstName}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft + standardLabelWidth + standardLabelMarginLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*4,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_LAST_NAME,
                    Value = $"Nazwisko: {receiverValues.LastName}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft + (standardLabelWidth + standardLabelMarginLeft)*2,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*4,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_PHONE_NUMBER,
                    Value = $"Numer telefonu: {StringFormat.ConvertToPhoneFormat(receiverValues.PhoneNumber)}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*5,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_CITY,
                    Value = $"Miasto: {receiverValues.City}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft + standardLabelWidth + standardLabelMarginLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*5,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_POST_CODE,
                    Value = $"Kod pocztowy: {StringFormat.ConvertToPostCodeFormat(receiverValues.PostCode)}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft + (standardLabelWidth + standardLabelMarginLeft)*2,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*5,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_STREET,
                    Value = $"Ulica: {receiverValues.Street}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*6,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_HOUSE_NUMBER,
                    Value = $"Numer domu: {receiverValues.HouseNumber}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft + standardLabelWidth + standardLabelMarginLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*6,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_APARTMENT_NUMBER,
                    Value = $"Numer mieszkania: {GetStringOrDash(receiverValues.ApartmentNumber)}"
                },
                new Label
                {
                    Width = standardLabelWidth,
                    Height = standardLabelHeight,
                    PaddingLeftX = smallLabelPaddingLeft + (standardLabelWidth + standardLabelMarginLeft)*2,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*6,
                    FontSize = 13,
                    FontWeight = "Bold",
                    TextColor = "#f1b938",
                    Type = LabelTypes.SEND_SUMMARY_RECEIVER_HOUSE_LETTER,
                    Value = $"Litera domu: {GetStringOrDash(receiverValues.HouseLetter)}"
                }
            };

            _buttons = new List<Button> {
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = basicButtonPaddingLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*7,
                    Type = ButtonTypes.VIEW_SUCCESS_SEND_THE_PARCEL,
                    Title = "Zatwierdź",
                    IsHovered = false
                },
                new Button
                {
                    Width = standardButtonWidth,
                    Height = standardButtonHeight,
                    PaddingLeftX = basicButtonPaddingLeft + standardButtonWidth + standardButtonMarginLeft,
                    PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop)*7,
                    Type = ButtonTypes.VIEW_SEND_THE_PARCEL_RECEIVER,
                    Title = "Wróć",
                    IsHovered = false
                },
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

        public Label GetSenderTitleLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_SENDER_TITLE);
            }
        }

        public Label GetSenderFirstNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_SENDER_FIRST_NAME);
            }
        }

        public Label GetSenderLastNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_SENDER_LAST_NAME);
            }
        }

        public Label GetSenderEmailLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_SENDER_EMAIL);
            }
        }

        public Label GetSenderPhoneNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_SENDER_PHONE_NUMBER);
            }
        }

        public Label GetReceiverTitleLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_TITLE);
            }
        }

        public Label GetReceiverFirstNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_FIRST_NAME);
            }
        }

        public Label GetReceiverLastNameLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_LAST_NAME);
            }
        }

        public Label GetReceiverPhoneNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_PHONE_NUMBER);
            }
        }

        public Label GetReceiverCityLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_CITY);
            }
        }

        public Label GetReceiverPostCodeLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_POST_CODE);
            }
        }

        public Label GetReceiverStreetLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_STREET);
            }
        }

        public Label GetReceiverHouseNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_HOUSE_NUMBER);
            }
        }

        public Label GetReceiverApartmentNumberLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_APARTMENT_NUMBER);
            }
        }

        public Label GetReceiverHouseLetterLabel
        {
            get
            {
                return _labels.Find(label => label.Type == LabelTypes.SEND_SUMMARY_RECEIVER_HOUSE_LETTER);
            }
        }

        public Button GetSendTheParcelReceiverViewButton
        {
            get
            {
                return Button.SearchButtonByType(_buttons, ButtonTypes.VIEW_SEND_THE_PARCEL_RECEIVER);
            }
        }

        public Button GetSuccessSendTheParcelViewButton
        {
            get
            {
                return Button.SearchButtonByType(_buttons, ButtonTypes.VIEW_SUCCESS_SEND_THE_PARCEL);
            }
        }

        // -- Methods --
        public void LoadSendTheParcelReceiverView()
        {
            _eventAggregator.PublishOnUIThread(new HandleSendTheParcelButtonClick
            {
                Type = SendTheParcelButtonClickTypes.SEND_THE_PARCEL_RECEIVER
            });
        }

        public void LoadSuccessSendTheParcelView()
        {
            _eventAggregator.PublishOnUIThread(new HandleSendTheParcelButtonClick
            {
                Type = SendTheParcelButtonClickTypes.SUCCESS_SEND
            });
        }

        // Handle window width change
        public void Handle(HandleWindowWidth message)
        {
            // labels
            double labelPaddingLeft = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardLabelWidth * 2 + standardLabelMarginLeft)) / 2;
            double smallLabelPaddingLeft = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardLabelWidth * 3 + standardLabelMarginLeft * 2)) / 2;

            GetSenderTitleLabel.PaddingLeftX = labelPaddingLeft;
            NotifyOfPropertyChange(() => GetSenderTitleLabel);

            GetSenderFirstNameLabel.PaddingLeftX = labelPaddingLeft;
            NotifyOfPropertyChange(() => GetSenderFirstNameLabel);

            GetSenderLastNameLabel.PaddingLeftX = labelPaddingLeft + standardLabelWidth + standardLabelMarginLeft;
            NotifyOfPropertyChange(() => GetSenderLastNameLabel);

            GetSenderEmailLabel.PaddingLeftX = labelPaddingLeft;
            NotifyOfPropertyChange(() => GetSenderEmailLabel);

            GetSenderPhoneNumberLabel.PaddingLeftX = labelPaddingLeft + standardLabelWidth + standardLabelMarginLeft;
            NotifyOfPropertyChange(() => GetSenderPhoneNumberLabel);

            GetReceiverTitleLabel.PaddingLeftX = smallLabelPaddingLeft;
            NotifyOfPropertyChange(() => GetReceiverTitleLabel);

            GetReceiverFirstNameLabel.PaddingLeftX = smallLabelPaddingLeft;
            NotifyOfPropertyChange(() => GetReceiverFirstNameLabel);

            GetReceiverLastNameLabel.PaddingLeftX = smallLabelPaddingLeft + standardLabelWidth + standardLabelMarginLeft;
            NotifyOfPropertyChange(() => GetReceiverLastNameLabel);

            GetReceiverPhoneNumberLabel.PaddingLeftX = smallLabelPaddingLeft + (standardLabelWidth + standardLabelMarginLeft)*2;
            NotifyOfPropertyChange(() => GetReceiverPhoneNumberLabel);

            GetReceiverCityLabel.PaddingLeftX = smallLabelPaddingLeft;
            NotifyOfPropertyChange(() => GetReceiverCityLabel);

            GetReceiverPostCodeLabel.PaddingLeftX = smallLabelPaddingLeft + standardLabelWidth + standardLabelMarginLeft;
            NotifyOfPropertyChange(() => GetReceiverPostCodeLabel);

            GetReceiverStreetLabel.PaddingLeftX = smallLabelPaddingLeft + (standardLabelWidth + standardLabelMarginLeft)*2;
            NotifyOfPropertyChange(() => GetReceiverStreetLabel);

            GetReceiverHouseNumberLabel.PaddingLeftX = smallLabelPaddingLeft;
            NotifyOfPropertyChange(() => GetReceiverHouseNumberLabel);

            GetReceiverApartmentNumberLabel.PaddingLeftX = smallLabelPaddingLeft + standardLabelWidth + standardLabelMarginLeft;
            NotifyOfPropertyChange(() => GetReceiverApartmentNumberLabel);

            GetReceiverHouseLetterLabel.PaddingLeftX = smallLabelPaddingLeft + (standardLabelWidth + standardLabelMarginLeft)*2;
            NotifyOfPropertyChange(() => GetReceiverHouseLetterLabel);

            // buttons
            double basicButtonPaddingLeft = (((message.WindowWidth - mainWindowPadding.PaddingLeft - mainWindowPadding.PaddingRight) * _gridColumnMultipliers[1] / GridColumnTotalDenominator) - (standardButtonWidth * 2 + standardButtonMarginLeft)) / 2;
            GetSuccessSendTheParcelViewButton.PaddingLeftX = basicButtonPaddingLeft;
            NotifyOfPropertyChange(() => GetSuccessSendTheParcelViewButton);

            GetSendTheParcelReceiverViewButton.PaddingLeftX = basicButtonPaddingLeft + standardButtonWidth + standardButtonMarginLeft;
            NotifyOfPropertyChange(() => GetSendTheParcelReceiverViewButton);
        }

        // Handle window height change
        public void Handle(HandleWindowHeight message)
        {
            double basicPaddingTop = (message.WindowHeight - mainWindowPadding.PaddingTop - mainWindowPadding.PaddingBottom) * 0.08;

            // labels
            GetSenderTitleLabel.PaddingTopY = basicPaddingTop;
            NotifyOfPropertyChange(() => GetSenderTitleLabel);

            GetSenderFirstNameLabel.PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetSenderFirstNameLabel);

            GetSenderLastNameLabel.PaddingTopY = basicPaddingTop + standardLabelHeight + standardLabelMarginTop;
            NotifyOfPropertyChange(() => GetSenderLastNameLabel);

            GetSenderEmailLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 2;
            NotifyOfPropertyChange(() => GetSenderEmailLabel);

            GetSenderPhoneNumberLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 2;
            NotifyOfPropertyChange(() => GetSenderPhoneNumberLabel);

            GetReceiverTitleLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 3;
            NotifyOfPropertyChange(() => GetReceiverTitleLabel);

            GetReceiverFirstNameLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 4;
            NotifyOfPropertyChange(() => GetReceiverFirstNameLabel);

            GetReceiverLastNameLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 4;
            NotifyOfPropertyChange(() => GetReceiverLastNameLabel);

            GetReceiverPhoneNumberLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 4;
            NotifyOfPropertyChange(() => GetReceiverPhoneNumberLabel);

            GetReceiverCityLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 5;
            NotifyOfPropertyChange(() => GetReceiverCityLabel);

            GetReceiverPostCodeLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 5;
            NotifyOfPropertyChange(() => GetReceiverPostCodeLabel);

            GetReceiverStreetLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 5;
            NotifyOfPropertyChange(() => GetReceiverStreetLabel);

            GetReceiverHouseNumberLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 6;
            NotifyOfPropertyChange(() => GetReceiverHouseNumberLabel);

            GetReceiverApartmentNumberLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 6;
            NotifyOfPropertyChange(() => GetReceiverApartmentNumberLabel);

            GetReceiverHouseLetterLabel.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 6;
            NotifyOfPropertyChange(() => GetReceiverHouseLetterLabel);

            // buttons
            GetSuccessSendTheParcelViewButton.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 7;
            NotifyOfPropertyChange(() => GetSuccessSendTheParcelViewButton);

            GetSendTheParcelReceiverViewButton.PaddingTopY = basicPaddingTop + (standardLabelHeight + standardLabelMarginTop) * 7;
            NotifyOfPropertyChange(() => GetSendTheParcelReceiverViewButton);
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
                if (GetSuccessSendTheParcelViewButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadSuccessSendTheParcelView();
                }
                else if (GetSendTheParcelReceiverViewButton.IsCursorInsideTheButton(relativeCursor))
                {
                    LoadSendTheParcelReceiverView();
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

            NotifyOfPropertyChange(() => GetSuccessSendTheParcelViewButton);
            NotifyOfPropertyChange(() => GetSendTheParcelReceiverViewButton);
        }

        private string GetStringOrDash(string str)
        {
            if (str.Length > 0)
            {
                return str;
            }

            return "-";
        }
    }
}
