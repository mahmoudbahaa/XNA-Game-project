using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace control
{
    public class Controller
    {
        //constants to access specific gesture state
        public const int FORWARD = 0;
        public const int BACKWARD = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        public const int RIGHT_HAND_STR = 4;
        private const int POINTER = 5;


        //gesture manager containing all gestures
        private GestureManager gestureManager;

        //buffer to hold unconsumed inputs
        private Boolean[] activeGesture;

        // to pause the thread 
        //private Boolean pause = false;


        //indicate the pointing hand
        private int pointingHand;

        //kinect device that all gesture will use
        Kinect kinect;
        public Controller(int pointingHand) 
        {
            kinect = new Kinect();
            this.pointingHand = pointingHand;
            initiGestureManager();
            activeGesture = new Boolean[gestureManager.gestures.Count];

        }

        private void initiGestureManager()
        {
            gestureManager = new GestureManager(pointingHand);
            gestureManager.AddGesture(new RightLegForward());
            gestureManager.AddGesture(new RightLegBackward());
            gestureManager.AddGesture(new LeanLeft());
            gestureManager.AddGesture(new LeanRight());
            gestureManager.AddGesture(new HandStretchForward((pointingHand+1)%2));
            gestureManager.AddGesture(new HandPointer(pointingHand));
            gestureManager.start();
        }

        private void updateContollerState() 
        {
            activeGesture[FORWARD] = gestureManager.gestures[FORWARD].active;
            activeGesture[BACKWARD] = gestureManager.gestures[BACKWARD].active;
            activeGesture[LEFT] = gestureManager.gestures[LEFT].active;
            activeGesture[RIGHT] = gestureManager.gestures[RIGHT].active;
            activeGesture[RIGHT_HAND_STR] = gestureManager.gestures[RIGHT_HAND_STR].active;
        }

        public Boolean isActive(int gesture)
        {
            updateContollerState();
            return activeGesture[gesture];
        }

        
        public Vector2 getPointer()
        {
            return ((HandPointer)gestureManager.gestures[POINTER]).theta;
        }








    }
}
