using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Helper
{
    class AdvancedConstants:DifficultyConstants
    {
        public AdvancedConstants()
        {
            MONSTER_SPEED = Constants.PLAYER_SPEED * 30;
            MONSTER_HEALTH_PER_BULLET = 20;
            PLAYER_HEALTH_DECREASE = 10;
            INCREASED_HEALTH_BY_MEDKIT = 50;
            NUM_MEDKITS_IN_FIELD = 5;
            NUM_MONSTERS_IN_FIELD = 75;
        }
    }
}
