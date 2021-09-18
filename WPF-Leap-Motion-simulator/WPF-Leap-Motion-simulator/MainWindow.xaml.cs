using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// System Diagnostics for Trace.WriteLine() function
using System.Diagnostics;

//Leap
using Leap;

//LeapTracker
using WPF_Leap_Motion_simulator.LeapTracker.LeapEventDelegate;
using WPF_Leap_Motion_simulator.LeapTracker.LeapEventListener;

namespace WPF_Leap_Motion_simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILeapEventDelegate
    {
        private Controller controller;
        private LeapEventListener listener;
        private Boolean isClosing = false;

        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller();
            listener = new LeapEventListener(this);
            controller.AddListener(listener);
        }

        delegate void LeapEventDelegate(string EventName);
        public void LeapEventNotification(string EventName)
        {
            if (CheckAccess())
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
                Dispatcher.Invoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
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
            displayFPS.Text = frame.CurrentFramesPerSecond.ToString();
            //this.displayIsValid.Content = frame.IsValid.ToString();
            //this.displayGestureCount.Content = frame.Gestures().Count.ToString();
            //this.displayImageCount.Content = frame.Images.Count.ToString();
        }

        void MainWindow_Closing(object sender, EventArgs e)
        {
            isClosing = true;
            controller.RemoveListener(listener);
            controller.Dispose();
        }
    }
}
