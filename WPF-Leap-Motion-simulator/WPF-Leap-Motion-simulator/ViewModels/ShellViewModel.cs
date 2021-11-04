using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

// Caliburn Micro
using Caliburn.Micro;

// System Diagnostics for Trace.WriteLine() function
using System.Diagnostics;

// Leap
using Leap;

// Models
using WPF_Leap_Motion_simulator.Models;

// LeapTracker
using WPF_Leap_Motion_simulator.LeapTracker;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class ShellViewModel: Caliburn.Micro.Conductor<object>,
        ILeapEventDelegate, IHandle<HandleInputField>, IHandle<HandleMenuButtonClick>, IHandle<HandleReceiveTheParcelButtonClick>, IHandle<HandleKeyboardChange>
    {
        //-- LeapMotion variables --
        private Controller controller;
        private LeapEventListener listener;
        private Boolean isClosing = false;

        //-- Event Aggregator --
        private readonly IEventAggregator _eventAggregator;

        //-- Global options --
        private Boolean _isRightHandACursor;

        //-- Whole window variables --
        private double _windowWidth;
        private double _windowHeight;
        private double _windowBorderSize;
        private double _windowHeaderSize;
        private double _windowFooterSize;
        private string _FPSCounter;
        private Cursor _Cursor;
        private string _cursorTapAnimationStatus;

        //-- Keyboard variables --
        private Keyboard _keyboard;

        //-- Menu window variables --

        //-- ReceiveTheParcel window variables --
        private string _receiveSMSCode = "";
        private string _receivePhoneNumber = "";

        //-- Constructor --
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            // Setting default global options
            _isRightHandACursor = true;
            _cursorTapAnimationStatus = "OFF";

            // Setting initial window variables
            _windowHeight = 720;
            _windowWidth = 1280;
            _windowBorderSize = 10;
            _windowHeaderSize = 19;
            _windowFooterSize = 13;
            _FPSCounter = 0.ToString(); // Setting FPS Counter to 0

            // Setting default values of the cursor
            _Cursor = new Cursor
            {
                IsVisible = true,
                PositionX = 0,
                PositionY = 0,
                CursorRadius = 9,
                CursorSensibility = 2.5
            };
            NotifyOfPropertyChange(() => ActualCursor);

            //Setting default value for the keyboard
            _keyboard = GetNewKeyboard(KeyboardTypes.NUMERIC);
            NotifyOfPropertyChange(() => ActualKeyboard);

            // Setting Event Aggregator
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            // Initializing controller with LeapEventListener object
            controller = new Controller();
            listener = new LeapEventListener(this);
            controller.AddListener(listener);

            // Activate the Menu window, as first window
            ActivateItem(new MenuViewModel(
                _eventAggregator,
                new TDOWindowSize {
                    WindowWidth = WindowWidth,
                    WindowHeight = WindowHeight
                },
                new TDOWindowPadding {
                    PaddingTop = WindowBorderSize + WindowHeaderSize,
                    PaddingRight = WindowBorderSize,
                    PaddingBottom = WindowBorderSize + WindowFooterSize,
                    PaddingLeft = WindowBorderSize
                }
            ));
        }

        //Method, that's fired when window closes
        protected override void OnDeactivate(bool close)
        {
            isClosing = true;
            controller.RemoveListener(listener);
            controller.Dispose();
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        // -- Leap motion delegate and methods --
        delegate void LeapEventDelegate(LeapEventTypes EventType);
        public void LeapEventNotification(LeapEventTypes EventType)
        {
            bool isCursorAnimationSetted = false;
            if (Application.Current.CheckAccess())
            {
                switch (EventType)
                {
                    case LeapEventTypes.onInit:
                        
                        break;
                    case LeapEventTypes.onConnect:
                        connectHandler();
                        break;
                    case LeapEventTypes.onFrame:
                        if (!isClosing)
                            newFrameHandler(controller.Frame());
                        break;
                    case LeapEventTypes.onExit:
                        
                        break;
                    case LeapEventTypes.onDisconnect:
                        
                        break;
                    case LeapEventTypes.onCircleGestureDetected:
                        
                        break;
                    case LeapEventTypes.onSwipeGestureDetected:
                        _eventAggregator.PublishOnUIThread(new HandleCursorHandGesture
                        {
                            CursorPositionX = _Cursor.PositionX,
                            CursorPositionY = _Cursor.PositionY,
                            CursorRadius = _Cursor.CursorRadius,
                            GestrueType = LeapGestureTypes.Swipe,
                            PaddingTop = WindowBorderSize + WindowHeaderSize,
                            PaddingRight = WindowBorderSize,
                            PaddingBottom = WindowBorderSize + WindowFooterSize,
                            PaddingLeft = WindowBorderSize,
                            WindowWidth = WindowWidth,
                            WindowHeight = WindowHeight
                        });
                        break;
                    case LeapEventTypes.onScreenTapGestureDetected:
                        
                        break;
                    case LeapEventTypes.onKeyTapGestureDetected:
                        if (_Cursor.IsVisible)
                        {
                            _eventAggregator.PublishOnUIThread(new HandleCursorHandGesture
                            {
                                CursorPositionX = _Cursor.PositionX,
                                CursorPositionY = _Cursor.PositionY,
                                CursorRadius = _Cursor.CursorRadius,
                                GestrueType = LeapGestureTypes.KeyTap,
                                PaddingTop = WindowBorderSize + WindowHeaderSize,
                                PaddingRight = WindowBorderSize,
                                PaddingBottom = WindowBorderSize + WindowFooterSize,
                                PaddingLeft = WindowBorderSize,
                                WindowWidth = WindowWidth,
                                WindowHeight = WindowHeight
                            });

                            CheckKeyboardKeyClick();

                            CursorTapAnimationStatus = "ON";
                            NotifyOfPropertyChange(() => CursorTapAnimationStatus);
                            isCursorAnimationSetted = true;
                        }
                        break;
                    case LeapEventTypes.onNoGestureDetected:
                        
                        break;
                }

                if (!isCursorAnimationSetted && (CursorTapAnimationStatus != "OFF"))
                {
                    CursorTapAnimationStatus = "OFF";
                    NotifyOfPropertyChange(() => CursorTapAnimationStatus);
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventType });
            }
        }

        void connectHandler()
        {
            controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE, true);
            controller.Config.SetFloat("Gesture.Circle.MinRadius", 40.0f); //40.0f
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE, true);
            controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP, true);
            controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP, true);
            controller.Config.Save();
        }

        void newFrameHandler(Leap.Frame frame)
        {
            //this.displayID.Content = frame.Id.ToString();
            //this.displayTimestamp.Content = frame.Timestamp.ToString();
            //this.displayIsValid.Content = frame.IsValid.ToString();
            //this.displayGestureCount.Content = frame.Gestures().Count.ToString();
            //this.displayImageCount.Content = frame.Images.Count.ToString();
            //displayFPS.Text = frame.CurrentFramesPerSecond.ToString();

            //Writing the fps number on the screen
            FPSCounter = ((int)frame.CurrentFramesPerSecond).ToString();

            //Saving the position of the cursor and sending this data to other components
            savePositionOfTheCursor(frame);

            //Check if cursor hovers any key
            CheckKeyboardKeyHover();
        }

        //-- Global options Properties --
        public Boolean IsRightHandACursor
        {
            get
            {
                return _isRightHandACursor;
            }

            set
            {
                _isRightHandACursor = value;
            }
        }

        //-- Window Properties --
        public double WindowWidth
        {
            get
            {
                return _windowWidth;
            }

            set
            {
                _windowWidth = value;
                NotifyOfPropertyChange(() => WindowWidth);

                _keyboard = GetNewKeyboard(_keyboard.Type, _keyboard.IsVisible);
                NotifyOfPropertyChange(() => ActualKeyboard);

                _eventAggregator.PublishOnUIThread(new HandleWindowWidth
                {
                    WindowWidth = WindowWidth
                });
            }
        }

        public double WindowHeight
        {
            get
            {
                return _windowHeight;
            }

            set
            {
                _windowHeight = value;
                NotifyOfPropertyChange(() => WindowHeight);

                _keyboard = GetNewKeyboard(_keyboard.Type, _keyboard.IsVisible);
                NotifyOfPropertyChange(() => ActualKeyboard);

                _eventAggregator.PublishOnUIThread(new HandleWindowHeight
                {
                    WindowHeight = WindowHeight
                });
            }
        }

        public double WindowBorderSize
        {
            get
            {
                return _windowBorderSize;
            }

            set
            {
                _windowBorderSize = value;
                NotifyOfPropertyChange(() => WindowBorderSize);
            }
        }

        public double WindowHeaderSize
        {
            get
            {
                return _windowHeaderSize;
            }

            set
            {
                _windowHeaderSize = value;
                NotifyOfPropertyChange(() => WindowHeaderSize);
            }
        }

        public double WindowFooterSize
        {
            get
            {
                return _windowFooterSize;
            }

            set
            {
                _windowHeaderSize = value;
                NotifyOfPropertyChange(() => WindowFooterSize);
            }
        }

        public string ProgramVersion
        {
            get
            {
                return "version 1.0";
            }
        }

        public string FPSCounter
        {
            get
            {
                return _FPSCounter;
            }

            set
            {
                _FPSCounter = value;
                NotifyOfPropertyChange(() => FPSCounter);
            }
        }

        public Cursor ActualCursor
        {
            get
            {
                return _Cursor;
            }
        }

        public Keyboard ActualKeyboard
        {
            get
            {
                return _keyboard;
            }
        }

        public string CursorTapAnimationStatus
        {
            get
            {
                return _cursorTapAnimationStatus;
            }

            set
            {
                _cursorTapAnimationStatus = value;
            }
        }

        //-- Handle change of inputs --
        public void Handle(HandleInputField message)
        {
            // TO DO
            if(message.Type == InputTypes.RECEIVE_SMS_CODE)
            {
                Console.WriteLine("ReceiveSMSCodeInput changed to: " + message.Value);
                _receiveSMSCode = message.Value;
            }
            else if (message.Type == InputTypes.RECEIVE_PHONE_NUMBER)
            {
                Console.WriteLine("ReceivePhoneNumberInput changed to: " + message.Value);
                _receivePhoneNumber = message.Value;
            }
        }

        //-- Handle change of button clicks --
        public void Handle(HandleMenuButtonClick message)
        {
            // TO DO
            if (message.Type == MenuButtonClickTypes.RECEIVE_THE_PARCEL)
            {
                ActivateItem(new ReceiveTheParcelViewModel(
                    _eventAggregator,
                    new TDOWindowSize {
                        WindowWidth = WindowWidth,
                        WindowHeight = WindowHeight
                    },
                    new TDOWindowPadding
                    {
                        PaddingTop = WindowBorderSize + WindowHeaderSize,
                        PaddingRight = WindowBorderSize,
                        PaddingBottom = WindowBorderSize + WindowFooterSize,
                        PaddingLeft = WindowBorderSize
                    }
                ));
            }
        }

        public void Handle(HandleReceiveTheParcelButtonClick message)
        {
            if (message.Type == ReceiveTheParcelButtonClickTypes.MENU)
            {
                _receiveSMSCode = "";
                _receivePhoneNumber = "";

                ActivateItem(new MenuViewModel(
                    _eventAggregator,
                    new TDOWindowSize
                    {
                        WindowWidth = WindowWidth,
                        WindowHeight = WindowHeight
                    },
                    new TDOWindowPadding
                    {
                        PaddingTop = WindowBorderSize + WindowHeaderSize,
                        PaddingRight = WindowBorderSize,
                        PaddingBottom = WindowBorderSize + WindowFooterSize,
                        PaddingLeft = WindowBorderSize
                    }
                ));
            }
            else if (message.Type == ReceiveTheParcelButtonClickTypes.SUCCESS_RECEIVE)
            {
                ActivateItem(new SuccessReceiveViewModel(
                    _eventAggregator,
                    new TDOWindowSize
                    {
                        WindowWidth = WindowWidth,
                        WindowHeight = WindowHeight
                    },
                    new TDOWindowPadding
                    {
                        PaddingTop = WindowBorderSize + WindowHeaderSize,
                        PaddingRight = WindowBorderSize,
                        PaddingBottom = WindowBorderSize + WindowFooterSize,
                        PaddingLeft = WindowBorderSize
                    },
                    _receiveSMSCode
                ));
            }
        }

        // Handle change of keyboard
        public void Handle(HandleKeyboardChange message)
        {
            if (message.IsRestoreDefault)
            {
                _keyboard = GetNewKeyboard(KeyboardTypes.NUMERIC, false);
                NotifyOfPropertyChange(() => ActualKeyboard);
            }
            else
            {
                if (message.Type == _keyboard.Type)
                {
                    if (!message.IsInputChange)
                    {
                        if (message.IsToggle || message.IsVisible != _keyboard.IsVisible)
                        {
                            _keyboard.IsVisible = !_keyboard.IsVisible;
                            NotifyOfPropertyChange(() => ActualKeyboard);
                        }
                    }
                    else
                    {
                        if(!_keyboard.IsVisible)
                        {
                            _keyboard.IsVisible = true;
                            NotifyOfPropertyChange(() => ActualKeyboard);
                        }
                    }
                }
                else //new type
                {
                    Boolean isVisibleToSet = message.IsVisible;
                    if (message.IsToggle || message.IsInputChange) //types doesn't matches and we want to toggle keyboard or we change the input, so we have to render new keyboard
                    {
                        isVisibleToSet = true;
                    }

                    _keyboard = GetNewKeyboard(message.Type, isVisibleToSet);
                    NotifyOfPropertyChange(() => ActualKeyboard);
                }
            }
        }

        //-- Private methods --
        private void savePositionOfTheCursor(Leap.Frame frame)
        {
            bool trackedIndexFinger = false;
            HandList hands = frame.Hands;
            foreach (Hand hand in hands)
            {
                if ((hand.IsRight && _isRightHandACursor) || (hand.IsLeft && !_isRightHandACursor))
                {
                    FingerList fingers = hand.Fingers;
                    foreach (Finger finger in fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_INDEX)
                        {
                            Bone distalBone = finger.Bone(Bone.BoneType.TYPE_DISTAL);
                            Leap.Vector centerOfTheBone = distalBone.Center;
                            trackedIndexFinger = true;
                            setCursorPosition(centerOfTheBone);
                            break;
                        }
                    }
                    break;
                }
            }

            if (!trackedIndexFinger)
            {
                hideCursor();
            }
        }

        private void setCursorPosition(Leap.Vector fingerPosition)
        {
            // -- OLD VERSION OF ACQUIRING WINDOW WIDHT AND HEIGHT
            //Window mainWindow = Application.Current.MainWindow;
            //double windowHeight = mainWindow.ActualHeight;
            //double windowWidth = mainWindow.ActualWidth;

            double centerHeight = WindowHeight / 2;
            double centerWidth = WindowWidth / 2d;

            double offsetX = centerWidth + fingerPosition.x * _Cursor.CursorSensibility;
            double offsetY = centerHeight + fingerPosition.z * _Cursor.CursorSensibility;

            // Send data of the cursor position, if both values (PositionX and PositionY) changes
            if((_Cursor.PositionX != offsetX) && (_Cursor.PositionY != offsetY))
            {
                _eventAggregator.PublishOnUIThread(new HandleCrusorMove
                {
                    CursorPositionX = offsetX,
                    CursorPositionY = offsetY,
                    CursorRadius = _Cursor.CursorRadius,
                    PaddingTop = WindowBorderSize + WindowHeaderSize,
                    PaddingRight = WindowBorderSize,
                    PaddingBottom = WindowBorderSize + WindowFooterSize,
                    PaddingLeft = WindowBorderSize,
                    WindowWidth = WindowWidth,
                    WindowHeight = WindowHeight
                });
            }

            // Setting cursor object fields
            _Cursor.PositionX = offsetX;
            _Cursor.PositionY = offsetY;
            _Cursor.IsVisible = true;
            NotifyOfPropertyChange(() => ActualCursor);
        }

        private void hideCursor()
        {
            _Cursor.PositionX = 0f;
            _Cursor.PositionY = 0f;
            _Cursor.IsVisible = false;
            NotifyOfPropertyChange(() => ActualCursor);
        }

        private Keyboard GetNewKeyboard(KeyboardTypes keyboardType, Boolean isVisible = false)
        {
            double width = (WindowWidth - (WindowBorderSize * 2)) * 5 / 8;
            double height = 300;
            double positionX = (WindowWidth - (WindowBorderSize * 2)) * 1.5 / 8;
            double positionY = WindowHeight - (WindowBorderSize*2 + WindowHeaderSize + WindowFooterSize + height);
            double keyWidth = 75;
            double keyHeight = 57;

            if (keyboardType == KeyboardTypes.NUMERIC)
            {
                // Create key scheme
                double paddingBetween = 10;
                List<Key> keys = KeyScheme.CreateNumericKeyboardKeyScheme(
                    new TDOPosition
                    {
                        PositionX = positionX,
                        PositionY = positionY
                    },
                    width,
                    height,
                    keyWidth,
                    keyHeight,
                    paddingBetween
                );

                return new NumericKeyboard
                {
                    Type = KeyboardTypes.NUMERIC,
                    IsVisible = isVisible,
                    Width = width,
                    Height = height,
                    PositionX = positionX,
                    PositionY = positionY,
                    Keys = keys
                };
            }
            else if (keyboardType == KeyboardTypes.LETTER)
            {
                height = 250;
                keyWidth = 67;
                keyHeight = 60;
                positionY = WindowHeight - (WindowBorderSize * 2 + WindowHeaderSize + WindowFooterSize + height);

                // Create key scheme
                double paddingBetween = 10;
                List<Key> keys = KeyScheme.CreateLetterKeyboardKeyScheme(
                    new TDOPosition
                    {
                        PositionX = positionX,
                        PositionY = positionY
                    },
                    width,
                    height,
                    keyWidth,
                    keyHeight,
                    paddingBetween
                );

                return new LetterKeyboard
                {
                    Type = KeyboardTypes.LETTER,
                    IsVisible = isVisible,
                    Width = width,
                    Height = height,
                    PositionX = positionX,
                    PositionY = positionY,
                    Keys = keys
                };
            }

            return null;
        }

        private void CheckKeyboardKeyClick()
        {
            // Creting cursor object, that has values relative to the keyboard bar
            Cursor relativeCursor = new Cursor
            {
                CursorRadius = _Cursor.CursorRadius,
                PositionX = _Cursor.PositionX - WindowBorderSize - ((WindowWidth - WindowBorderSize * 2) * 1.5 / 8),
                PositionY = _Cursor.PositionY - (WindowBorderSize + WindowHeaderSize) - (WindowHeight - (WindowBorderSize * 2 + WindowHeaderSize + WindowFooterSize + _keyboard.Height))
            };

            //double positionX = (WindowWidth - (WindowBorderSize * 2)) * 1.5 / 8;
            //double positionY = WindowHeight - (WindowBorderSize * 2 + WindowHeaderSize + WindowFooterSize + height);

            _keyboard.Keys.ForEach(key =>
            {
                if (key.IsCursorInsideTheKey(relativeCursor))
                {
                    _eventAggregator.PublishOnUIThread(new HandleKeyClick
                    {
                        KeyType = key.Type,
                        keyToAdd = key.Value
                    });
                }
            });
        }

        private void CheckKeyboardKeyHover()
        {
            // Checking logic
            if (_keyboard.IsVisible)
            {
                // Creting cursor object, that has values relative to the keyboard bar
                Cursor relativeCursor = new Cursor
                {
                    CursorRadius = _Cursor.CursorRadius,
                    PositionX = _Cursor.PositionX - WindowBorderSize - ((WindowWidth - WindowBorderSize * 2) * 1.5 / 8),
                    PositionY = _Cursor.PositionY - (WindowBorderSize + WindowHeaderSize) - (WindowHeight - (WindowBorderSize * 2 + WindowHeaderSize + WindowFooterSize + _keyboard.Height))
                };

                for (int i = 0; i < _keyboard.Keys.Count; i++)
                {
                    Key key = _keyboard.Keys[i];
                    if (key.IsCursorInsideTheKey(relativeCursor))
                    {
                        key.IsHovered = true;
                    }
                    else
                    {
                        key.IsHovered = false;
                    }
                }

                NotifyOfPropertyChange(() => ActualKeyboard);
            }
        }
    }
}
