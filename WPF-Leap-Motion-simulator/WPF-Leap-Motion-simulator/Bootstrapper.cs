using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

//Caliburn Micro
using Caliburn.Micro;

//ViewModels
using WPF_Leap_Motion_simulator.ViewModels;

// System Diagnostics for Trace.WriteLine() function
using System.Diagnostics;

//Leap
using Leap;

//LeapTracker
using WPF_Leap_Motion_simulator.LeapTracker.LeapEventDelegate;
using WPF_Leap_Motion_simulator.LeapTracker.LeapEventListener;

namespace WPF_Leap_Motion_simulator
{
    public class Bootstrapper: BootstrapperBase, ILeapEventDelegate
    {
        private Controller controller;
        private LeapEventListener listener;
        private Boolean isClosing = false;

        private ShellViewModel shellViewModel;

        public Bootstrapper()
        {
            Initialize();
            controller = new Controller();
            listener = new LeapEventListener(this);
            controller.AddListener(listener);
            shellViewModel = IoC.Get<ShellViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            isClosing = true;
            controller.RemoveListener(listener);
            controller.Dispose();
        }

        delegate void LeapEventDelegate(string EventName);
        public void LeapEventNotification(string EventName)
        {
            if (Application.Current.CheckAccess())
            {
                switch (EventName)
                {
                    case "onInit":
                        Debug.WriteLine("Init");
                        break;
                    case "onConnect":
                        connectHandler();
                        break;
                    case "onFrame":
                        if (!isClosing)
                            newFrameHandler(controller.Frame());
                        break;
                    case "onExit":
                        Debug.WriteLine("Exit");
                        break;
                    case "onDisconnect":
                        Debug.WriteLine("Disconnected");
                        break;
                    case "onCircleGestureDetected":
                        Debug.WriteLine("circleGestureDetected");
                        break;
                    case "onSwipeGestureDetected":
                        Debug.WriteLine("swipeGestureDetected");
                        break;
                    case "onScreenTapGestureDetected":
                        Debug.WriteLine("screenTapGestureDetected");
                        break;
                    case "onKeyTapGestureDetected":
                        Debug.WriteLine("keyTapGestureDetected");
                        break;
                    case "onNoGestureDetected":
                        Debug.WriteLine("noGestureDetected");
                        break;
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
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

            //TO CHANGE
            shellViewModel.FPSCounter = frame.CurrentFramesPerSecond.ToString();
            //Console.WriteLine("FPS from obj: "+shellViewModel.FPSCounter);
        }
    }
}
