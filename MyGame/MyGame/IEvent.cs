using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper;

namespace MyGame
{
    public interface IEvent
    {
        void addEvent(Event ev);
    }
}
