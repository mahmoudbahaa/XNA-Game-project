using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace control
{
    class GestureManager
    {

        public List<Gesture> gestures { get; private set; }
        private int pointingHand;
        private Thread thread;
        public static bool running = true;

        public GestureManager(int pointingHand)
        {
            this.pointingHand = pointingHand;
            gestures = new List<Gesture>();
            thread = new Thread(Run);
        }

        public void start()
        {
            thread.Start();
        }

        public void AddGesture(Gesture g)
        {
            gestures.Add(g);
        }

        public void updateState()
        {
            foreach (Gesture g in gestures)
                g.eval();
        }

      public void Run()
      {
          while (running)
          {
              updateState();
              Thread.Sleep(30);
          }

      }



    }
}
