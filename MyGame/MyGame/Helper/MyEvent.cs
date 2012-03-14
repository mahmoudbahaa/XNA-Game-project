using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public enum MyEvent
    {
        /*
         * P_ player related events
         * M_ monster related events
         * C_ control related events
         */

        P_RUN = 0,
        P_STOP,
        P_ATTACK_AXE1,
        P_ATTACK_AXE2,
        P_ATTACK_AXE3,
        P_ATTACK_AXE4,
        P_ATTACK_AXE5,
        
        C_FORWARD,
        C_BACKWARD,
        C_LEFT,
        C_RIGHT,
        C_ATTACK_BULLET_BEGIN,
        C_ATTACK_BULLET_END,
        C_ATTACK_AXE,
        C_Pointer,

        M_DIE,
    }
}
