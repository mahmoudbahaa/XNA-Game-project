using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper;

namespace MyGame
{
    /// <summary>
    /// This class represent IEvent that is implemented by all those who which to listen to events
    /// </summary>
    public interface IEvent
    {
        void addEvent(Event ev);
    }
}
