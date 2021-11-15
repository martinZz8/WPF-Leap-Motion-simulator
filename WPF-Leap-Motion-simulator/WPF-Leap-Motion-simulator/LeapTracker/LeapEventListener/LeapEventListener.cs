using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

// Leap
using Leap;

namespace WPF_Leap_Motion_simulator.LeapTracker
{
    public class LeapEventListener : Listener
    {
        ILeapEventDelegate eventDelegate;

        public LeapEventListener(ILeapEventDelegate delegateObject)
        {
            eventDelegate = delegateObject;
        }

        public override void OnInit(Controller controller)
        {
            Console.WriteLine("Initialized");
            eventDelegate.LeapEventNotification(LeapEventTypes.onInit);
        }

        // OnConnect is after OnInit
        public override void OnConnect(Controller controller)
        {
            controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE, true);
            controller.Config.SetFloat("Gesture.Circle.MinRadius", 40.0f); //40.0f
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE, true);
            controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP, true);
            controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP, true);
            controller.Config.Save();

            Console.WriteLine("Connected");
            eventDelegate.LeapEventNotification(LeapEventTypes.onConnect);
        }

        public override void OnExit(Controller controller)
        {
            Console.WriteLine("Exited");
            eventDelegate.LeapEventNotification(LeapEventTypes.onExit);
        }
        public override void OnDisconnect(Controller controller)
        {
            Console.WriteLine("Disconnected");
            eventDelegate.LeapEventNotification(LeapEventTypes.onDisconnect);
        }

        public override void OnFrame(Controller controller)
        {
            //Processing code of latest acquired frame
            Frame frame = controller.Frame();
            //Console.WriteLine("----------Frame id: " + frame.Id.ToString() + "----------------");
            //Console.WriteLine("Frame timestamp: " + frame.Timestamp.ToString());
            //Console.WriteLine("Current FPS: " + frame.CurrentFramesPerSecond.ToString());
            //Console.WriteLine("Is valid: " + frame.IsValid.ToString());
            //Console.WriteLine("Frame gestures count: " + frame.Gestures().Count.ToString());
            //Console.WriteLine("Images count: " + frame.Images.Count.ToString());
            //Console.Write("\n\n");
            GestureDetection(frame.Gestures());

            eventDelegate.LeapEventNotification(LeapEventTypes.onFrame);
        }

        private void GestureDetection(GestureList gesture_list)
        {
            bool gd = false;
            //if(gesture_list.Count > 0)
            //{
            //    Console.WriteLine("Gesture length: " + gesture_list.Count);
            //}
            foreach (Gesture gesture in gesture_list)
            {
                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPE_CIRCLE:
                        gd = true;
                        eventDelegate.LeapEventNotification(LeapEventTypes.onCircleGestureDetected);
                        Console.WriteLine("GESTURE TYPE_CIRCLE");
                        break;
                    case Gesture.GestureType.TYPE_SWIPE:
                        gd = true;
                        eventDelegate.LeapEventNotification(LeapEventTypes.onSwipeGestureDetected);
                        Console.WriteLine("GESTURE TYPE_SCREEN_SWIPE");
                        break;
                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        gd = true;
                        eventDelegate.LeapEventNotification(LeapEventTypes.onScreenTapGestureDetected);
                        Console.WriteLine("GESTURE TYPE_SCREEN_TAP");
                        break;
                    case Gesture.GestureType.TYPE_KEY_TAP:
                        gd = true;
                        Console.WriteLine("GESTURE TYPE_KEY_TAP");
                        eventDelegate.LeapEventNotification(LeapEventTypes.onKeyTapGestureDetected);
                        break;
                    default:
                        //Handle unrecognized gestures
                        Console.WriteLine("unrecognized gesture");
                        break;
                }
            }

            if (!gd)
            {
                eventDelegate.LeapEventNotification(LeapEventTypes.onNoGestureDetected);
            }
                
        }
    }
}