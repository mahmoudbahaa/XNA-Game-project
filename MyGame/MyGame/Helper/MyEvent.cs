using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public static class MyEvent
    {
        /*
         * P_ player related events
         * M_ monster related events
         * C_ control related events
         */

        public const int P_RUN = 0;
        public const int P_STOP = 7;

        public const int C_FORWARD = 1;
        public const int C_BACKWARD = 2;
        public const int C_LEFT = 3;
        public const int C_RIGHT = 4;
        public const int C_ATTACK = 5;

        public const int M_DIE = 6;
        

    }
}
