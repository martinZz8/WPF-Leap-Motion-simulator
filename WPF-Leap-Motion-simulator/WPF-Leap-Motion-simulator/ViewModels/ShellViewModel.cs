using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

//Caliburn Micro
using Caliburn.Micro;

// System Diagnostics for Trace.WriteLine() function
using System.Diagnostics;

//Leap
using Leap;

//LeapTracker
using WPF_Leap_Motion_simulator.LeapTracker;

namespace WPF_Leap_Motion_simulator.ViewModels
{
    class ShellViewModel: Caliburn.Micro.Conductor<object>, ILeapEventDelegate
    {
        //-- LeapMotion variables --
        private Controller controller;
        private LeapEventListener listener;
        private Boolean isClosing = false;

        //-- Window variables --
        private string _FPSCounter;

        //-- Constructor --
        public ShellViewModel()
        {
            controller = new Controller();
            listener = new LeapEventListener(this);
            controller.AddListener(listener);
            ActivateItem(new MenuViewModel());
        }

        //Method, that's fired when window closes
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            isClosing = true;
            controller.RemoveListener(listener);
            controller.Dispose();
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
            controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            controller.Config.SetFloat("Gesture.Circle.MinRadius", 40.0f);
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
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
        }

        //-- Window properties and methods --
        public string FPSCounter
        {
            get
            {
                return _FPSCounter;
            }

            set
            {
                _FPSCounter = value;
                Console.WriteLine("Changing to: " + value);
                NotifyOfPropertyChange(() => FPSCounter);
            }
        }

        public string ProgramVersion
        {
            get
            {
                return "version 1.0";
            }
        }

        public void LoadReceiveTheParcelScreen()
        {
            //ActivateItem(new ReceiveTheParcelViewModel());
        }
    }
}
