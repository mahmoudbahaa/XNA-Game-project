using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MyGame;

namespace control
{
    class GestureManager
    {

        public List<Gesture> gestures { get; private set; }
        private int pointingHand;
        private Thread thread;

        Game1 game;

        public GestureManager(Game1 game, int pointingHand)
        {
            this.pointingHand = pointingHand;
            this.game = game;
            gestures = new List<Gesture>();
            thread = new Thread(Run);
            init();
        }

        private void init()
        {
            gestures.Add(new RightLegForward());
            gestures.Add(new RightLegBackward());
            gestures.Add(new LeanLeft());
            gestures.Add(new LeanRight());
            gestures.Add(new HandStretchForward((pointingHand + 1) % 2));
            gestures.Add(new HandPointer(pointingHand));
            start();
        }

        public void start()
        {
            thread.Start();
        }

        public void updateState()
        {
            foreach (Gesture g in gestures)
            {
                g.eval();
                if (g.active)
                    game.fireEvent(0, g.getArgs());
            }
        }

        public void Run()
        {
          while (true)
          {
              updateState();
              Thread.Sleep(30);
          }

        }



    }
}
