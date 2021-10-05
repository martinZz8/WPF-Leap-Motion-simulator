using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;
using System.Threading;

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

        public override void OnConnect(Controller controller)
        {
            controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            controller.Config.SetFloat("Gesture.Circle.MinRadius", 40.0f);
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);

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
            Console.WriteLine("----------Frame id: " + frame.Id.ToString() + "----------------");
            Console.WriteLine("Frame timestamp: " + frame.Timestamp.ToString());
            Console.WriteLine("Current FPS: " + frame.CurrentFramesPerSecond.ToString());
            Console.WriteLine("Is valid: " + frame.IsValid.ToString());
            Console.WriteLine("Frame gestures count: " + frame.Gestures().Count.ToString());
            Console.WriteLine("Images count: " + frame.Images.Count.ToString());
            Console.Write("\n\n");
            GestureDetection(frame.Gestures());

            //TO DELETE
            //if(frame.Fingers.Count > 0)
            //{
            //    Console.WriteLine("Finger one: " +frame.Fingers[0].Bone(Bone.BoneType.TYPE_DISTAL).Length.ToString());
            //}
            

            eventDelegate.LeapEventNotification(LeapEventTypes.onFrame);
            //Thread.Sleep(1000);
        }

        private void GestureDetection(GestureList gesture_list)
        {
            bool gd = false;
            Console.WriteLine("HH Gesture length: \n" + gesture_list.Count);
            foreach (Gesture gesture in gesture_list)
            {
                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPE_CIRCLE:
                        gd = true;
                        eventDelegate.LeapEventNotification(LeapEventTypes.onCircleGestureDetected);
                        break;
                    case Gesture.GestureType.TYPE_SWIPE:
                        gd = true;
                        eventDelegate.LeapEventNotification(LeapEventTypes.onSwipeGestureDetected);
                        break;
                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        gd = true;
                        eventDelegate.LeapEventNotification(LeapEventTypes.onScreenTapGestureDetected);
                        break;
                    case Gesture.GestureType.TYPE_KEY_TAP:
                        gd = true;
                        eventDelegate.LeapEventNotification(LeapEventTypes.onKeyTapGestureDetected);
                        break;
                    default:
                        //Handle unrecognized gestures
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