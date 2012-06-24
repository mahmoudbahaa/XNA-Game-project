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
        
        C_FORWARD,
        C_BACKWARD,
        C_LEFT,
        C_RIGHT,
        C_ATTACK_BULLET_BEGIN,
        C_ATTACK_BULLET_END,
        C_ATTACK_STOPPED,
        C_Pointer,

        M_DIE,
        M_BITE,

        G_PAUSE,
        G_RESUME,
        G_StartScreen,
        G_HelpScreen,
        G_StartGame,
        G_StartLevel,
        G_NextLevel,
        G_NextLevel_END_OF_MUSIC,
        G_CreditScreen,
        G_GameOver,
        G_Exit,
    }
}
