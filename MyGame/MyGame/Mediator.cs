using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System;
using Helper;
using control;
using XNAnimation;

namespace MyGame
{
    public class Mediator 
    {
        Hashtable hash;

        public Mediator()
        {
            hash = new Hashtable();
        }

        public void register(IEvent ie,params MyEvent[] eventKey)
        {
            foreach (int ev in eventKey)
            {
                if (hash[ev] != null)
                {
                    ((List<IEvent>)hash[ev]).Add(ie);
                }
                else
                {
                    List<IEvent> list = new List<IEvent>();
                    list.Add(ie);
                    hash[ev] = list;
                }
            }
        }

        public void fireEvent(MyEvent ev,params Object[] param)
        {
            if (hash[(int)ev] == null) return;
            List<IEvent> list = (List<IEvent>)hash[(int)ev];
            Event eve = new Event(ev,param);
            foreach (IEvent ie in list)
            {
                ie.addEvent(eve);
            }
        }

        public void controlPointer(float deltaX)
        {
            fireEvent(MyEvent.C_Pointer,"deltaX", deltaX);
        }
    }
}
