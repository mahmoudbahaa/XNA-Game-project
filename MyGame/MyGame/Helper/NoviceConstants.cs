using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Helper
{
    class NoviceConstants:DifficultyConstants
    {
        public NoviceConstants()
        {
            MONSTER_SPEED = Constants.PLAYER_SPEED * 20;
            MONSTER_HEALTH_PER_BULLET = 30;
            PLAYER_HEALTH_DECREASE = 5;
            INCREASED_HEALTH_BY_MEDKIT = 100;
            NUM_MEDKITS_IN_FIELD = 10;
            NUM_MONSTERS_IN_FIELD = 50;
        }
    }
}
