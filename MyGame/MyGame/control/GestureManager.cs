using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace control
{
    /// <summary>
    /// Manage a collection of gesture and perodically evaluate them.
    /// </summary>
    class GestureManager
    {
        /// <summary>
        /// list to hold collection of gestures.
        /// </summary>
        public List<Gesture> gestures { get; private set; }
        /// <summary>
        /// to specify pointing hand.
        /// </summary>
        private int pointingHand;
        /// <summary>
        /// Thread to evaluate the list of gesture perodically, separately from the main context execution.
        /// </summary>
        private Thread thread;
        /// <summary>
        /// true while the game is running.
        /// </summary>
        public static bool running = true;
        /// <summary>
        /// true while the game is paused.
        /// </summary>
        public static bool paused = false;

        /// <summary>
        /// Constructor of the GestuerManager Class
        /// </summary>
        /// <param name="pointingHand">A spsecific hand to will be used for pointing(aiming)</param>
        public GestureManager(int pointingHand)
        {
            this.pointingHand = pointingHand;
            gestures = new List<Gesture>();
            thread = new Thread(Run);
        }

        /// <summary>
        /// to start the evaluation thread.
        /// </summary>
        public void start()
        {
            thread.Start();
        }

        /// <summary>
        /// to add a new gesture to the list.
        /// </summary>
        /// <param name="g">the gesture to be added</param>
        public void AddGesture(Gesture g)
        {
            gestures.Add(g);
        }
        /// <summary>
        /// update(evaluate) the state of all gesture.
        /// </summary>
        public void updateState()
        {
            foreach (Gesture g in gestures)
                g.eval();
        }


        /// <summary>
        /// the function that will be runing in the sperated thread.
        /// </summary>       
        public void Run()
        {
            while (running)
            {
                // if the game is pause check every 1 sec
                if (paused)
                {
                    foreach (Gesture g in gestures)
                        g.active = false;
                    Thread.Sleep(1000);
                }
                else // if the game is running evaluate the list of gesture almost 33 time/sec.
                {
                    updateState();
                    Thread.Sleep(30);
                }
            }

        }
    }
}
