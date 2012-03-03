using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace control
{
    class Controller
    {
        //constants to access specific gesture state
        public const int FORWARD = 0;
        public const int BACKWARD = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        public const int RIGHT_HAND_STR = 4;
        private const int POINTER = 5;
        
        //frame per second
        private const int FPS = 35;

        //gesture manager containing all gestures
        private GestureManager gestureManager;

        //buffer to hold unconsumed inputs
        private Boolean[,] activeGestureBuffer;

        // to pause the thread 
        private Boolean pause = false;

        //sync attributte
        private int index = 0;
        private Mutex mutex;
        private int producerCount = 0;

        //indicate the pointing hand
        private int pointingHand;

        //kinect device that all gesture will use
        Kinect kinect;
        public Controller(int pointingHand) 
        {
            kinect = new Kinect();
            this.pointingHand = pointingHand;
            initiGestureManager();
            activeGestureBuffer = new Boolean[FPS, gestureManager.gestures.Count];
            mutex = new Mutex(false);
            //Thread thread = new Thread(buffering);
            //thread.Start();

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

        private void updateContollerState(int i) 
        {
            activeGestureBuffer[i, FORWARD] = gestureManager.gestures[FORWARD].active;
            activeGestureBuffer[i, BACKWARD] = gestureManager.gestures[BACKWARD].active;
            activeGestureBuffer[i, LEFT] = gestureManager.gestures[LEFT].active;
            activeGestureBuffer[i, RIGHT] = gestureManager.gestures[RIGHT].active;
            activeGestureBuffer[i, RIGHT_HAND_STR] = gestureManager.gestures[RIGHT_HAND_STR].active;
        }

        public Boolean isActiveV2(int gesture)
        {
            bool active;
            if (producerCount <= 0)
            {
                return false;
            }
            else
            {
                
                mutex.WaitOne();
                producerCount--;
                index = (index + 1) % FPS;
                active = activeGestureBuffer[index, gesture];
                mutex.ReleaseMutex();
                Console.WriteLine(producerCount);
                return active;
            }
        }

        private void buffering()
        {
            int i = 0;
            while (true)
            {
                Thread.Sleep(30);
                if (!pause || producerCount >= FPS)
                {
                    updateContollerState(i);
                    i = (i + 1) % FPS;
                    mutex.WaitOne();
                    producerCount++;
                    mutex.ReleaseMutex();
                }
            }
        }
        public Boolean isActive(int gesture)
        {
            updateContollerState(0);
            return activeGestureBuffer[0, gesture];
        }

        
        public Vector2 getPointer()
        {
            return ((HandPointer)gestureManager.gestures[POINTER]).theta;
        }








    }
}
