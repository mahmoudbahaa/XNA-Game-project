using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Helper
{
    public class Event
    {
        public int eventId { get; set; }
        public Hashtable args;

        public Event(int id, params Object[] kv)
        {
            this.eventId = id;
            if (kv.Length == 0)
            {
                this.args = null;
            }
            else
            {
                this.args = new Hashtable();
                for (int i = 0; i < kv.Length; i += 2)
                {
                    this.args[kv[i]] = kv[i + 1];
                }
            }
        }
    }
}
