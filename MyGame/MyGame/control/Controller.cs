using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace control
{
    /// <summary>
    /// Controller class is an adaptor to use the kinect device gesture detection as keyboard and mouse.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// constants to access specific gesture state
        /// </summary>
        public const int FORWARD = 0;
        public const int BACKWARD = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        public const int RIGHT_HAND_STR = 4;
        public const int POINTER = 5;
        private const int SHOULDER = 6;


        /// <summary>
        /// gesture manager containing all gestures
        /// </summary>
        private GestureManager gestureManager;

        /// <summary>
        /// buffer to hold unconsumed inputs
        /// </summary>
        private Boolean[] activeGesture;

        // to pause the thread 
        //private Boolean pause = false;


        /// <summary>
        /// indicate the pointing hand
        /// </summary>
        private int pointingHand;

        /// <summary>
        /// kinect device that all gesture will use
        /// </summary>
        Kinect kinect;
        /// <summary>
        /// constructor of the controller class
        /// </summary>
        /// <param name="pointingHand">to indicate the pointing hand (left/right)</param>
        public Controller(int pointingHand) 
        {
            kinect = new Kinect();
            this.pointingHand = pointingHand;
            initiGestureManager();
            activeGesture = new Boolean[gestureManager.gestures.Count];

        }

        /// <summary>
        /// initilize gesture that will be used.
        /// </summary>
        private void initiGestureManager()
        {
            gestureManager = new GestureManager(pointingHand);
            gestureManager.AddGesture(new RightLegForward());
            gestureManager.AddGesture(new RightLegBackward());
            gestureManager.AddGesture(new LeanLeft());
            gestureManager.AddGesture(new LeanRight());
            gestureManager.AddGesture(new HandStretchForward((pointingHand+1)%2));
            gestureManager.AddGesture(new HandPointer(pointingHand));
            gestureManager.AddGesture(new shoulderDifference());
            gestureManager.start();
        }

        /// <summary>
        /// update controller state.
        /// </summary>
        private void updateContollerState() 
        {
            activeGesture[FORWARD] = gestureManager.gestures[FORWARD].active;
            activeGesture[BACKWARD] = gestureManager.gestures[BACKWARD].active;
            activeGesture[LEFT] = gestureManager.gestures[LEFT].active;
            activeGesture[RIGHT] = gestureManager.gestures[RIGHT].active;
            activeGesture[RIGHT_HAND_STR] = gestureManager.gestures[RIGHT_HAND_STR].active;
            activeGesture[POINTER] = gestureManager.gestures[POINTER].active;
        }

        /// <summary>
        /// return boolean indicating the state of the gesture.
        /// </summary>
        /// <param name="gesture">the number coresspond to a specific gesture</param>
        public Boolean isActive(int gesture)
        {
            updateContollerState();
            return activeGesture[gesture];
        }

        /// <summary>
        /// return vector2 containing the value indcating the angle between the z axies and the pointing hand.
        /// </summary>
        /// <returns>vector2</returns>
        public Vector2 getPointer()
        {
            return ((HandPointer)gestureManager.gestures[POINTER]).theta;
        }

        /// <summary>
        /// return the z coord. of the left shoulder.
        /// </summary>
        public bool getShoulderLeft()
        {
            return ((shoulderDifference)gestureManager.gestures[SHOULDER]).left;
        }

        /// <summary>
        /// return the z coord. of the right shoulder.
        /// </summary>
        public bool getShoulderRight()
        {
            return ((shoulderDifference)gestureManager.gestures[SHOULDER]).right;
        }

        /// <summary>
        /// return the difference btween z coord. of the left and the right shoulders.
        /// </summary>
        public float getShoulderDiff()
        {
            return ((shoulderDifference)gestureManager.gestures[SHOULDER]).diff;
        }








    }
}
