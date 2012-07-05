using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Helper
{
    /// <summary>
    /// Class repersentign event. each event has a uniqe id and it may contains args.
    /// </summary>
    public class Event
    {
        /// <summary> eventId is an enum to ensure the uniqness of the id.</summary>
        private MyEvent eventId;
        public int EventId
        {
            get
            {
                return (int)eventId;
            }
        }
        /// <summary> hash table containing the args related to this event.</summary>
        public Hashtable args;

        /// <summary>
        /// Constructor of the Event class.
        /// </summary>
        /// <param name="id">the id of the event.</param>
        /// <param name="kv">the set of args</param>
        public Event(MyEvent id, params Object[] kv)
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
