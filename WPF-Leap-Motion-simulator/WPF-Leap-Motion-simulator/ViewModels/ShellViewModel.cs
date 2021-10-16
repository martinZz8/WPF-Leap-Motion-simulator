using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

//Caliburn Micro
using Caliburn.Micro;
using WPF_Leap_Motion_simulator.Models;

// System Diagnostics for Trace.WriteLine() function
using System.Diagnostics;

//Leap
using Leap;

//LeapTracker
using WPF_Leap_Motion_simulator.LeapTracker;
using System.Windows.Controls;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class ShellViewModel: Caliburn.Micro.Conductor<object>, ILeapEventDelegate, IHandle<InputField>, IHandle<MenuButtonClick>
    {
        //-- LeapMotion variables --
        private Controller controller;
        private LeapEventListener listener;
        private Boolean isClosing = false;

        //-- Event Aggregator --
        private readonly IEventAggregator _eventAggregator;

        //-- Whole window variables --
        private string _FPSCounter;
        private Cursor _Cursor;

        //-- Menu window variables --
        private string _testInput;

        //-- Constructor --
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            // Setting FPS Counter to 0
            _FPSCounter = 0.ToString();

            // Setting default values of the cursor
            _Cursor = new Cursor
            {
                IsVisible = true,
                PositionX = 0,
                PositionZ = 0
            };
            NotifyOfPropertyChange(() => ActualCursor);

            // Setting Event Aggregator
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            // Initializing controller with LeapEventListener object
            controller = new Controller();
            listener = new LeapEventListener(this);
            controller.AddListener(listener);

            // Activate the Menu window, as first window
            ActivateItem(new MenuViewModel(_eventAggregator));
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
            if (Application.Current.CheckAccess())
            {
                switch (EventType)
                {
                    case LeapEventTypes.onInit:
                        Debug.WriteLine("Init");
                        break;
                    case LeapEventTypes.onConnect:
                        connectHandler();
                        break;
                    case LeapEventTypes.onFrame:
                        if (!isClosing)
                            newFrameHandler(controller.Frame());
                        break;
                    case LeapEventTypes.onExit:
                        Debug.WriteLine("Exit");
                        break;
                    case LeapEventTypes.onDisconnect:
                        Debug.WriteLine("Disconnected");
                        break;
                    case LeapEventTypes.onCircleGestureDetected:
                        Debug.WriteLine("circleGestureDetected");
                        break;
                    case LeapEventTypes.onSwipeGestureDetected:
                        Debug.WriteLine("swipeGestureDetected");
                        break;
                    case LeapEventTypes.onScreenTapGestureDetected:
                        Debug.WriteLine("screenTapGestureDetected");
                        break;
                    case LeapEventTypes.onKeyTapGestureDetected:
                        Debug.WriteLine("keyTapGestureDetected");
                        break;
                    case LeapEventTypes.onNoGestureDetected:
                        Debug.WriteLine("noGestureDetected");
                        break;
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

            //Saving the position of the cursor
            savePositionOfTheCursor(frame);
        }

        //-- Window Properties --
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

        public string TestInput
        {
            get
            {
                return _testInput;
            }

            set
            {
                _testInput = value;
                NotifyOfPropertyChange(() => TestInput);
            }
        }

        public Cursor ActualCursor
        {
            get
            {
                return _Cursor;
            }
        }

        //-- Handle change of inputs --
        public void Handle(InputField message)
        {
            // TO DO
            if(message.Name == "testInput")
            {
                Console.WriteLine("Input changed to: " + message.Value);
                TestInput = message.Value;
            }
        }

        //-- Handle change of button clicks --
        public void Handle(MenuButtonClick message)
        {
            // TO DO
            if (message.Name == "receiveTheParcel")
            {
                ActivateItem(new ReceiveTheParcelViewModel(_eventAggregator));
            }
            else if (message.Name == "menu")
            {
                ActivateItem(new MenuViewModel(_eventAggregator));
            }
        }

        //-- Private methods --
        private void savePositionOfTheCursor(Leap.Frame frame)
        {
            bool trackedIndexFinger = false;
            HandList hands = frame.Hands;
            foreach (Hand hand in hands)
            {
                if (hand.IsRight)
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
            // TO DO - Calculate properly the position of the cursor
            Window mainWindow = Application.Current.MainWindow;
            double windowHeight = mainWindow.ActualHeight;
            double windowWidth = mainWindow.ActualWidth;

            double centerHeight = windowHeight / 2;
            double centerWidth = windowWidth / 2d;

            double cursorSensibility = 2;
            double offsetX = centerWidth + fingerPosition.x* cursorSensibility;
            double offsetZ = centerHeight + fingerPosition.z* cursorSensibility;

            // check if values are not over the window
            double maxLeft = _Cursor.CursorRadius;
            double maxRight = windowWidth - _Cursor.CursorRadius;
            double maxTop = _Cursor.CursorRadius;
            double maxBottom = windowHeight - _Cursor.CursorRadius;

            if (offsetX < maxLeft)
            {
                offsetX = maxLeft;
            }
            else if(offsetX > maxRight)
            {
                offsetX = maxRight;
            }

            if (offsetZ < maxTop)
            {
                offsetZ = maxTop;
            }
            else if (offsetZ > maxBottom)
            {
                offsetZ = maxBottom;
            }

            // Setting cursor object fields
            _Cursor.PositionX = offsetX;
            _Cursor.PositionZ = offsetZ;
            _Cursor.IsVisible = true;
            NotifyOfPropertyChange(() => ActualCursor);

            Console.WriteLine($"X: {_Cursor.PositionX}\tZ: {_Cursor.PositionZ}");
        }

        private void hideCursor()
        {
            _Cursor.PositionX = 0f;
            _Cursor.PositionZ = 0f;
            _Cursor.IsVisible = false;
            NotifyOfPropertyChange(() => ActualCursor);
        }
    }
}
